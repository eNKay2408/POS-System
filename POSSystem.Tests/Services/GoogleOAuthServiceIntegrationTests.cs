using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSSystem.Models;
using POSSystem.Services;
using System.Threading.Tasks;

namespace POSSystem.Tests.Services
{
    public class GoogleOAuthServiceIntegrationTests
    {
        [TestMethod()]
        public async Task AuthenticateAsync_WhenValidCredentials_ShouldReturnsEmployee()
        {
            // Arrange
            var googleOAuthService = new GoogleOAuthService();

            // Act
            var result = await googleOAuthService.AuthenticateAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Employee));
            Assert.IsNotNull(result.Name);
            Assert.IsNotNull(result.Email);
        }
    }
}
