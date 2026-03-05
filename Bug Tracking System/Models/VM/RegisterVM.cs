using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracking_System.Models.VM
{
    public class RegisterVM
    {
        public string Name {  get; set; }
        public string Email {  get; set; }
        public string Password { get; set; }
    }
}