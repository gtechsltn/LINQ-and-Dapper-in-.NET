using Dapper;
using DapperLinQWebApi.Data;
using DapperLinQWebApi.Helpers;
using DapperLinQWebApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.NetworkInformation;

namespace DapperLinQWebApi.Services
{
    public class UserRepository
    {
        private readonly DapperContext _connection;

        public UserRepository(IConfiguration configuration, DapperContext connection)
        {
            _connection= connection;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = "SELECT * FROM Users WHERE IsActive = 1";
                return await db.QueryAsync<User>(sql);
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = "SELECT * FROM Users WHERE Id = @Id";
                return await db.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
            }
        }

        public async Task AddUserAsync(User user)
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = "INSERT INTO Users (Name, Email, IsActive) VALUES (@Name, @Email, @IsActive)";
                await db.ExecuteAsync(sql, user);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = "UPDATE Users SET Name = @Name, Email = @Email, IsActive = @IsActive WHERE Id = @Id";
                await db.ExecuteAsync(sql, user);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = "DELETE FROM Users WHERE Id = @Id";
                await db.ExecuteAsync(sql, new { Id = id });
            }
        }

        //--------------other APIS

        //Add a new method to filter users:
        public async Task<IEnumerable<User>> GetUsersByNameAsync(string name)
        {
            var users = new List<User>();

            using (var db = _connection.CreateDbConnection())
            {
                var sql = "SELECT * FROM Users";
                var result = await db.QueryAsync<User>(sql);
                users = result.ToList();
            }            
            
            return users.Where(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        //Modify the UserRepository for Pagination
        public async Task<IEnumerable<User>> GetUsersPaginatedAsync(int page, int pageSize)
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = "SELECT * FROM Users ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                var users = await db.QueryAsync<User>(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize });
                return users;
            }
        }

        ///LINQ for Aggregations
        public async Task<int> CountActiveUsersAsync()
        {
            var sql = "SELECT COUNT(*) FROM Users WHERE IsActive = 1";
            using (var db = _connection.CreateDbConnection())
            {
                return await db.ExecuteScalarAsync<int>(sql);
            }
        }

        ///Using LINQ with Anonymous Types
        public async Task<IEnumerable<dynamic>> GetUserSummariesAsync()
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = "SELECT Id, Name FROM Users WHERE IsActive = 1";
                return await db.QueryAsync<dynamic>(sql);
            }
        }

        //LINQ for Grouping Data
        public async Task<IEnumerable<dynamic>> GetUserOrderCountsAsync()
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = "SELECT UserId, COUNT(*) AS OrderCount FROM Orders GROUP BY UserId";
                return await db.QueryAsync<dynamic>(sql);
            }
        }

        //Using LINQ to Sort Data
        public async Task<IEnumerable<User>> GetAllUsersSortedAsync(string sortField = "Name", bool ascending = true)
        {
            using (var db = _connection.CreateDbConnection())
            {
                var sql = $"SELECT * FROM Users ORDER BY {sortField} {(ascending ? "ASC" : "DESC")}";
                return await db.QueryAsync<User>(sql);
            }
        }

        //Using LINQ with Extension Methods
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            var users = await GetAllUsersAsync();
            return users.WhereIsActive();
        }



    }
}
