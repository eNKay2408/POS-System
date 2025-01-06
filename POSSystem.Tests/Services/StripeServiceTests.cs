using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Helpers;
using POSSystem.Services;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POSSystem.Tests.Services
{
    [TestClass()]
    public class StripeServiceTests
    {
        private Mock<IConfigHelper> _configHelperMock;
        private Mock<SessionService> _sessionServiceMock;
        private IStripeService _stripeService;

        [TestInitialize()]
        public void Setup()
        {
            _configHelperMock = new Mock<IConfigHelper>();
            _configHelperMock.Setup(x => x.GetStripeSecretKey()).Returns("test_secret_key");
            _sessionServiceMock = new Mock<SessionService>();

            _stripeService = new StripeService(_configHelperMock.Object, _sessionServiceMock.Object);
        }

        [TestMethod()]
        public async Task CreateCheckoutSession_ShouldReturnSessionUrl()
        {
            // Arrange
            var invoiceItems = new List<Models.InvoiceItem>
            {
                new Models.InvoiceItem
                {
                    ProductName = "Test Product 1",
                    UnitPrice = 10.00m,
                    Quantity = 2
                },
                new Models.InvoiceItem
                {
                    ProductName = "Test Product 2",
                    UnitPrice = 15.00m,
                    Quantity = 1
                }
            };
            decimal total = 20.00m;

            _sessionServiceMock
                .Setup(s => s.CreateAsync(
                    It.Is<SessionCreateOptions>(options =>
                        options.LineItems.Count == 2 &&
                        options.Mode == "payment" &&
                        options.PaymentMethodTypes.Contains("card") &&
                        options.SuccessUrl == "https://example.com/success" &&
                        options.CancelUrl == "https://example.com/cancel"
                    ),
                    It.IsAny<RequestOptions>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new Session { Url = "https://example.com/session" });

            // Act
            var result = await _stripeService.CreateCheckoutSession(invoiceItems, total);

            // Assert
            Assert.AreEqual("https://example.com/session", result);

            _sessionServiceMock.Verify(s => s.CreateAsync(
                It.IsAny<SessionCreateOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}