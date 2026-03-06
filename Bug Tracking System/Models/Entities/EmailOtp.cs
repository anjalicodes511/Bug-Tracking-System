using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracking_System.Models.Entities
{
    public class EmailOtp
    {
        public int Id { get; set; }

        public int UserId {  get; set; }

        public string OtpCode {  get; set; }

        public int AttemptCount {  get; set; }

        public bool IsUsed {  get; set; }

        public DateTime ExpiryTime {  get; set; }

        public DateTime CreatedAt { get; set; }

    }
}