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
    public class EmployeeRepositoryIntegrationTests
    {
        private static readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
        private NpgsqlConnection _connection;
        private IEmployeeRepository _repository;

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
        public async Task TestInitialize()
        {
            var connectionString = _postgreSqlContainer.GetConnectionString();
            _repository = new EmployeeRepository(connectionString);

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var setupQuery = @"
                    CREATE TABLE Employee (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR(255) NOT NULL,
                        Email VARCHAR(255) NOT NULL,
                        Password VARCHAR(255)
                    );

                    INSERT INTO Employee (Name, Email, Password) VALUES 
                        ('Admin', 'admin@enkay.live', '$2a$10$P13f37S6P4kN3JaY8i/a/O700M4NCqRv4LtOiLZ3IIozaj3ozqFnG');
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

                var dropTableQuery = "DROP TABLE Employee";

                using (var cmd = new NpgsqlCommand(dropTableQuery, connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        [TestMethod()]
        public async Task SaveEmployee_ShouldSaveEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                Name = "Employee",
                Email = "employee@enkay.live",
                Password = "Password123"
            };

            // Act
            await _repository.SaveEmployee(employee);

            // Assert
            var savedEmployee = await _repository.GetEmployeeByEmail(employee.Email);
            Assert.IsNotNull(savedEmployee);
            Assert.AreEqual(employee.Name, savedEmployee.Name);
            Assert.AreEqual(employee.Email, savedEmployee.Email);
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(employee.Password, savedEmployee.Password));
        }

        [TestMethod()]
        public async Task SaveEmployee_WhenEmployeeExists_ShouldUpdateEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                Name = "New Admin",
                Email = "admin@enkay.live",
                Password = "12345678Aa"
            };

            // Act & Assert
            // check message
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _repository.SaveEmployee(employee));
            Assert.AreEqual("Email already exists", ex.Message);
        }

        [TestMethod()]
        public async Task GetEmployeeByEmail_ShouldReturnEmployee()
        {
            // Act
            var employee = await _repository.GetEmployeeByEmail("admin@enkay.live");

            // Assert
            Assert.IsNotNull(employee);
            Assert.AreEqual("Admin", employee.Name);
            Assert.AreEqual("admin@enkay.live", employee.Email);
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("12345678Aa", employee.Password));
        }

        [TestMethod()]
        public async Task GetEmployeeByEmail_WhenEmployeeDoesNotExist_ShouldReturnNull()
        {
            // Act
            var employee = await _repository.GetEmployeeByEmail("admin123@enkay.live");

            // Assert
            Assert.IsNull(employee);
        }

        [TestMethod()]
        public async Task SaveEmployeeFromGoogle_ShouldSaveEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                Name = "Employee",
                Email = "employee@enkay.live",
            };

            // Act
            await _repository.SaveEmployeeFromGoogle(employee);

            // Assert
            var savedEmployee = await _repository.GetEmployeeByEmail(employee.Email);
            Assert.IsNotNull(savedEmployee);
            Assert.AreEqual(employee.Name, savedEmployee.Name);
            Assert.AreEqual(employee.Email, savedEmployee.Email);
            Assert.IsNull(savedEmployee.Password);
        }

        [TestMethod()]
        public async Task SaveEmployeeFromGoogle_WhenEmployeeExists_ShouldUpdateEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                Name = "New Admin",
                Email = "admin@enkay.live"
            };

            // Act
            await _repository.SaveEmployeeFromGoogle(employee);

            // Assert
            var savedEmployee = await _repository.GetEmployeeByEmail(employee.Email);
            Assert.IsNotNull(savedEmployee);
            Assert.AreEqual(employee.Name, savedEmployee.Name);
            Assert.AreEqual(employee.Email, savedEmployee.Email);
        }
    }
}
