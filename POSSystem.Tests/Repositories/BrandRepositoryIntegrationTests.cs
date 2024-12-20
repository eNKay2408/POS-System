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
    public class BrandRepositoryIntegrationTests
    {
        private static readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
        private NpgsqlConnection _connection;
        private IBrandRepository _repository;

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
        public async Task Setup()
        {
            var connectionString = _postgreSqlContainer.GetConnectionString();
            _repository = new BrandRepository(connectionString);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var setupQuery = @"
                    CREATE TABLE Brand (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL
                    );
                    INSERT INTO Brand (Name) VALUES ('Brand A'), ('Brand B');
                ";

                using (var cmd = new NpgsqlCommand(setupQuery, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        [TestCleanup()]
        public async Task Cleanup()
        {
            var connectionString = _postgreSqlContainer.GetConnectionString();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var dropTableQuery = "DROP TABLE Brand;";

                using (var cmd = new NpgsqlCommand(dropTableQuery, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        [TestMethod()]
        public async Task GetAllBrands_ShouldReturnAllBrands()
        {
            // Act
            var brands = await _repository.GetAllBrands();

            // Assert
            Assert.AreEqual(2, brands.Count);
            Assert.AreEqual("Brand A", brands[0].Name);
        }

        [TestMethod()]
        public async Task GetBrandById_ShouldReturnBrand()
        {
            // Act
            var brand = await _repository.GetBrandById(1);

            // Assert
            Assert.AreEqual("Brand A", brand.Name);
        }

        [TestMethod()]
        public async Task AddBrand_ShouldAddBrand()
        {
            // Arrange
            var brand = new Brand
            {
                Name = "Brand C"
            };

            // Act
            await _repository.AddBrand(brand);

            // Assert
            var brands = await _repository.GetAllBrands();
            Assert.AreEqual(3, brands.Count);
            Assert.AreEqual("Brand C", brands[2].Name);
        }

        [TestMethod()]
        public async Task AddBrand_WhenBrandExists_ShouldThrowException()
        {
            // Arrange
            var brand = new Brand
            {
                Name = "Brand A"
            };

            // Act and Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(async () => await _repository.AddBrand(brand));
            Assert.AreEqual("Brand already exists", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateBrand_ShouldUpdateBrand()
        {
            // Arrange
            var brand = new Brand
            {
                Id = 1,
                Name = "Brand X"
            };

            // Act
            await _repository.UpdateBrand(brand);

            // Assert
            var updatedBrand = await _repository.GetBrandById(1);
            Assert.AreEqual("Brand X", updatedBrand.Name);
        }

        [TestMethod()]
        public async Task DeleteBrand_ShouldDeleteBrand()
        {
            // Act
            await _repository.DeleteBrand(2);

            // Assert
            var brands = await _repository.GetAllBrands();
            Assert.AreEqual(1, brands.Count);
            Assert.AreEqual("Brand A", brands[0].Name);
        }
    }
}
