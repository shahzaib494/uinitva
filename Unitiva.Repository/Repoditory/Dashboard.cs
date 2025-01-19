using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Unitiva.Domain.User;
using Unitiva.Service.Contracts.Dashborad;

namespace Unitiva.Repository.Repoditory
{
    public class Dashboard : IDashborad
    {
        private readonly string _connectionString;

        public Dashboard(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<dynamic> GetDashboard()
        {
            var query = @"
                         SELECT Status, COUNT(*) AS Count
                         FROM [DiscountForm]
                         GROUP BY Status
 
                         UNION ALL
 
                         SELECT 'TOTAL' AS Status, COUNT(*) AS Count
                         FROM [DiscountForm]"; 

            using var connection = CreateConnection();
            var res = await connection.QueryAsync<dynamic>(query);
            return res;
        }

        public async Task<dynamic> GetFacultyCounts()
        {
            var query = @"                       
                          SELECT 
                            Faculty,
                            COUNT(*) AS TotalCount,
                            SUM(CASE WHEN STATUS = 'PENDING' THEN 1 ELSE 0 END) AS PendingCount,
                            SUM(CASE WHEN STATUS = 'APPROVED' THEN 1 ELSE 0 END) AS ApprovedCount,
                            SUM(CASE WHEN STATUS = 'REJECTED' THEN 1 ELSE 0 END) AS RejectedCount
                        FROM [UNITIVA].[dbo].[DiscountForm]
                        GROUP BY Faculty;";
                        
            using var connection = CreateConnection();
            var res = await connection.QueryAsync<dynamic>(query);
            return res;
        }

        public async Task<dynamic> GetFormByStatus(string status)
        {
            var query = $" SELECT * FROM [UNITIVA].[dbo].[DiscountForm] where status = '{status}' or '{status}' = 'ALL';";

            using var connection = CreateConnection();
            var res = await connection.QueryAsync<dynamic>(query);
            return res;
        }
    }
}
