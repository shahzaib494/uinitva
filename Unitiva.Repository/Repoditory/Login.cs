
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Unitiva.Domain.User;
using Unitiva.Service.Contracts.ILogin;

namespace Unitiva.Repository.Repoditory
{
    public class Login : ILogin
    {
        private readonly string _connectionString;

        public Login(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);


        public async Task<string> login(string username, string password)
        {
            string query = $"SELECT * FROM [UNITIVA].[dbo].[USER] where [USER_NAME] = '{username}' AND [PASSWORD] = '{password}'";
            using var connection = CreateConnection();
                var user  =await connection.QueryAsync<User>(query);
            if(user.Count() > 0) {
                string userName = user?.FirstOrDefault().FIRST_NAME + " " + user?.FirstOrDefault().LAST_NAME;
                return userName;
            }
            return null;
        }

        public async Task<bool> register(RegisterUserDto request)
        {
            string query = $@"
              INSERT INTO [UNITIVA].[dbo].[USER] 
                  ([USER_NAME], [PASSWORD], [EMAIL], [FIRST_NAME], [LAST_NAME], [CREATED_BY], [CREATED_ON], [MODIFIED_BY], [MODIFIED_ON], [ROLE])
              VALUES
                  ('{request.UserName}', '{request.Password}', '{request.Email}', '{request.FirstName}', '{request.LastName}', 'Admin', GETDATE(), NULL, GETDATE(), 'User')";

            using var connection = CreateConnection();
            await connection.QueryAsync<User>(query);
            return true;
        }
    }
}
