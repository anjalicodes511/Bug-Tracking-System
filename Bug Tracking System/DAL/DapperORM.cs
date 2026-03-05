using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Bug_Tracking_System.DAL
{
    public class DapperORM
    {
        private readonly static string _connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        public static void ExecuteWithoutReturn(string ProcedureName, DynamicParameters param = null)
        {
            using(SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                con.Execute(ProcedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public static void ExecuteWithoutReturn(string ProcedureName, Object obj)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                con.Execute(ProcedureName, obj, commandType: CommandType.StoredProcedure);
            }
        }

        public static T ReturnSingle<T>(string ProcedureName, DynamicParameters param = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var result = con.QueryFirstOrDefault<T>(ProcedureName, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public static IEnumerable<T> ReturnList<T>(string ProcedureName, DynamicParameters param = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var result = con.Query<T>(ProcedureName, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}