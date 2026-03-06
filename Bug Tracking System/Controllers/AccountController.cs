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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM user)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(null, "Enter Valid Details");
                return View();
            }
            return RedirectToAction("Dashboard", "Home");
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Signup(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(null, "Enter Valid Details");
                return Json(new {success = false,message = "Something Went Wrong"});
            }
            _authService.Register(model);
            return Json(new { success = true });
        }
    }
}