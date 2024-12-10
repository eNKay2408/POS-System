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

        Task<Invoice> GetInvoiceById(int id);

        Task UpdateInvoice(Invoice invoice);

        Task DeleteInvoice(int id);
    }
}
