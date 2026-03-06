using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Bug_Tracking_System.Models.VM;
using Bug_Tracking_System.Repositories;
using Bug_Tracking_System.Services;
using Microsoft.AspNet.Identity;

namespace Bug_Tracking_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController()
        {
            var repo = new UserRepository();
            var otpRepo = new OtpRepository();
            var passwordService = new PasswordService();
            var emailService = new EmailService();
            var otpService = new OtpService();
            _authService = new AuthService(repo,emailService, passwordService, otpService,otpRepo);
        }
        // GET: Account

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Signup(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new {success = false,message = "Enter Valid Details"});
            }
            var user = _authService.Register(model);
            Session["PendingUserId"] = user.Id;
            return Json(new { success = true,message = "OTP sent successfully!!" });
        }

        public ActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public JsonResult VerifyOtp(string otp)
        {
            int userId = (int)Session["PendingUserId"];

            bool isValid = _authService.VerifyOtp(userId, otp);

            if (!isValid)
            {
                return Json(new { success = false, message = "Invalid OTP" });
            }

            Session.Remove("PendingUserId");

            return Json(new { success = true, message = "Account verified successfully" });
        }
    }
}