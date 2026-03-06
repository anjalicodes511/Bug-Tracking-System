using Bug_Tracking_System.Models.Entities;
using Bug_Tracking_System.Models.VM;
using Bug_Tracking_System.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracking_System.Services
{
    public class AuthService
    {
        private readonly UserRepository _repo;
        private readonly EmailService _email;
        private readonly PasswordService _password;
        private readonly OtpService _otp;
        private readonly OtpRepository _otpRepository;
        public AuthService(UserRepository repo, EmailService emailService, PasswordService passwordService, OtpService otpService, OtpRepository otpRepo) {
            _repo = repo;
            _email = emailService;
            _password = passwordService;
            _otp = otpService;
            _otpRepository = otpRepo;
        }
        public void Register(RegisterVM model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model)); 
            }

            var existingUser = _repo.GetByEmail(model.Email);
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
            _repo.Create(user);
            var newUser = _repo.GetByEmail(model.Email);

            var otp = _otp.GenerateOTP();
            EmailOtp emailotp = new EmailOtp
            {
                UserId = newUser.Id,
                OtpCode = otp,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5)
            };
            _otpRepository.Create(emailotp);
            

        }
    }
}