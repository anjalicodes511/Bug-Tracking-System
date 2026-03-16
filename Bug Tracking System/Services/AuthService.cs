using Bug_Tracking_System.Exceptions;
using Bug_Tracking_System.Models.Entities;
using Bug_Tracking_System.Models.VM;
using Bug_Tracking_System.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.WebPages;
using static System.Net.WebRequestMethods;

namespace Bug_Tracking_System.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly EmailService _email;
        private readonly PasswordService _password;
        private readonly OtpService _otpService;
        private readonly OtpRepository _otpRepository;
        public AuthService(UserRepository repo, EmailService emailService, PasswordService passwordService, OtpService otpService, OtpRepository otpRepo) {
            _userRepository = repo;
            _email = emailService;
            _password = passwordService;
            _otpService = otpService;
            _otpRepository = otpRepo;
        }
        public User Register(RegisterVM model)
        {
            Debug.WriteLine("Email: " + model.Email);
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model)); 
            }

            var existingUser = _userRepository.GetByEmail(model.Email);
            if (existingUser != null) {
                Debug.WriteLine("Email: " + model.Email);
                throw new BusinessException("Email Already Exists!!");
            }

            User user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = _password.Hash(model.Password),
                IsEmailVerified = false
            };
            _userRepository.Create(user);

            var otp = _otpService.GenerateOTP();
            var newUser = _userRepository.GetByEmail(model.Email);
            EmailOtp emailotp = new EmailOtp
            {
                UserId = newUser.Id,
                OtpCode = otp,
                ExpiryTime = DateTime.Now.AddMinutes(5)
            };

            //if (emailotp == null)
            //{
            //    Debug.WriteLine("EmailOtp is NULL");
            //}
            //else
            //{
            //    Debug.WriteLine("User ID"+emailotp.UserId );
            //    Debug.WriteLine("OtpCode" + emailotp.OtpCode);
            //    Debug.WriteLine("ExpiryTime" + emailotp.ExpiryTime);
            //}

            string to = model.Email;
            string subject = "Verify Your Email";
            string body = BuildOtpEmail(otp, false);

            _email.SendEmail(to, subject, body);
            _otpRepository.Create(emailotp);

            return newUser;
        }

        public void VerifyOtp(int userId, string otp)
        {
            var user = _userRepository.GetById(userId);

            if (user == null)
            {
                throw new BusinessException("User not found.");
            }

            if (user.IsBlocked && user.BlockedUntil > DateTime.Now)
            {
                throw new BusinessException("Account is temporarily blocked. Try again later.");
            }

            if (user.IsBlocked && user.BlockedUntil <= DateTime.Now)
            {
                user.IsBlocked = false;
                user.BlockedUntil = null;
                _userRepository.Update(user);
            }

            var record = _otpRepository.GetLatestOtp(userId);

            if (record == null)
            {
                throw new BusinessException("OTP not found. Please request a new one.");
            }

            if (record.IsUsed){
                throw new BusinessException("OTP already used.");
            }

            if (record.ExpiryTime < DateTime.Now)
            {
                record.IsUsed = true;
                _otpRepository.Update(record);
                throw new BusinessException("OTP expired. Please request a new one.");
            }

            if (record.OtpCode != otp)
            {
                record.AttemptCount++;

                if (record.AttemptCount >= 5)
                {
                    record.IsUsed = true;

                    user.IsBlocked = true;
                    user.BlockedUntil = DateTime.Now.AddMinutes(15);

                    _userRepository.Update(user);
                }

                _otpRepository.Update(record);
                throw new BusinessException("Invalid OTP.");
            }

            record.IsUsed = true;
            _otpRepository.Update(record);

            user.IsEmailVerified = true;
            _userRepository.Update(user);
        }
    
        public void ResendOtp(int userId)
        {
            var user = _userRepository.GetById(userId);

            if (user.IsBlocked && user.BlockedUntil > DateTime.Now)
            {
                throw new BusinessException("Account is temporarily blocked. Try again later.");
            }

            var oldOtp = _otpRepository.GetLatestOtp(userId);

            if (oldOtp != null)
            {
                if(oldOtp.CreatedAt.AddSeconds(60) > DateTime.Now)
                {
                    throw new BusinessException("Please Wait Before Requesting Another OTP");
                }

                oldOtp.IsUsed = true;
                _otpRepository.Update(oldOtp);
            }

            var newOtp = _otpService.GenerateOTP();

            EmailOtp emailotp = new EmailOtp
            {
                UserId = userId,
                OtpCode = newOtp,
                ExpiryTime = DateTime.Now.AddMinutes(5)
            };

            string to = user.Email;
            string subject = "Your New OTP - Email Verification";

            string body = BuildOtpEmail(newOtp, true);
            _email.SendEmail(to, subject, body);
            _otpRepository.Create(emailotp);
        }


        private string BuildOtpEmail(string otp, bool isResend)
        {
            string message = isResend
                ? "You requested a new OTP to verify your email address."
                : "Thank you for registering. Please use the OTP below to verify your email address.";

                return $@"
                        <html>
                        <body style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding:20px;'>

                            <div style='max-width:500px; margin:auto; background:white; padding:25px; border-radius:8px;'>

                                <h2 style='color:#333;'>Email Verification</h2>

                                <p>Hello,</p>

                                <p>{message}</p>
                                <div style='text-align:center; margin:20px 0;'>
                                    <span style='font-size:28px; font-weight:bold; letter-spacing:4px; color:#007bff;'>
                                        {otp}
                                    </span>
                                </div>

                                <p>This OTP will expire in <b>5 minutes</b>.</p>

                                <p>If you did not request this, please ignore this email.</p>

                                <hr/>

                                <p style='font-size:12px; color:#888;'>
                                    Bug Tracking System
                                </p>

                            </div>

                        </body>
                        </html>";
        }
    
    
        public void Login(LoginVM model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            var user = _userRepository.GetByEmail(model.Email);
            if (user == null || !_password.Verify(user.PasswordHash, model.Password))
            {
                throw new BusinessException("Invalid Email or Password");
            }

            if (user.IsBlocked && user.BlockedUntil > DateTime.Now)
            {
                throw new BusinessException("Account is temporarily blocked. Try again later.");
            }
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
        }

        public User ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessException("Email is required.");
            }

            var user = _userRepository.GetByEmail(email);

            if (user == null)
            {
                throw new BusinessException("Invalid Email.");
            }

            if (user.IsBlocked && user.BlockedUntil > DateTime.Now)
            {
                throw new BusinessException("Account is temporarily blocked. Try again later.");
            }
            var oldOtp = _otpRepository.GetLatestOtp(user.Id);

            if (oldOtp != null)
            {
                if (oldOtp.CreatedAt.AddSeconds(60) > DateTime.Now)
                {
                    throw new BusinessException("Please wait before requesting another OTP.");
                }

                oldOtp.IsUsed = true;
                _otpRepository.Update(oldOtp);
            }

            var otp = _otpService.GenerateOTP();

            EmailOtp emailOtp = new EmailOtp
            {
                UserId = user.Id,
                OtpCode = otp,
                ExpiryTime = DateTime.Now.AddMinutes(5)
            };

            string subject = "Reset Your Password - OTP";
            string body = BuildOtpEmail(otp, false);

            _email.SendEmail(user.Email, subject, body);

            _otpRepository.Create(emailOtp);

            return user;
        }

        public void VerifyForgotPasswordOtp(int userId, string otp)
        {
            var record = _otpRepository.GetLatestOtp(userId);

            if (record == null)
            {
                throw new BusinessException("OTP not found.");
            }

            if (record.IsUsed)
            {
                throw new BusinessException("OTP already used.");
            }

            if (record.ExpiryTime < DateTime.Now)
            {
                record.IsUsed = true;
                _otpRepository.Update(record);

                throw new BusinessException("OTP expired.");
            }

            if (record.OtpCode != otp)
            {
                record.AttemptCount++;
                _otpRepository.Update(record);

                throw new BusinessException("Invalid OTP.");
            }

            record.IsUsed = true;
            _otpRepository.Update(record);
        }


    }
}