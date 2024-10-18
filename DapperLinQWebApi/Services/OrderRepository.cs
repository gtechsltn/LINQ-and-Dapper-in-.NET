using Dapper;
using DapperLinQWebApi.Data;
using DapperLinQWebApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLinQWebApi.Services
{
    public class OrderRepository
    {
        private readonly DapperContext _connection;

        public OrderRepository(DapperContext connectionq)
        {
            _connection = connectionq;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = "SELECT * FROM Orders WHERE UserId = @UserId";
                return await db.QueryAsync<Order>(sql, new { UserId = userId });
            }
        }
    }
}
