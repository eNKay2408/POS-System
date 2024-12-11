using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSSystem.Models;

namespace POSSystem.Repositories
{
    internal class InvoiceRepository: BaseRepository, IInvoiceRepository
    {
        public Task SaveInvoice(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Invoice>> GetAllInvoices()
        {
            try
            {
                var invoices = new List<Invoice>();
                string query = "SELECT i.*, e.name FROM Invoice i JOIN employee e ON i.employeeid = i.id";
                await Connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, Connection))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        //not reading password
                        invoices.Add(new Invoice
                        {
                            Id = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            Total = reader.GetDecimal(2),
                            Timestamp = reader.GetDateTime(3),
                            EmployeeName = reader.GetString(4)
                        });
                    }
                }

                return invoices;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                await Connection.CloseAsync();
            }
        }

        public Task<Invoice> GetInvoiceById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateInvoice(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Task DeleteInvoice(int id)
        {
            throw new NotImplementedException();
        }
    }
}
