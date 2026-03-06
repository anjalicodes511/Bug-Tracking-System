using Bug_Tracking_System.Models.Entities;
using Bug_Tracking_System.Models.VM;
using Bug_Tracking_System.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.WebPages;

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
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model)); 
            }

            var existingUser = _userRepository.GetByEmail(model.Email);
            if (existingUser != null) {
                throw new Exception("Email Already Exists!!");
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
            string body = $@"
                            <html>
                            <body style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding:20px;'>
                                <div style='max-width:500px; margin:auto; background:white; padding:25px; border-radius:8px;'>
        
                                    <h2 style='color:#333;'>Email Verification</h2>
        
                                    <p>Hello,</p>
        
                                    <p>Thank you for registering. Please use the OTP below to verify your email address.</p>
        
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
                            </html>
                            ";

            _email.SendEmail(to, subject, body);
            _otpRepository.Create(emailotp);

            return newUser;
        }

        public bool VerifyOtp(int userId, string otp)
        {
            var record = _otpRepository.GetLatestOtp(userId);

            if (record == null)
                return false;

            if (record.IsUsed)
                return false;

            if (record.ExpiryTime < DateTime.Now)
                return false;

            if (record.AttemptCount >= 5)
                return false;

            if (record.OtpCode != otp)
            {
                record.AttemptCount++;
                _otpRepository.Update(record);
                return false;
            }

            record.IsUsed = true;
            _otpRepository.Update(record);

            var user = _userRepository.GetById(userId);
            user.IsEmailVerified = true;
            _userRepository.Update(user);

            return true;
        }
    }
}