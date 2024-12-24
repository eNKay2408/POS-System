using POSSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.Repositories
{
    public interface IInvoiceItemRepository
    {
        public Task<List<InvoiceItem>> GetInvoiceItemsByInvoiceId(int invoiceId);
        public Task AddInvoiceItem(InvoiceItem invoiceItem);
        public Task UpdateInvoiceItem(InvoiceItem invoiceItem);
        public Task DeleteInvoiceItem(int invoiceItemId);
        public Task DeleteInvoiceItemsByInvoiceId(int invoiceId);
    }
}
