using Npgsql;
using System.Threading.Tasks;
using POSSystem.Models;
using POSSystem.Repositories;
using System;

namespace POSSystem.Repository
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        private readonly NpgsqlConnection _connection;

        public EmployeeRepository()
        {
            _connection = new NpgsqlConnection(ConnectionString);
        }

        // Constructor for integration testing
        public EmployeeRepository(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        public async Task SaveEmployee(Employee employee)
        {
            var existingEmployee = await GetEmployeeByEmail(employee.Email);
            if (existingEmployee != null)
            {
                throw new Exception("Email already exists");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password);

            await _connection.OpenAsync();
            string query = "INSERT INTO Employee (Name, Email, Password) VALUES (@Name, @Email, @Password)";

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("Name", employee.Name);
                cmd.Parameters.AddWithValue("Email", employee.Email);
                cmd.Parameters.AddWithValue("Password", hashedPassword);
                await cmd.ExecuteNonQueryAsync();
            }

            await _connection.CloseAsync();
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            await _connection.OpenAsync();
            string query = "SELECT * FROM Employee WHERE Email = @Email";
            Employee employee = null;

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("Email", email);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.IsDBNull(3) ? null : reader.GetString(3)
                        };
                    }
                }
            }

            await _connection.CloseAsync();

            return employee;
        }

        public async Task SaveEmployeeFromGoogle(Employee employee)
        {
            var existingEmployee = await GetEmployeeByEmail(employee.Email);

            await _connection.OpenAsync();

            if (existingEmployee == null)
            {

                string query = "INSERT INTO Employee (Name, Email) VALUES (@Name, @Email)";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Name", employee.Name);
                    cmd.Parameters.AddWithValue("Email", employee.Email);
                    await cmd.ExecuteNonQueryAsync();
                }

            }
            else
            {
                string query = "UPDATE Employee SET Name = @Name WHERE Email = @Email";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Name", employee.Name);
                    cmd.Parameters.AddWithValue("Email", employee.Email);
                    await cmd.ExecuteNonQueryAsync();
                }

            }
            await _connection.CloseAsync();
        }
    }
}
