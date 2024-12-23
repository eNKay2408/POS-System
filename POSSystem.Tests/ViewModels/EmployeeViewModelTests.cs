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
    public class EmployeeViewModelTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private EmployeeViewModel _employeeViewModel;
        private List<Employee> _employees;

        [TestInitialize]
        public void Setup()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeViewModel = new EmployeeViewModel(_employeeRepositoryMock.Object);

            _employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Employee A", Email="A@gmail.com" },
                new Employee { Id = 2, Name = "Employee B", Email= "B@gmail.com" }
            };
        }

        [TestMethod()]
        public async Task LoadEmployees_ShouldLoadEmployeesIntoViewModel()
        {
            // Arrange
            _employeeRepositoryMock.Setup(repo => repo.GetAllEmployees()).ReturnsAsync(_employees);

            // Act
            await _employeeViewModel.LoadEmployees();

            // Assert
            Assert.AreEqual(2, _employeeViewModel.Employees.Count);
            Assert.AreEqual("Employee A", _employeeViewModel.Employees[0].Name);
            Assert.AreEqual(1, _employeeViewModel.Employees[0].Index);
        }

        [TestMethod()]
        public async Task UpdateEmployee_ShouldUpdateEmployeeAndReloadEmployees()
        {
            // Arrange
            var employeeId = 1;
            var updatedEmployeeName = "Employee A Updated";
            var updatedEmployeeEmail = "AUpdated@gmail.com";

            _employeeRepositoryMock.Setup(repo => repo.UpdateEmployee(It.IsAny<Employee>())).Returns(Task.CompletedTask);
            _employeeRepositoryMock.Setup(repo => repo.GetAllEmployees()).ReturnsAsync(() =>
            {
                _employees[0].Name = updatedEmployeeName;
                _employees[0].Email = updatedEmployeeEmail;
                return _employees;
            });

            // Act
            await _employeeViewModel.UpdateEmployee(employeeId, updatedEmployeeName, updatedEmployeeEmail);

            // Assert
            _employeeRepositoryMock.Verify(repo => repo.UpdateEmployee(It.Is<Employee>(e => e.Id == employeeId && e.Name == updatedEmployeeName && e.Email == updatedEmployeeEmail)), Times.Once);
            Assert.AreEqual(2, _employeeViewModel.Employees.Count);
            Assert.AreEqual(updatedEmployeeName, _employeeViewModel.Employees[0].Name);
            Assert.AreEqual(updatedEmployeeEmail, _employeeViewModel.Employees[0].Email);
        }

        [TestMethod()]
        public async Task UpdateEmployee_WhenNameIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var employeeId = 1;
            var updatedEmployeeName = "";
            var updatedEmployeeEmail = "AUpdated@gmail.com";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _employeeViewModel.UpdateEmployee(employeeId, updatedEmployeeName, updatedEmployeeEmail));
            Assert.AreEqual("Employee name cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateEmployee_WhenNameAllWhiteSpaces_ShouldThrowArgumentException()
        {
            // Arrange
            var employeeId = 1;
            var updatedEmployeeName = "   ";
            var updatedEmployeeEmail = "AUpdated@gmail.com";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _employeeViewModel.UpdateEmployee(employeeId, updatedEmployeeName, updatedEmployeeEmail));
            Assert.AreEqual("Employee name cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateEmployee_WhenEmailIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var employeeId = 1;
            var updatedEmployeeName = "Employee A Updated";
            var updatedEmployeeEmail = "";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _employeeViewModel.UpdateEmployee(employeeId, updatedEmployeeName, updatedEmployeeEmail));
            Assert.AreEqual("Employee email cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateEmployee_WhenEmailAllWhiteSpaces_ShouldThrowArgumentException()
        {
            // Arrange
            var employeeId = 1;
            var updatedEmployeeName = "Employee A Updated";
            var updatedEmployeeEmail = "   ";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _employeeViewModel.UpdateEmployee(employeeId, updatedEmployeeName, updatedEmployeeEmail));
            Assert.AreEqual("Employee email cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateEmployee_WhenEmailIsInvalid_ShouldThrowArgumentException()
        {
            // Arrange
            var employeeId = 1;
            var updatedEmployeeName = "Employee A Updated";
            var updatedEmployeeEmail = "invalid-email";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _employeeViewModel.UpdateEmployee(employeeId, updatedEmployeeName, updatedEmployeeEmail));
            Assert.AreEqual("Employee email is not in the correct format.", ex.Message);
        }

        [TestMethod()]
        public async Task DeleteEmployee_ShouldDeleteEmployeeAndReloadEmployees()
        {
            // Arrange
            var employeeId = 1;

            _employeeRepositoryMock.Setup(repo => repo.DeleteEmployee(employeeId)).Returns(Task.CompletedTask);
            _employeeRepositoryMock.Setup(repo => repo.GetAllEmployees()).ReturnsAsync(() =>
            {
                _employees.RemoveAt(0);
                return _employees;
            });

            // Act
            await _employeeViewModel.DeleteEmployee(employeeId);

            // Assert
            _employeeRepositoryMock.Verify(repo => repo.DeleteEmployee(employeeId), Times.Once);
            Assert.AreEqual(1, _employeeViewModel.Employees.Count);
            Assert.AreEqual("Employee B", _employeeViewModel.Employees[0].Name);
        }
    }
}
