using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using POSSystem.Models;
using System;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using POSSystem.Repositories;

namespace POSSystem.Tests.Repositories
{
    [TestClass()]
    public class ProductRepositoryIntegrationTests
    {
        private static readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
        private IProductRepository _repository;

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
            _repository = new ProductRepository(connectionString);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var setupQuery = @"
                    CREATE TABLE Category (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL
                    );

                    CREATE TABLE Brand (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL
                    );

                    CREATE TABLE Product (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL,
                        Price DECIMAL(10, 2),
                        Stock INT,
                        CategoryId INT NOT NULL,
                        BrandId INT NOT NULL,
                        FOREIGN KEY (CategoryId) REFERENCES Category(Id),
                        FOREIGN KEY (BrandId) REFERENCES Brand(Id)
                    );

                    INSERT INTO Category (Name) VALUES ('Category A'), ('Category B');

                    INSERT INTO Brand (Name) VALUES ('Brand A'), ('Brand B');

                    INSERT INTO Product (Name, Price, Stock, CategoryId, BrandId) VALUES ('Product A', 10.00, 100, 1, 1), ('Product B', 20.00, 200, 2, 2);
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

                var cleanupQuery = @"
                    DROP TABLE IF EXISTS Product;
                    DROP TABLE IF EXISTS Brand;
                    DROP TABLE IF EXISTS Category;
                ";

                using (var cmd = new NpgsqlCommand(cleanupQuery, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        [TestMethod()]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {
            // Act
            var products = await _repository.GetAllProducts();

            // Assert
            Assert.AreEqual(2, products.Count);
            Assert.AreEqual("Product A", products[0].Name);
            Assert.AreEqual(10.00m, products[0].Price);
            Assert.AreEqual(100, products[0].Stock);
            Assert.AreEqual(1, products[0].CategoryId);
            Assert.AreEqual(1, products[0].BrandId);

            Assert.AreEqual("Product B", products[1].Name);
        }

        [TestMethod()]
        public async Task GetProductById_ShouldReturnProduct()
        {
            // Act
            var product = await _repository.GetProductById(2);

            // Assert
            Assert.AreEqual("Product B", product.Name);
            Assert.AreEqual(20.00m, product.Price);
            Assert.AreEqual(200, product.Stock);
            Assert.AreEqual(2, product.CategoryId);
            Assert.AreEqual(2, product.BrandId);
        }

        [TestMethod()]
        public async Task AddProduct_ShouldAddProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product C",
                Price = 30.00m,
                Stock = 300,
                CategoryId = 1,
                BrandId = 2
            };

            // Act
            await _repository.AddProduct(product);

            // Assert
            var products = await _repository.GetAllProducts();
            Assert.AreEqual(3, products.Count);
            Assert.AreEqual("Product C", products[2].Name);
            Assert.AreEqual(30.00m, products[2].Price);
            Assert.AreEqual(300, products[2].Stock);
            Assert.AreEqual(1, products[2].CategoryId);
            Assert.AreEqual(2, products[2].BrandId);
        }

        [TestMethod()]
        public async Task AddProduct_WhenProductNameExists_ShouldThrowException()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product A",
                Price = 30.00m,
                Stock = 300,
                CategoryId = 1,
                BrandId = 2
            };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _repository.AddProduct(product));
            Assert.AreEqual("Product name already exists", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateProduct_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 2,
                Name = "Product C",
                Price = 30.00m,
                Stock = 300,
                CategoryId = 1,
                BrandId = 2
            };

            // Act
            await _repository.UpdateProduct(product);

            // Assert
            var products = await _repository.GetAllProducts();
            Assert.AreEqual(2, products.Count);
            Assert.AreEqual("Product C", products[1].Name);
            Assert.AreEqual(30.00m, products[1].Price);
            Assert.AreEqual(300, products[1].Stock);
            Assert.AreEqual(1, products[1].CategoryId);
            Assert.AreEqual(2, products[1].BrandId);
        }

        [TestMethod()]
        public async Task UpdateProduct_WhenProductNameExists_ShouldThrowException()
        {
            // Arrange
            var product = new Product
            {
                Id = 2,
                Name = "Product A",
                Price = 30.00m,
                Stock = 300,
                CategoryId = 1,
                BrandId = 2
            };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _repository.UpdateProduct(product));
            Assert.AreEqual("Product name already exists", ex.Message);
        }

        [TestMethod()]
        public async Task DeleteProduct_ShouldDeleteProduct()
        {
            // Act
            await _repository.DeleteProduct(2);

            // Assert
            var products = await _repository.GetAllProducts();
            Assert.AreEqual(1, products.Count);
            Assert.AreEqual("Product A", products[0].Name);
        }
    }
}
