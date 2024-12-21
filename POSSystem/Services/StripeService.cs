using POSSystem.Helpers;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.Services
{
    public class StripeService : IStripeService
    {
        private IConfigHelper _configHelper;
        private SessionService _sessionService;

        public StripeService()
        {
            _configHelper = ServiceFactory.GetChildOf<IConfigHelper>();
            _sessionService = new SessionService();
            StripeConfiguration.ApiKey = _configHelper.GetStripeSecretKey();
        }

        // Constructor for unit testing
        public StripeService(IConfigHelper configHelper, SessionService sessionService)
        {
            _configHelper = configHelper;
            _sessionService = sessionService;
            StripeConfiguration.ApiKey = _configHelper.GetStripeSecretKey();
        }

        public async Task<string> CreateCheckoutSession(Models.Product product, int quantity)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(product.Price * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = product.Name,
                                Description = $"Category: {product.CategoryName}, Brand: {product.BrandName}",
                            },
                        },
                        Quantity = quantity,
                    },
                },
                Mode = "payment",
                SuccessUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel"
            };

            try
            {
                var session = await _sessionService.CreateAsync(options);
                return session.Url;
            }
            catch (StripeException)
            {
                throw;
            }
        }
    }
}
