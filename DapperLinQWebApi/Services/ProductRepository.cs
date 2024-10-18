using Dapper;
using DapperLinQWebApi.Data;
using DapperLinQWebApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLinQWebApi.Services
{
    public class ProductRepository
    {
        private readonly DapperContext _connection;

        public ProductRepository(DapperContext connectionq)
        {
            _connection = connectionq;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            using var dbConnection = _connection.CreateDbConnection();

            return dbConnection.Query<Product>("SELECT * FROM Products");
        }

        public Product GetProductById(int id)
        {
            using var dbConnection = _connection.CreateDbConnection();

            return dbConnection.QueryFirstOrDefault<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
        }

        public void AddProduct(Product product)
        {
            using (var dbConnection = _connection.CreateDbConnection())
            {

                var sqlQuery = "INSERT INTO Products (Name, Price) VALUES(@Name, @Price)";
                dbConnection.Execute(sqlQuery, product);
            }

        }
    }
}
