using Npgsql;
using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSSystem.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public async Task<List<Category>> GetAllCategories()
        {
            var categories = new List<Category>();
            string query = "SELECT * FROM Category";
            await Connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, Connection))
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

            await Connection.CloseAsync();
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
            await Connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("Name", category.Name);
                await cmd.ExecuteNonQueryAsync();
            }

            await Connection.CloseAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            string query = "UPDATE Category SET Name = @Name WHERE Id = @Id";
            await Connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("Id", category.Id);
                cmd.Parameters.AddWithValue("Name", category.Name);
                await cmd.ExecuteNonQueryAsync();
            }

            await Connection.CloseAsync();
        }

        public async Task DeleteCategory(int id)
        {
            string query = "DELETE FROM Category WHERE Id = @Id";
            await Connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            await Connection.CloseAsync();
        }
    }
}
