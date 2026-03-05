using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracking_System.Models.Entities
{
    public class User
    {
        public int Id {  get; set; }
        public string Name {  get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsEmailVerified {  get; set; }
        public string EmailVerificationToken {  get; set; }
        public DateTime EmailTokenExpiry { get; set; } 
        public string ResetPasswordToken {  get; set; }
        public DateTime ResetPasswordExpiry { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}