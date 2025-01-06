using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.Tests.ViewModels
{
    [TestClass()]
    public class InvoiceAddItemViewModelTests
    {
        private Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IBrandRepository> _brandRepositoryMock;
        private Mock<IInvoiceItemRepository> _invoiceItemRepositoryMock;

        private InvoiceAddItemViewModel _invoiceAddItemViewModel;

        [TestInitialize]
        public void Setup()
        {
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _brandRepositoryMock = new Mock<IBrandRepository>();
            _invoiceItemRepositoryMock = new Mock<IInvoiceItemRepository>();

            _invoiceAddItemViewModel = new InvoiceAddItemViewModel(
                _invoiceRepositoryMock.Object,
                _productRepositoryMock.Object,
                _categoryRepositoryMock.Object,
                _brandRepositoryMock.Object,
                _invoiceItemRepositoryMock.Object
            );
        }

        [TestMethod()]
        public async Task LoadProducts_ShouldLoadProductsIntoViewModel()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 100, CategoryId = 1, BrandId = 1 },
                new Product { Id = 2, Name = "Product 2", Price = 200, CategoryId = 2, BrandId = 2 }
            };

            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(products);

            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };

            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(1)).ReturnsAsync(categories[0]);
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(2)).ReturnsAsync(categories[1]);

            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Brand 1" },
                new Brand { Id = 2, Name = "Brand 2" }
            };

            _brandRepositoryMock.Setup(repo => repo.GetBrandById(1)).ReturnsAsync(brands[0]);
            _brandRepositoryMock.Setup(repo => repo.GetBrandById(2)).ReturnsAsync(brands[1]);

            // Act
            await _invoiceAddItemViewModel.LoadProducts();

            // Assert
            Assert.AreEqual(2, _invoiceAddItemViewModel.Products.Count);
            Assert.AreEqual(1, _invoiceAddItemViewModel.Products[0].Id);
            Assert.AreEqual("Category 1", _invoiceAddItemViewModel.Products[0].CategoryName);
            Assert.AreEqual("Brand 1", _invoiceAddItemViewModel.Products[0].BrandName);
            Assert.AreEqual(1, _invoiceAddItemViewModel.Products[0].Index);
        }

        [TestMethod()]
        public void LoadSelectedProduct_ShouldLoadSelectedProductIntoViewModel()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100, CategoryId = 1, BrandId = 1 };
            _invoiceAddItemViewModel.InvoiceItem = new InvoiceItem { ProductId = 1 };
            _invoiceAddItemViewModel.Products = new List<Product> { product };

            // Act
            _invoiceAddItemViewModel.LoadSelectedProduct();

            // Assert
            Assert.AreEqual(product, _invoiceAddItemViewModel.CurrentProduct);
        }

        [TestMethod()]
        public async Task SaveInvoiceItem_WhenProductIsNew_ShouldSaveInvoiceItem()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100, CategoryId = 1, BrandId = 1, Stock = 10 };
            _invoiceAddItemViewModel.CurrentProduct = product;
            _invoiceAddItemViewModel.InvoiceItem = new InvoiceItem { ProductId = 0, Quantity = 1, InvoiceId = 1 };

            // Act
            await _invoiceAddItemViewModel.SaveInvoiceItem();

            // Assert
            _invoiceItemRepositoryMock.Verify(repo => repo.AddInvoiceItem(It.IsAny<InvoiceItem>()), Times.Once);
            Assert.AreEqual(1, _invoiceAddItemViewModel.InvoiceItem.ProductId);
            Assert.AreEqual(100, _invoiceAddItemViewModel.InvoiceItem.UnitPrice);
        }

        [TestMethod()]
        public async Task SaveInvoiceItem_WhenProductIsNotNew_ShouldSaveInvoiceItem()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 100, CategoryId = 1, BrandId = 1, Stock = 10 };
            _invoiceAddItemViewModel.CurrentProduct = product;
            _invoiceAddItemViewModel.InvoiceItem = new InvoiceItem { ProductId = 0, Quantity = 1, InvoiceId = 1, Id = 1 };

            // Act
            await _invoiceAddItemViewModel.SaveInvoiceItem();

            // Assert
            _invoiceItemRepositoryMock.Verify(repo => repo.UpdateInvoiceItem(It.IsAny<InvoiceItem>()), Times.Once);
        }

        [TestMethod()]
        public async Task SaveInvoiceItem_WhenProductIsNotSelected_ShouldThrowException()
        {
            // Arrange
            _invoiceAddItemViewModel.CurrentProduct = new Product { Id = 0 };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _invoiceAddItemViewModel.SaveInvoiceItem());
            Assert.AreEqual("Please select a product.", ex.Message);
        }

        [TestMethod()]
        public async Task SaveInvoiceItem_WhenQuantityIsNotEntered_ShouldThrowException()
        {
            // Arrange
            _invoiceAddItemViewModel.CurrentProduct = new Product { Id = 1 };
            _invoiceAddItemViewModel.InvoiceItem = new InvoiceItem { ProductId = 0, InvoiceId = 0 };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _invoiceAddItemViewModel.SaveInvoiceItem());
            Assert.AreEqual("Please enter a quantity.", ex.Message);
        }

        [TestMethod()]
        public async Task SaveInvoiceItem_WhenQuantityExceedsStock_ShouldThrowException()
        {
            // Arrange
            _invoiceAddItemViewModel.CurrentProduct = new Product { Id = 1, Stock = 10 };
            _invoiceAddItemViewModel.InvoiceItem = new InvoiceItem { ProductId = 0, Quantity = 11, InvoiceId = 1 };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _invoiceAddItemViewModel.SaveInvoiceItem());
            Assert.AreEqual("Quantity exceeds stock.", ex.Message);
        }
    }
}
