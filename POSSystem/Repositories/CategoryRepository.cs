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
        private readonly NpgsqlConnection _connection;

        public CategoryRepository()
        {
            _connection = new NpgsqlConnection(ConnectionString);
        }

        // Constructor for integration testing
        public CategoryRepository(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var categories = new List<Category>();
            string query = "SELECT * FROM Category ORDER BY Id";
            await _connection.OpenAsync();

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllCategories: {ex.Message}");
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }

            return categories;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var categories = await GetAllCategories();
            return categories.FirstOrDefault(c => c.Id == id);
        }

        public async Task AddCategory(Category category)
        {
            var categories = await GetAllCategories();
            if (categories.Any(c => c.Name == category.Name))
            {
                throw new Exception("Category already exists");
            }

            string query = "INSERT INTO Category (Name) VALUES (@Name)";
            await _connection.OpenAsync();

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Name", category.Name);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddCategory: {ex.Message}");
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task UpdateCategory(Category category)
        {
            string query = "UPDATE Category SET Name = @Name WHERE Id = @Id";
            await _connection.OpenAsync();

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Id", category.Id);
                    cmd.Parameters.AddWithValue("Name", category.Name);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateCategory: {ex.Message}");
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task DeleteCategory(int id)
        {
            string query = "DELETE FROM Category WHERE Id = @Id";
            await _connection.OpenAsync();

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteCategory: {ex.Message}");
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> HasReferencingProducts(int categoryId)
        {
            string query = "SELECT COUNT(*) FROM Product WHERE CategoryId = @CategoryId";
            await _connection.OpenAsync();

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("CategoryId", categoryId);
                    var count = (long)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HasReferencingProducts: {ex.Message}");
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
