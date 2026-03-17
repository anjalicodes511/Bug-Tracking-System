using Bug_Tracking_System.Models.VM;
using Bug_Tracking_System.Repositories;
using Bug_Tracking_System.Services;
using Microsoft.AspNet.Identity;
using Sprache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

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
            var user = _authService.Register(model);
            Session["PendingUserId"] = user.Id;
            Session["OtpPurpose"] = "Signup";
            //return Success("OTP sent successfully!!");
            return Json(new { success = true, message = "OTP Sent Successfully", redirectUrl = "/Account/VerifyOtp" });
        }

        public ActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public JsonResult VerifyOtp(string otp)
        {
            if (Session["PendingUserId"] == null || Session["OtpPurpose"] == null)
            {
                Response.StatusCode = 400;
                return Json(new { success = false, message = "Session expired. Please signup again." });
            }

            int userId = (int)Session["PendingUserId"];
            string purpose = Session["OtpPurpose"].ToString();

            Debug.WriteLine("OtpPurpose: " + purpose);
            if (purpose == "Signup")
            {
                _authService.VerifyOtp(userId, otp);
                Session.Remove("PendingUserId");
                Session.Remove("OtpPurpose");

                return Json(new { success = true, message = "Account verified", redirectUrl = "/Account/Login" });
            }
            else if (purpose == "ForgotPassword")
            {
                _authService.VerifyForgotPasswordOtp(userId, otp);
                Session.Remove("OtpPurpose");

                return Json(new { success = true, message = "OTP verified", redirectUrl = "/Account/ResetPassword" });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult ResendOtp()
        {
            if (Session["PendingUserId"] == null)
            {
                Response.StatusCode = 400;
                return Json(new { success = false, message = "Session expired. Signup again." });
            }
            int userId = (int)Session["PendingUserId"];
            _authService.ResendOtp(userId);
            return Json(new { success = true, message = "OTP resent successfully!", redirectUrl = "/Account/VerifyOtp" });
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginVM model)
        {
            _authService.Login(model);
            return Json(new { success = true, message = "Login Successful", redirectUrl = "/Account/Home" });
        }

        [Authorize]
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult ForgotPassword(string email)
        {
            var user = _authService.ForgotPassword(email);
            Session["PendingUserId"] = user.Id;
            Session["OtpPurpose"] = "ForgotPassword";
            return Json(new { success = true, message = "OTP sent to Email",redirectUrl = "/Account/VerifyOtp" });
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ResetPassword(string newPassword)
        {
            if (Session["PendingUserId"] == null)
            {
                Response.StatusCode = 400;
                return Json(new { success = false, message = "Session expired." });
            }

            int userId = (int)Session["PendingUserId"];

            _authService.ResetPassword(userId, newPassword);

            Session.Remove("PendingUserId");

            return Json(new
            {
                success = true,
                message = "Password reset successful!",
                redirectUrl = "/Account/Login"
            });
        }
        //public ActionResult VerifyForgotPasswordOtp()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult VerifyForgotPasswordOtp(string otp)
        //{
        //    if (Session["PendingUserId"] == null)
        //    {
        //        Response.StatusCode = 400;
        //        return Json(new { success = false, message = "Session expired. Please signup again." });
        //    }

        //    int userId = (int)Session["PendingUserId"];

        //    _authService.VerifyOtp(userId, otp);

        //    Session.Remove("PendingUserId");
        //    return Json(new { success = true, message = "Account verified successfully", redirectUrl = "/Account/Login" });
        //}

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}