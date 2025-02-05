﻿using Npgsql;
using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSSystem.Repositories
{
    public class BrandRepository : BaseRepository, IBrandRepository
    {
        private readonly NpgsqlConnection _connection;

        public BrandRepository()
        {
            _connection = new NpgsqlConnection(ConnectionString);
        }

        // Constructor for integration testing
        public BrandRepository(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            var brands = new List<Brand>();
            string query = "SELECT * FROM Brand ORDER BY Id";
            await _connection.OpenAsync();

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        brands.Add(new Brand
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in GetAllBrands: {ex.Message}");
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }

            return brands;
        }

        public async Task<Brand> GetBrandById(int id)
        {
            var brands = await GetAllBrands();
            return brands.FirstOrDefault(m => m.Id == id);
        }

        public async Task AddBrand(Brand brand)
        {
            // check if brand already exists
            var brands = await GetAllBrands();
            if (brands.Any(m => m.Name == brand.Name))
            {
                throw new Exception("Brand already exists");
            }

            string query = "INSERT INTO Brand (Name) VALUES (@Name)";
            await _connection.OpenAsync();

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Name", brand.Name);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in AddBrand: {ex.Message}");
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task UpdateBrand(Brand brand)
        {
            string query = "UPDATE Brand SET Name = @Name WHERE Id = @Id";
            await _connection.OpenAsync();

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Id", brand.Id);
                    cmd.Parameters.AddWithValue("Name", brand.Name);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in UpdateBrand: {ex.Message}");
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task DeleteBrand(int id)
        {
            string query = "DELETE FROM Brand WHERE Id = @Id";
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
                // Log the exception
                Console.WriteLine($"Error in DeleteBrand: {ex.Message}");
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> HasReferencingProducts(int brandId)
        {
            string query = "SELECT COUNT(*) FROM Product WHERE BrandId = @BrandId";
            await _connection.OpenAsync();

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("BrandId", brandId);
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
