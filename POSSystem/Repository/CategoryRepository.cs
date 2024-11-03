using Npgsql;
using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSSystem.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NpgsqlConnection _connection;

        public CategoryRepository(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var categories = new List<Category>();
            string query = "SELECT * FROM Category";
            await _connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, _connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    categories.Add(new Category
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }

            await _connection.CloseAsync();
            return categories;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var categories = await GetAllCategories();

            return categories.FirstOrDefault(c => c.Id == id);
        }

        public async Task AddCategory(Category category)
        {
            string query = "INSERT INTO Category (Name) VALUES (@Name)";
            await _connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("Name", category.Name);
                await cmd.ExecuteNonQueryAsync();
            }

            await _connection.CloseAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            string query = "UPDATE Category SET Name = @Name WHERE Id = @Id";
            await _connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("Id", category.Id);
                cmd.Parameters.AddWithValue("Name", category.Name);
                await cmd.ExecuteNonQueryAsync();
            }

            await _connection.CloseAsync();
        }

        public async Task DeleteCategory(int id)
        {
            string query = "DELETE FROM Category WHERE Id = @Id";
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
