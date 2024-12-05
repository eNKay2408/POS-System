using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Models;
using POSSystem.Repository;
using POSSystem.ViewModels;
using System;
using System.Threading.Tasks;


namespace POSSystem.Tests.ViewModels
{
    [TestClass()]
    public class RegisterViewModelTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;

        private RegisterViewModel _registerViewModel;

        [TestInitialize]
        public void Setup()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();

            _registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
        }

        [TestMethod()]
        public void IsStrongPassword_WhenPasswordContainsUpperCaseLowerCaseAndNumber_ShouldReturnsTrue()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string password = "12345678Aa";

            // Act
            var result = RegisterViewModel.IsStrongPassword(password);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsStrongPassword_WhenPasswordLengthLessThan8_ShouldReturnsFalse()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string password = "1234567";

            // Act
            var result = RegisterViewModel.IsStrongPassword(password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsStrongPassword_WhenPasswordDoesNotContainUpperCase_ShouldReturnsFalse()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string password = "12345678A";

            // Act
            var result = RegisterViewModel.IsStrongPassword(password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsStrongPassword_WhenPasswordDoesNotContainLowerCase_ShouldReturnsFalse()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string password = "12345678A";

            // Act
            var result = RegisterViewModel.IsStrongPassword(password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsStrongPassword_WhenPasswordDoesNotContainNumber_ShouldReturnsFalse()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string password = "ABCDEFGH";

            // Act
            var result = RegisterViewModel.IsStrongPassword(password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsEmail_WhenEmailIsValid_ShouldReturnsTrue()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string email = "admin@enkay.live";

            // Act
            var result = RegisterViewModel.IsEmail(email);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsEmail_WhenEmailIsInvalid_ShouldReturnsFalse_1()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string email = "admin@enkay";

            // Act
            var result = RegisterViewModel.IsEmail(email);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsEmail_WhenEmailIsInvalid_ShouldReturnsFalse_2()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string email = "admin@enkay.";

            // Act
            var result = RegisterViewModel.IsEmail(email);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsEmail_WhenEmailIsInvalid_ShouldReturnsFalse_3()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string email = "admin@.live";

            // Act
            var result = RegisterViewModel.IsEmail(email);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsEmail_WhenEmailIsInvalid_ShouldReturnsFalse_4()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string email = "admin@";

            // Act
            var result = RegisterViewModel.IsEmail(email);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsEmail_WhenEmailIsInvalid_ShouldReturnsFalse_5()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel(_employeeRepositoryMock.Object);
            string email = "admin";

            // Act
            var result = RegisterViewModel.IsEmail(email);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public async Task SaveEmployee_WhenAllParametersAreValid_ShouldCallSaveEmployee()
        {
            // Arrange
            string name = "Admin";
            string email = "admin@enkay.live";
            string password = "12345678Aa";
            string confirmPassword = "12345678Aa";

            // Act
            await _registerViewModel.SaveEmployee(name, email, password, confirmPassword);

            // Assert
            _employeeRepositoryMock.Verify(x => x.SaveEmployee(It.IsAny<Employee>()), Times.Once);
        }

        [TestMethod()]
        public async Task SaveEmployee_WhenPasswordDoesNotMatchConfirmPassword_ShouldThrowArgumentException()
        {
            // Arrange
            string name = "Admin";
            string email = "admin@enkay.live";
            string password = "12345678Aa";
            string confirmPassword = "12345678A";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _registerViewModel.SaveEmployee(name, email, password, confirmPassword));
            Assert.AreEqual("Passwords do not match", ex.Message);
        }

        [TestMethod()]
        public async Task SaveEmployee_WhenPasswordIsNotStrong_ShouldThrowArgumentException()
        {
            // Arrange
            string name = "Admin";
            string email = "admin@enkay.live";
            string password = "12345678";
            string confirmPassword = "12345678";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _registerViewModel.SaveEmployee(name, email, password, confirmPassword));
            Assert.AreEqual("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number", ex.Message);
        }

        [TestMethod()]
        public async Task SaveEmployee_WhenEmailIsInvalid_ShouldThrowFormatException()
        {
            // Arrange
            string name = "Admin";
            string email = "admin@enkay";
            string password = "12345678Aa";
            string confirmPassword = "12345678Aa";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<FormatException>(() => _registerViewModel.SaveEmployee(name, email, password, confirmPassword));
            Assert.AreEqual("Invalid email address", ex.Message);
        }
    }
}