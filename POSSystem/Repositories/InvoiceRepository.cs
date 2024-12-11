using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSSystem.Models;

namespace POSSystem.Repositories
{
    internal class InvoiceRepository : BaseRepository, IInvoiceRepository
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

        public Task<Invoice> GetInvoiceById(int invoiceId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateInvoice(int invoiceId, Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteInvoice(int invoiceId)
        {
            throw new NotImplementedException();
        }

        public  Task RemoveProductFromInvoice(int invoiceId, int productId)
        {
            throw new NotImplementedException();
        }

        public  Task AddProductToInvoice(int invoiceId, Product product)
        {
            throw new NotImplementedException();
        }


        public async Task<List<Product>> GetAllProductsOfInvoice(int invoiceId)
        {
            try
            {
                var products = new List<Product>();
                string query = "SELECT i.quantity, p.name, p.price, b.name, c.name FROM invoice_detail i JOIN product p ON i.productid = p.id JOIN brand b on b.id = p.brandid JOIN category c on c.id = p.categoryid WHERE invoiceid = @invoiceid";
                await Connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("id", invoiceId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new Product
                            {
                                Stock = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2),
                                BrandName = reader.GetString(3),
                                CategoryName = reader.GetString(4)
                            });
                        }
                    }
                }

                return products;
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
