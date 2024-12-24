using POSSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.Repositories
{
    public interface IInvoiceRepository
    {
        Task<int> CreateInvoice(Invoice invoice);
        Task SaveInvoice(Invoice invoice);
        Task<List<Invoice>> GetAllInvoices();
        Task DeleteInvoice(int invoiceId);
        Task UpdateInvoiceIsPaid(int invoiceId, bool isPaid);
    }
}
