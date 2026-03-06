using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracking_System.Services
{
    public class OtpService
    {
        public string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000,999999).ToString();
        }
    }
}