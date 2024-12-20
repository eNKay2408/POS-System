using Npgsql;
using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSSystem.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly NpgsqlConnection _connection;

        public ProductRepository()
        {
            _connection = new NpgsqlConnection(ConnectionString);
        }

        // Constructor for integration testing
        public ProductRepository(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = new List<Product>();
            string query = "SELECT * FROM Product ORDER BY Id";
            await _connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, _connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Price = reader.GetDecimal(2),
                        Stock = reader.GetInt32(3),
                        CategoryId = reader.GetInt32(4),
                        BrandId = reader.GetInt32(5)
                    });
                }
            }

            await _connection.CloseAsync();
            return products;
        }

        public async Task<Product> GetProductById(int id)
        {
            var products = await GetAllProducts();

            return products.FirstOrDefault(p => p.Id == id);
        }

        public async Task AddProduct(Product product)
        {
            var products = await GetAllProducts();
            if (products.Any(p => p.Name == product.Name))
            {
                throw new Exception("Product name already exists");
            }

            string query = "INSERT INTO Product (Name, Price, Stock, CategoryId, BrandId) VALUES (@Name, @Price, @Stock, @CategoryId, @BrandId)";
            await _connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("Name", product.Name);
                cmd.Parameters.AddWithValue("Price", product.Price);
                cmd.Parameters.AddWithValue("Stock", NpgsqlTypes.NpgsqlDbType.Integer, product.Stock);
                cmd.Parameters.AddWithValue("CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("BrandId", product.BrandId);
                await cmd.ExecuteNonQueryAsync();
            }

            await _connection.CloseAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            var products = await GetAllProducts();
            if (products.Any(p => p.Id != product.Id && p.Name == product.Name))
            {
                throw new Exception("Product name already exists");
            }

            string query = "UPDATE Product SET Name = @Name, Price = @Price, Stock = @Stock, CategoryId = @CategoryId, BrandId = @BrandId WHERE Id = @Id";
            await _connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("Id", product.Id);
                cmd.Parameters.AddWithValue("Name", product.Name);
                cmd.Parameters.AddWithValue("Price", product.Price);
                cmd.Parameters.AddWithValue("Stock", product.Stock);
                cmd.Parameters.AddWithValue("CategoryId", product.CategoryId);
                cmd.Parameters.AddWithValue("BrandId", product.BrandId);
                await cmd.ExecuteNonQueryAsync();
            }

            await _connection.CloseAsync();
        }

        public async Task DeleteProduct(int id)
        {
            string query = "DELETE FROM Product WHERE Id = @Id";
            await _connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            await _connection.CloseAsync();
        }
    }
}
