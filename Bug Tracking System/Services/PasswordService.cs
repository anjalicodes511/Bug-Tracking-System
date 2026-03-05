using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Bug_Tracking_System.Services
{
    public class PasswordService
    {
        PasswordHasher hasher = new PasswordHasher();

        public string Hash(string password)
        {
            return hasher.HashPassword(password);
        }

        public bool Verify(string hash, string password)
        {
            return hasher.VerifyHashedPassword(hash, password) == PasswordVerificationResult.Success;
        }
    }
}