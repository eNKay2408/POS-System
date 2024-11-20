using POSSystem.Helpers;
using Stripe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.Services
{
    public class StripeService : IStripeService
    {
        private ConfigHelper _configHelper;

        public StripeService()
        {
            _configHelper = new ConfigHelper();
            StripeConfiguration.ApiKey = _configHelper.GetStripeSecretKey();
        }

        public async Task<string> CreateCheckoutSession(Models.Product product, int quantity)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(product.Price * 100),
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
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
                var service = new Stripe.Checkout.SessionService();
                var session = await service.CreateAsync(options);
                return session.Url;
            }
            catch (StripeException)
            {
                throw;
            }
        }
    }
}
