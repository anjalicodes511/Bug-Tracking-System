using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracking_System.Services
{
    public class TokenService
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}