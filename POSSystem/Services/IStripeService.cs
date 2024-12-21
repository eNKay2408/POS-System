using System.Threading.Tasks;
using POSSystem.Models;

namespace POSSystem.Services
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSession(Product product, int quantity);
    }
}
