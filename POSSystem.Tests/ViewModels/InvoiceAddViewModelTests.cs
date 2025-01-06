using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Helpers;
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
        public void DeleteInvoiceItem_ShouldRemoveItemAndRecalculateTotal()
        {
            // Arrange
            var invoiceItems = new FullObservableCollection<InvoiceItem>
            {
                new InvoiceItem { Id = 1, ProductId = 1, Quantity = 2, UnitPrice = 50, SubTotal = 100 },
                new InvoiceItem { Id = 2, ProductId = 2, Quantity = 1, UnitPrice = 30, SubTotal = 30 }
            };
            _invoiceAddViewModel.InvoiceItems = invoiceItems;
            _invoiceAddViewModel.Total = 130;

            // Act
            _invoiceAddViewModel.DeleteInvoiceItem(0);

            // Assert
            Assert.AreEqual(1, _invoiceAddViewModel.InvoiceItems.Count);
            Assert.AreEqual(30, _invoiceAddViewModel.Total);
        }

        [TestMethod]
        public async Task DeleteItem_ShouldDeleteInvoiceItem()
        {
            // Arrange
            var invoiceItem = new InvoiceItem { Id = 1, ProductId = 1, Quantity = 2, UnitPrice = 50 };

            // Act
            await _invoiceAddViewModel.DeleteItem(invoiceItem);

            // Assert
            _invoiceItemRepositoryMock.Verify(repo => repo.DeleteInvoiceItem(1), Times.Once);
        }

        [TestMethod]
        public async Task SaveInvoice_ShouldSaveInvoice()
        {
            // Arrange
            _invoiceAddViewModel.SelectedEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@gmail.com"
            };
            _invoiceAddViewModel.Total = 100;
            _invoiceAddViewModel.InvoiceItems = new FullObservableCollection<InvoiceItem>
            {
                new InvoiceItem { ProductId = 1, Quantity = 2, UnitPrice = 50, SubTotal = 100 }
            };

            // Act
            await _invoiceAddViewModel.SaveInvoice();

            // Assert
            _invoiceRepositoryMock.Verify(repo => repo.SaveInvoice(It.IsAny<Invoice>()), Times.Once);
            _invoiceItemRepositoryMock.Verify(repo => repo.AddInvoiceItem(It.IsAny<InvoiceItem>()), Times.Once);
        }

        [TestMethod]
        public async Task SaveInvoice_ShouldNotSaveInvoiceIfEmployeeIsNotSelected()
        {
            // Arrange
            _invoiceAddViewModel.SelectedEmployee = null;
            _invoiceAddViewModel.Total = 100;
            _invoiceAddViewModel.InvoiceItems = new FullObservableCollection<InvoiceItem>
            {
                new InvoiceItem { ProductId = 1, Quantity = 2, UnitPrice = 50, SubTotal = 100 }
            };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _invoiceAddViewModel.SaveInvoice());
            Assert.AreEqual("Employee is required.", ex.Message);
        }

        [TestMethod]
        public async Task SaveInvoice_ShouldNotSaveInvoiceIfNoItemsAreAdded()
        {
            // Arrange
            _invoiceAddViewModel.SelectedEmployee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@gmail.com"
            };
            _invoiceAddViewModel.Total = 0;
            _invoiceAddViewModel.InvoiceItems = new FullObservableCollection<InvoiceItem>();

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _invoiceAddViewModel.SaveInvoice());
            Assert.AreEqual("Invoice must have at least one item.", ex.Message);
        }

        [TestMethod]
        public void DiscardChanges_ShouldClearInvoiceItemsAndResetTotal()
        {
            // Arrange
            _invoiceAddViewModel.InvoiceItems = new FullObservableCollection<InvoiceItem>
            {
                new InvoiceItem { ProductId = 1, Quantity = 2, UnitPrice = 50, SubTotal = 100 }
            };
            _invoiceAddViewModel.Total = 100;

            // Act
            _invoiceAddViewModel.DiscardChanges();

            // Assert
            Assert.AreEqual(0, _invoiceAddViewModel.InvoiceItems.Count);
            Assert.AreEqual(0, _invoiceAddViewModel.Total);
            Assert.IsNull(_invoiceAddViewModel.SelectedEmployee);
        }

        [TestMethod]
        public void AddItemToInvoice_ShouldAddNewItemAndRecalculateTotal()
        {
            // Arrange
            var newItem = new InvoiceItem { ProductId = 1, Quantity = 2, UnitPrice = 50 };

            // Act
            _invoiceAddViewModel.AddItemToInvoice(newItem);

            // Assert
            Assert.AreEqual(1, _invoiceAddViewModel.InvoiceItems.Count);
            Assert.AreEqual(100, _invoiceAddViewModel.Total);
        }

        [TestMethod]
        public void UpdateInvoiceItem_ShouldUpdateExistingItemAndRecalculateTotal()
        {
            // Arrange
            var existingItem = new InvoiceItem { ProductId = 1, Quantity = 2, UnitPrice = 50, SubTotal = 100, Index = 0 };
            _invoiceAddViewModel.InvoiceItems = new FullObservableCollection<InvoiceItem> { existingItem };
            var updatedItem = new InvoiceItem { ProductId = 1, Quantity = 3, UnitPrice = 50, SubTotal = 150, Index = 0 };

            // Act
            _invoiceAddViewModel.UpdateInvoiceItem(updatedItem);

            // Assert
            Assert.AreEqual(1, _invoiceAddViewModel.InvoiceItems.Count);
            Assert.AreEqual(150, _invoiceAddViewModel.Total);
        }

        [TestMethod]
        public void DeleteItemFromInvoice_ShouldRemoveItemAndRecalculateTotal()
        {
            // Arrange
            var invoiceItems = new FullObservableCollection<InvoiceItem>
            {
                new InvoiceItem { ProductId = 1, Quantity = 2, UnitPrice = 50, SubTotal = 100 },
                new InvoiceItem { ProductId = 2, Quantity = 1, UnitPrice = 30, SubTotal = 30 }
            };
            _invoiceAddViewModel.InvoiceItems = invoiceItems;
            _invoiceAddViewModel.Total = 130;
            var itemToRemove = invoiceItems[0];

            // Act
            _invoiceAddViewModel.DeleteItemFromInvoice(itemToRemove);

            // Assert
            Assert.AreEqual(1, _invoiceAddViewModel.InvoiceItems.Count);
            Assert.AreEqual(30, _invoiceAddViewModel.Total);
        }

        [TestMethod]
        public async Task ModifyEmployeeAsync_ShouldReloadEmployees()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John Doe", Email = "john@gmail.com" };
            var employees = new List<Employee> { employee };
            _employeeRepositoryMock.Setup(repo => repo.GetAllEmployees()).ReturnsAsync(employees);

            // Act
            await _invoiceAddViewModel.ModifyEmployeeAsync(employee);

            // Assert
            Assert.AreEqual(1, _invoiceAddViewModel.Employees.Count);
            Assert.AreEqual("John Doe", _invoiceAddViewModel.Employees[0].Name);
        }
    }
}