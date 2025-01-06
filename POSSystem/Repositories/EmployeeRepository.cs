using Npgsql;
using System.Threading.Tasks;
using POSSystem.Models;
using POSSystem.Repositories;
using System.Collections.Generic;
using System;

namespace POSSystem.Repositories
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
            try
            {
                await _connection.OpenAsync();
                var existingEmployee = await GetEmployeeByEmail(employee.Email);

                if (existingEmployee != null)
                {
                    throw new Exception("Email already exists");
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password);

                string query = "INSERT INTO Employee (Name, Email, Password) VALUES (@Name, @Email, @Password)";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Name", employee.Name);
                    cmd.Parameters.AddWithValue("Email", employee.Email);
                    cmd.Parameters.AddWithValue("Password", hashedPassword);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            try
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

                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task SaveEmployeeFromGoogle(Employee employee)
        {
            try
            {
                await _connection.OpenAsync();
                var existingEmployee = await GetEmployeeByEmail(employee.Email);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                await _connection.OpenAsync();
                var employees = new List<Employee>();
                string query = "SELECT * FROM employee";

                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        //not reading password
                        employees.Add(new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2)
                        });
                    }
                }

                return employees;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task UpdateEmployee(Employee employee)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "UPDATE Employee SET Name = @Name, Email = @Email WHERE Id = @Id";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Name", employee.Name);
                    cmd.Parameters.AddWithValue("Email", employee.Email);
                    cmd.Parameters.AddWithValue("Id", employee.Id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task DeleteEmployee(int id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "DELETE FROM employee WHERE Id = @Id";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> HasReferencingInvoices(int employeeId)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Invoice WHERE EmployeeId = @EmployeeId";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("EmployeeId", employeeId);
                    var count = (long)await cmd.ExecuteScalarAsync();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}