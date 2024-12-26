using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using POSSystem.Models;
using POSSystem.Repositories;
using System;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace POSSystem.Tests.Repositories
{
    [TestClass()]
    public class CategoryRepositoryIntegrationTests
    {
        private static readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
        private ICategoryRepository _repository;

        [ClassInitialize()]
        public static async Task ClassSetup(TestContext context)
        {
            await _postgreSqlContainer.StartAsync();
        }

        [ClassCleanup()]
        public static async Task ClassCleanup()
        {
            await _postgreSqlContainer.DisposeAsync();
        }

        [TestInitialize()]
        public async Task TestSetup()
        {
            var connectionString = _postgreSqlContainer.GetConnectionString();
            _repository = new CategoryRepository(connectionString);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var setupQuery = @"
                    CREATE TABLE Category (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL
                    );
                    INSERT INTO Category (Name) VALUES ('Category A'), ('Category B');
                ";

                using (var cmd = new NpgsqlCommand(setupQuery, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        [TestCleanup()]
        public async Task TestCleanup()
        {
            var connectionString = _postgreSqlContainer.GetConnectionString();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var cleanupQuery = "DROP TABLE IF EXISTS Category;";
                using (var cmd = new NpgsqlCommand(cleanupQuery, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        [TestMethod()]
        public async Task GetAllCategories_ShouldReturnAllCategories()
        {
            // Act
            var categories = await _repository.GetAllCategories();

            // Assert
            Assert.AreEqual(2, categories.Count);
            Assert.AreEqual("Category A", categories[0].Name);
        }

        [TestMethod()]
        public async Task GetCategoryById_ShouldReturnCategory()
        {
            // Act
            var category = await _repository.GetCategoryById(2);

            // Assert
            Assert.AreEqual("Category B", category.Name);
        }

        [TestMethod()]
        public async Task AddCategory_ShouldAddCategory()
        {
            // Arrange
            var category = new Category { Name = "Category C" };

            // Act
            await _repository.AddCategory(category);

            // Assert
            var categories = await _repository.GetAllCategories();
            Assert.AreEqual(3, categories.Count);
            Assert.AreEqual("Category C", categories[2].Name);
        }

        [TestMethod()]
        public async Task AddCategory_WhenCategoryExists_ShouldThrowException()
        {
            // Arrange
            var category = new Category { Name = "Category A" };

            // Act and Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _repository.AddCategory(category));
            Assert.AreEqual("Category already exists", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateCategory_ShouldUpdateCategory()
        {
            // Arrange
            var category = new Category
            {
                Id = 1,
                Name = "Category X"
            };

            // Act
            await _repository.UpdateCategory(category);

            // Assert
            var updatedCategory = await _repository.GetCategoryById(1);
            Assert.AreEqual("Category X", updatedCategory.Name);
        }

        [TestMethod()]
        public async Task DeleteCategory_ShouldDeleteCategory()
        {
            // Act
            await _repository.DeleteCategory(2);

            // Assert
            var categories = await _repository.GetAllCategories();
            Assert.AreEqual(1, categories.Count);
            Assert.AreEqual("Category A", categories[0].Name);
        }
    }
}