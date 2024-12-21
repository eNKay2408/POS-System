using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Helpers;
using POSSystem.Services;
using Stripe;
using Stripe.Checkout;
using System.Threading;
using System.Threading.Tasks;

namespace POSSystem.Tests.Services
{
    [TestClass()]
    public class StripeServiceTests
    {
        private Mock<IConfigHelper> _configHelperMock;
        private Mock<SessionService> _sessionServiceMock;
        private StripeService _stripeService;

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
            var product = new Models.Product
            {
                Name = "Test Product",
                Price = 10.00m,
                CategoryName = "Test Category",
                BrandName = "Test Brand"
            };
            var quantity = 2;

            _sessionServiceMock
                .Setup(s => s.CreateAsync(
                    It.IsAny<SessionCreateOptions>(),
                    It.IsAny<RequestOptions>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new Session { Url = "https://example.com/session" });

            // Act
            var result = await _stripeService.CreateCheckoutSession(product, quantity);

            // Assert
            Assert.AreEqual("https://example.com/session", result);
        }
    }
}
