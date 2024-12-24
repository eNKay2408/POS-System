using POSSystem.Helpers;
using Stripe;
using Stripe.Checkout;
using System;
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

        public async Task<string> CreateCheckoutSession(List<Models.InvoiceItem> invoiceItems, decimal total)
        {
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in invoiceItems)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(item.UnitPrice * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName,
                        },
                    },
                    Quantity = item.Quantity,
                });
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel"
            };

            try
            {
                var session = await _sessionService.CreateAsync(options);
                return session.Url;
            }
            catch (StripeException ex)
            {
                throw new Exception("Failed to create Stripe Checkout session. Details: " + ex.Message, ex);
            }
        }
    }
}
