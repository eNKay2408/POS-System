using System.Collections.Generic;
using System.Threading.Tasks;
using POSSystem.Models;

namespace POSSystem.Services
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSession(List<InvoiceItem> invoiceItems, decimal total);
    }
}
