using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSSystem.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace POSSystem.Tests.Services
{
    [TestClass()]
    public class EncryptionServiceTests
    {
        private IEncryptionService _encryptionService;

        [TestInitialize()]
        public void Setup()
        {
            _encryptionService = new EncryptionService();
        }

        [TestMethod()]
        public async Task EncryptAsync_ShouldReturnEncryptedText()
        {
            // Arrange
            string text = "Hello World";

            // Act
            string encryptedText = await _encryptionService.EncryptAsync(text);

            // Assert
            Assert.IsNotNull(encryptedText);
            Assert.AreNotEqual(text, encryptedText);
        }

        [TestMethod()]
        public async Task DecryptAsync_ShouldReturnDecryptedText()
        {
            // Arrange
            string text = "Hello World";
            string encryptedText = await _encryptionService.EncryptAsync(text);

            // Act
            string decryptedText = await _encryptionService.DecryptAsync(encryptedText);

            // Assert
            Assert.IsNotNull(decryptedText);
            Assert.AreEqual(text, decryptedText);
        }
    }
}
