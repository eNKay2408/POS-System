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
    [TestClass]
    public class InvoiceAddViewModelTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IInvoiceItemRepository> _invoiceItemRepositoryMock;
        private Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private InvoiceAddViewModel _invoiceAddViewModel;

        [TestInitialize]
        public void Setup()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _invoiceItemRepositoryMock = new Mock<IInvoiceItemRepository>();
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();

            _invoiceAddViewModel = new InvoiceAddViewModel(
                _employeeRepositoryMock.Object,
                _invoiceItemRepositoryMock.Object,
                _invoiceRepositoryMock.Object,
                _productRepositoryMock.Object
            );
        }

        [TestMethod]
        public async Task LoadEmployees_ShouldLoadEmployeesIntoViewModel()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Email = "john@gmail.com" },
                new Employee { Id = 2, Name = "Jane Doe", Email = "jane@gmail.com" }
                };

            _employeeRepositoryMock.Setup(repo => repo.GetAllEmployees()).ReturnsAsync(employees);

            // Act
            await _invoiceAddViewModel.LoadEmployees();

            // Assert
            Assert.AreEqual(2, _invoiceAddViewModel.Employees.Count);
            Assert.AreEqual("John Doe", _invoiceAddViewModel.Employees[0].Name);
            Assert.AreEqual("Jane Doe", _invoiceAddViewModel.Employees[1].Name);
        }

        [TestMethod]
        public async Task LoadInvoiceItems_ShouldLoadInvoiceItemsIntoViewModel()
        {
            // Arrange
            _invoiceAddViewModel.InvoiceId = 1;

            var invoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem { Id = 1, ProductId = 1, Quantity = 2, UnitPrice = 50 },
                new InvoiceItem { Id = 2, ProductId = 2, Quantity = 3, UnitPrice = 100 }
            };
            _invoiceItemRepositoryMock.Setup(repo => repo.GetInvoiceItemsByInvoiceId(1)).ReturnsAsync(invoiceItems);

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Price = 50 },
                new Product { Id = 2, Name = "Product B", Price = 100 }
            };

            _productRepositoryMock.Setup(repo => repo.GetProductById(1)).ReturnsAsync(products[0]);
            _productRepositoryMock.Setup(repo => repo.GetProductById(2)).ReturnsAsync(products[1]);

            // Act
            await _invoiceAddViewModel.LoadInvoiceItems();

            // Assert
            Assert.AreEqual(2, _invoiceAddViewModel.InvoiceItems.Count);
            Assert.AreEqual(400, _invoiceAddViewModel.Total);
            Assert.AreEqual(1, _invoiceAddViewModel.InvoiceItems[0].Id);
            Assert.AreEqual(1, _invoiceAddViewModel.InvoiceItems[0].Index);
            Assert.AreEqual("Product A", _invoiceAddViewModel.InvoiceItems[0].ProductName);
        }

        [TestMethod]
        public async Task DeleteItem_ShouldDeleteInvoiceItem()
        {
            // Arrange
            var invoiceItem = new InvoiceItem { Id = 1, ProductId = 1, Quantity = 2, UnitPrice = 50 };

            _invoiceAddViewModel.InvoiceId = 1;
            _invoiceItemRepositoryMock.Setup(repo => repo.GetInvoiceItemsByInvoiceId(1)).ReturnsAsync(new List<InvoiceItem>());

            // Act
            await _invoiceAddViewModel.DeleteItem(invoiceItem);

            // Assert
            _invoiceItemRepositoryMock.Verify(repo => repo.DeleteInvoiceItem(1), Times.Once);
        }

        [TestMethod]
        public async Task SaveInvoice_ShouldSaveInvoice()
        {
            // Arrange
            _invoiceAddViewModel.InvoiceId = 1;
            _invoiceAddViewModel.SelectedEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@gmail.com"
            };
            _invoiceAddViewModel.Total = 100;
            _invoiceAddViewModel.InvoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem { ProductId = 1, Quantity = 2, UnitPrice = 50 },
                new InvoiceItem { ProductId = 2, Quantity = 3, UnitPrice = 100 }
            };

            // Act
            await _invoiceAddViewModel.SaveInvoice();

            // Assert
            _invoiceRepositoryMock.Verify(repo => repo.SaveInvoice(It.IsAny<Invoice>()), Times.Once);
        }

        [TestMethod]
        public async Task SaveInvoice_ShouldNotSaveInvoiceIfEmployeeIsNotSelected()
        {
            // Arrange
            _invoiceAddViewModel.InvoiceId = 1;
            _invoiceAddViewModel.SelectedEmployee = null;
            _invoiceAddViewModel.Total = 100;
            _invoiceAddViewModel.InvoiceItems = new List<InvoiceItem>
            {
                new InvoiceItem { ProductId = 1, Quantity = 2, UnitPrice = 50 },
                new InvoiceItem { ProductId = 2, Quantity = 3, UnitPrice = 100 }
            };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _invoiceAddViewModel.SaveInvoice());
            Assert.AreEqual("Employee is required.", ex.Message);
        }

        [TestMethod]
        public async Task SaveInvoice_ShouldNotSaveInvoiceIfNoItemsAreAdded()
        {
            // Arrange
            _invoiceAddViewModel.InvoiceId = 1;
            _invoiceAddViewModel.SelectedEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@gmail.com"
            };
            _invoiceAddViewModel.Total = 0;
            _invoiceAddViewModel.InvoiceItems = new List<InvoiceItem>();

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _invoiceAddViewModel.SaveInvoice());
            Assert.AreEqual("Invoice must have at least one item.", ex.Message);
        }

        [TestMethod]
        public async Task DiscardChanges_ShouldDiscardChanges()
        {
            // Arrange
            _invoiceAddViewModel.InvoiceId = 1;

            // Act
            await _invoiceAddViewModel.DiscardChanges();

            // Assert
            _invoiceItemRepositoryMock.Verify(repo => repo.DeleteInvoiceItemsByInvoiceId(1), Times.Once);
            _invoiceRepositoryMock.Verify(repo => repo.DeleteInvoice(1), Times.Once);
        }
    }
}