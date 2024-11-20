using System.Threading.Tasks;

namespace POSSystem.Services
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSession(Models.Product product, int quantity);
    }
}
