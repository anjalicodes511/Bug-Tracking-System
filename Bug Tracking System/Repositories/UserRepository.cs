using Bug_Tracking_System.DAL;
using Bug_Tracking_System.Models.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

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
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Name", user.Name);
            dp.Add("@Email", user.Email);
            dp.Add("@PasswordHash", user.PasswordHash);
            dp.Add("@IsEmailVerified", user.IsEmailVerified);
            DapperORM.ExecuteWithoutReturn("CreateUser", dp);
        }
        public User GetByEmail(string email)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Email", email);

            var user = DapperORM.ReturnSingle<User>("GetUserByEmail", dp);
            return user;
        }

        public User GetById(int UserId)
        {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Id", UserId);

            var user = DapperORM.ReturnSingle<User>("GetUserById", dp);
            return user;
        }

        public void Update(User record)
        {
            DynamicParameters dp = new DynamicParameters(record);

            DapperORM.ExecuteWithoutReturn("UpdateUser", dp);
        }
    }
}