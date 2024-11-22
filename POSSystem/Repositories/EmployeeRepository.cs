using Npgsql;
using System.Threading.Tasks;
using POSSystem.Models;
using POSSystem.Repositories;
using System.Collections.Generic;
using System;

namespace POSSystem.Repository
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public async Task SaveEmployee(Employee employee)
        {
            try
            {
                employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);

                await Connection.OpenAsync();
                string query = "INSERT INTO Employee (Name, Email, Password) VALUES (@Name, @Email, @Password)";

                using (var cmd = new NpgsqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("Name", employee.Name);
                    cmd.Parameters.AddWithValue("Email", employee.Email);
                    cmd.Parameters.AddWithValue("Password", employee.Password);
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
                await Connection.CloseAsync();
            }
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            try
            {
                await Connection.OpenAsync();
                string query = "SELECT * FROM Employee WHERE Email = @Email";
                Employee employee = null;

                using (var cmd = new NpgsqlCommand(query, Connection))
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
                await Connection.CloseAsync();
            }
        }

        public async Task SaveEmployeeFromGoogle(Employee employee)
        {
            try
            {
                var existingEmployee = await GetEmployeeByEmail(employee.Email);

                if (existingEmployee == null)
                {
                    await Connection.OpenAsync();

                    string query = "INSERT INTO Employee (Name, Email) VALUES (@Name, @Email)";

                    using (var cmd = new NpgsqlCommand(query, Connection))
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
                await Connection.CloseAsync();
            }
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = new List<Employee>();
                string query = "SELECT * FROM employee";
                await Connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, Connection))
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
                await Connection.CloseAsync();
            }
        }

        public async Task UpdateEmployee(Employee employee)
        {
            try
            {
                await Connection.OpenAsync();
                string query = "UPDATE Employee SET Name = @Name, Email = @Email WHERE Id = @Id";

                using (var cmd = new NpgsqlCommand(query, Connection))
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
                await Connection.CloseAsync();
            }
        }


        public async Task DeleteEmployee(int id)
        {
            try
            {
                await Connection.OpenAsync();
                string query = "DELETE FROM employee WHERE Id = @Id";

                using (var cmd = new NpgsqlCommand(query, Connection))
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
                await Connection.CloseAsync();
            }
        }
    }
}
