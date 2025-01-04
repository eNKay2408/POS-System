using Npgsql;
using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;

namespace POSSystem.Repositories
{
    public class InvoiceItemRepository : BaseRepository, IInvoiceItemRepository
    {
        private readonly NpgsqlConnection _connection;

        public InvoiceItemRepository()
        {
            _connection = new NpgsqlConnection(ConnectionString);
        }

        public async Task<List<InvoiceItem>> GetInvoiceItemsByInvoiceId(int invoiceId)
        {
            var invoiceItems = new List<InvoiceItem>();

            string query = "SELECT i.*, p.name FROM InvoiceItem i JOIN product p ON i.productid = p.id WHERE invoiceid = @invoiceId";
            await _connection.OpenAsync();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        invoiceItems.Add(new InvoiceItem
                        {
                            Id = reader.GetInt32(0),
                            InvoiceId = reader.GetInt32(1),
                            ProductId = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3),
                            UnitPrice = reader.GetDecimal(4),
                            SubTotal = reader.GetDecimal(5),
                            ProductName = reader.GetString(6)
                        });
                    }
                }
            }

            await _connection.CloseAsync();
            return invoiceItems;
        }

        public async Task AddInvoiceItem(InvoiceItem invoiceItem)
        {
            string query = "INSERT INTO InvoiceItem (invoiceid, productid, quantity, unitprice, subtotal) VALUES (@invoiceId, @productId, @quantity, @unitPrice, @subtotal)";
            _connection.Open();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("invoiceId", invoiceItem.InvoiceId);
                cmd.Parameters.AddWithValue("productId", invoiceItem.ProductId);
                cmd.Parameters.AddWithValue("quantity", invoiceItem.Quantity);
                cmd.Parameters.AddWithValue("unitPrice", invoiceItem.UnitPrice);
                cmd.Parameters.AddWithValue("subtotal", invoiceItem.SubTotal);

                await cmd.ExecuteNonQueryAsync();
            }

            _connection.Close();
        }

        public async Task UpdateInvoiceItem(InvoiceItem invoiceItem)
        {
            string query = "UPDATE InvoiceItem SET productid = @productId, quantity = @quantity, unitprice = @unitPrice WHERE id = @id";
            _connection.Open();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("productId", invoiceItem.ProductId);
                cmd.Parameters.AddWithValue("quantity", invoiceItem.Quantity);
                cmd.Parameters.AddWithValue("unitPrice", invoiceItem.UnitPrice);
                cmd.Parameters.AddWithValue("id", invoiceItem.Id);

                await cmd.ExecuteNonQueryAsync();
            }

            _connection.Close();
        }

        public async Task DeleteInvoiceItem(int invoiceItemId)
        {
            string query = "DELETE FROM InvoiceItem WHERE id = @invoiceItemId";
            _connection.Open();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("invoiceItemId", invoiceItemId);

                await cmd.ExecuteNonQueryAsync();
            }

            _connection.Close();
        }

        public async Task DeleteInvoiceItemsByInvoiceId(int invoiceId)
        {
            string query = "DELETE FROM InvoiceItem WHERE invoiceid = @invoiceId";
            _connection.Open();

            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                await cmd.ExecuteNonQueryAsync();
            }

            _connection.Close();
        }
    }
}
