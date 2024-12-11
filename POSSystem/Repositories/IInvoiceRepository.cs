using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.Repositories
{
    public interface IInvoiceRepository
    {
        Task SaveInvoice(Invoice invoice);

        Task<List<Invoice>> GetAllInvoices();

        Task<Invoice> GetInvoiceById(int invoiceId);

        Task UpdateInvoice(int invoiceId, Product product);

        Task DeleteInvoice(int invoiceId);

        Task RemoveProductFromInvoice(int invoiceId, int productId);

        Task AddProductToInvoice(int invoiceId, Product product);

        Task <List<Product>> GetAllProductsOfInvoice(int invoiceId);
    }
}
