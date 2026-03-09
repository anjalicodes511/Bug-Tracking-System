using Bug_Tracking_System.DAL;
using Bug_Tracking_System.Models.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace Bug_Tracking_System.Repositories
{
    public class OtpRepository
    {
        public void Create(EmailOtp emailotp)
        {
            if(emailotp == null)
            {
                throw new ArgumentNullException("EmailOtp");
            }
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", emailotp.UserId);
            dp.Add("@OtpCode", emailotp.OtpCode);
            dp.Add("@ExpiryTime", emailotp.ExpiryTime);
            DapperORM.ExecuteWithoutReturn("CreateEmailOtp",dp);
        }

        public EmailOtp GetLatestOtp(int UserId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", UserId);

            return DapperORM.ReturnSingle<EmailOtp>("GetLatestOtp", dp);
        }

        public void Update(EmailOtp record)
        {
            DynamicParameters dp = new DynamicParameters(record);
            
            DapperORM.ExecuteWithoutReturn("UpdateEmailOtp", dp);
        }
    }
}