using Bug_Tracking_System.DAL;
using Bug_Tracking_System.Models.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracking_System.Repositories
{
    public class OtpRepository
    {
        public void Create(EmailOtp emailotp)
        {
            if(emailotp != null)
            {
                throw new ArgumentNullException("EmailOtp");
            }
            DynamicParameters dp = new DynamicParameters(emailotp);
            DapperORM.ExecuteWithoutReturn("CreateEmailOtp",dp);
        }
    }
}