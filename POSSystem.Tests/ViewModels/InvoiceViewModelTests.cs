using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Repositories;
using POSSystem.Services;
using POSSystem.ViewModels;
using POSSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.Tests.ViewModels
{
    [TestClass()]
    public class InvoiceViewModelTests
    {
        private Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private Mock<IInvoiceItemRepository> _invoiceItemRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IStripeService> _stripeServiceMock;
        private Mock<IUriLauncher> _uriLauncherMock;
        private InvoiceViewModel _invoiceViewModel;

        private List<Invoice> _invoices;

        [TestInitialize]
        public void Setup()
        {
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _invoiceItemRepositoryMock = new Mock<IInvoiceItemRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _stripeServiceMock = new Mock<IStripeService>();
            _uriLauncherMock = new Mock<IUriLauncher>();

            _invoiceViewModel = new InvoiceViewModel(
                _invoiceRepositoryMock.Object,
                _invoiceItemRepositoryMock.Object,
                _productRepositoryMock.Object,
                _stripeServiceMock.Object,
                _uriLauncherMock.Object
            );

            _invoices = new List<Invoice>
            {
                new Invoice { Id = 1, Total = 100, Timestamp = DateTime.UtcNow, IsPaid = false },
                new Invoice { Id = 2, Total = 200, Timestamp = DateTime.UtcNow, IsPaid = false }
            };
        }

        [TestMethod()]
        public async Task LoadInvoices_ShouldLoadInvoicesIntoViewModel()
        {
            // Arrange
            _invoiceRepositoryMock.Setup(repo => repo.GetAllInvoices()).ReturnsAsync(_invoices);

            // Act
            await _invoiceViewModel.LoadInvoices();

            // Assert
            Assert.AreEqual(2, _invoiceViewModel.Invoices.Count);
            Assert.AreEqual(1, _invoiceViewModel.Invoices[0].Index);
            Assert.AreEqual(100, _invoiceViewModel.Invoices[0].Total);
        }

        [TestMethod()]
        public async Task PayInvoice_ShouldProcessPaymentAndReloadInvoices()
        {
            // Arrange
            var invoice = _invoices[0];
            var invoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem { InvoiceId = invoice.Id, ProductId = 1, Quantity = 1, UnitPrice = 100 }
            };
            var product = new Product { Id = 1, Name = "Product A" };

            _invoiceItemRepositoryMock.Setup(repo => repo.GetInvoiceItemsByInvoiceId(invoice.Id)).ReturnsAsync(invoiceItems);
            _productRepositoryMock.Setup(repo => repo.GetProductById(1)).ReturnsAsync(product);
            _stripeServiceMock.Setup(service => service.CreateCheckoutSession(invoiceItems, invoice.Total)).ReturnsAsync("https://checkout.stripe.com/");
            _uriLauncherMock.Setup(launcher => launcher.LaunchUriAsync(It.IsAny<Uri>())).Returns(Task.CompletedTask);
            _invoiceRepositoryMock.Setup(repo => repo.UpdateInvoiceIsPaid(invoice.Id, true)).Returns(Task.CompletedTask);
            _invoiceRepositoryMock.Setup(repo => repo.GetAllInvoices()).ReturnsAsync(_invoices);

            // Act
            await _invoiceViewModel.PayInvoice(invoice);

            // Assert
            _invoiceItemRepositoryMock.Verify(repo => repo.GetInvoiceItemsByInvoiceId(invoice.Id), Times.Once);
            _productRepositoryMock.Verify(repo => repo.GetProductById(1), Times.Once);
            _stripeServiceMock.Verify(service => service.CreateCheckoutSession(invoiceItems, invoice.Total), Times.Once);
            _uriLauncherMock.Verify(launcher => launcher.LaunchUriAsync(It.Is<Uri>(uri => uri.ToString() == "https://checkout.stripe.com/")), Times.Once);
            _invoiceRepositoryMock.Verify(repo => repo.UpdateInvoiceIsPaid(invoice.Id, true), Times.Once);
        }

        [TestMethod()]
        public async Task LoadInvoices_WhenRepositoryFails_ShouldThrowException()
        {
            // Arrange
            _invoiceRepositoryMock.Setup(repo => repo.GetAllInvoices()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _invoiceViewModel.LoadInvoices());
            Assert.AreEqual("Database error", ex.Message);
        }
    }
}
