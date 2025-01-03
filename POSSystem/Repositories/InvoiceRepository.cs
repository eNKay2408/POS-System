using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POSSystem.Models;
using Stripe.Issuing;

namespace POSSystem.Repositories
{
    internal class InvoiceRepository : BaseRepository, IInvoiceRepository
    {
        private readonly NpgsqlConnection _connection;

        public InvoiceRepository()
        {
            _connection = new NpgsqlConnection(ConnectionString);
        }

        //public async Task<int> CreateInvoice(Invoice invoice)
        //{
        //    try
        //    {
        //        string query = "INSERT INTO Invoice (employeeid, timestamp, total, ispaid) VALUES (@employeeid, @timestamp, @total, @ispaid)";
        //        await _connection.OpenAsync();

        //        using (var cmd = new NpgsqlCommand(query, _connection))
        //        {
        //            cmd.Parameters.AddWithValue("employeeid", invoice.EmployeeId);
        //            cmd.Parameters.AddWithValue("timestamp", invoice.Timestamp);
        //            cmd.Parameters.AddWithValue("total", invoice.Total);
        //            cmd.Parameters.AddWithValue("ispaid", invoice.IsPaid);

        //            return (int)await cmd.ExecuteScalarAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //    finally
        //    {
        //        await _connection.CloseAsync();
        //    }
        //}

        

        public async Task<int> SaveInvoice(Invoice invoice)
        {
            try
            {
                string query = "INSERT INTO Invoice (employeeid, timestamp, total, ispaid, employeename) VALUES (@employeeid, @timestamp, @total, @ispaid, (SELECT name from employee where id = @employeeid)) RETURNING id";
                //string query = "UPDATE Invoice SET employeeid = @employeeid, timestamp = @timestamp, total = @total, employeename = (SELECT name from employee where id = @employeeid) WHERE id = @id";
                await _connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("employeeid", invoice.EmployeeId);
                    cmd.Parameters.AddWithValue("timestamp", invoice.Timestamp);
                    cmd.Parameters.AddWithValue("total", invoice.Total);
                    //cmd.Parameters.AddWithValue("id", invoice.Id);
                    cmd.Parameters.AddWithValue("ispaid", invoice.IsPaid);

                    return (int)await cmd.ExecuteScalarAsync();
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

        public async Task<List<Invoice>> GetAllInvoices()
        {
            try
            {
                var invoices = new List<Invoice>();
                string query = "SELECT i.*, e.name FROM Invoice i JOIN Employee e ON i.employeeid = e.id";
                await _connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        invoices.Add(new Invoice
                        {
                            Id = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            Timestamp = reader.GetDateTime(2),
                            Total = reader.GetDecimal(3),
                            IsPaid = reader.GetBoolean(4),
                            EmployeeName = reader.GetString(5)
                        });
                    }
                }

                return invoices;
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

        public async Task DeleteInvoice(int invoiceId)
        {
            try
            {
                string query = "DELETE FROM Invoice WHERE id = @id";
                await _connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("id", invoiceId);
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

        public async Task UpdateInvoiceIsPaid(int invoiceId, bool isPaid)
        {
            try
            {
                string query = "UPDATE Invoice SET ispaid = @ispaid WHERE id = @id";
                await _connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("ispaid", isPaid);
                    cmd.Parameters.AddWithValue("id", invoiceId);
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
    }
}
