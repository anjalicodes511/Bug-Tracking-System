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
        private readonly TokenService _token;
        public AuthService(UserRepository repo, EmailService emailService, PasswordService passwordServic, TokenService tokenService) {
            _repo = repo;
            _email = emailService;
            _password = passwordServic;
            _token = tokenService;
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

            string token = _token.Generate();

            User user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = _password.Hash(model.Password),
                EmailVerificationToken = token,
                EmailTokenExpiry = DateTime.UtcNow.AddHours(24),
                IsEmailVerified = false
            };

            _repo.Create(user);
        }
    }
}