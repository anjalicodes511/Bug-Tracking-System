using Bug_Tracking_System.DAL;
using Bug_Tracking_System.Models.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracking_System.Repositories
{
    public class UserRepository
    {
        public void Create(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException("user"); 
            }
            DynamicParameters dp = new DynamicParameters(user);
            DapperORM.ExecuteWithoutReturn("CreateUser", dp);
        }
        public User GetByEmail(string email)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Email", email);

            var user = DapperORM.ReturnSingle<User>("GetUserByEmail", dp);
            return user;
        }
    }
}