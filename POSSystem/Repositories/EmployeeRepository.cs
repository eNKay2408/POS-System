﻿using Npgsql;
using System.Threading.Tasks;
using POSSystem.Models;
using POSSystem.Repositories;

namespace POSSystem.Repository
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public async Task SaveEmployee(Employee employee)
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

            await Connection.CloseAsync();
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
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

            await Connection.CloseAsync();

            return employee;
        }

        public async Task SaveEmployeeFromGoogle(Employee employee)
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

                await Connection.CloseAsync();
            }
        }
    }
}
