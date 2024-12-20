using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Repositories;
using POSSystem.Services;
using POSSystem.ViewModels;
using POSSystem.Models;
using System.Threading.Tasks;

namespace POSSystem.Tests.ViewModels
{
    [TestClass()]
    public class LoginViewModelTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IGoogleOAuthService> _googleOAuthServiceMock;
        private Mock<ISettingsService> _settingsServiceMock;
        private Mock<IEncryptionService> _encryptionServiceMock;

        private LoginViewModel _loginViewModel;

        private readonly int _id = 1;
        private readonly string _name = "Admin";
        private readonly string _email = "admin@enkay.live";
        private readonly string _password = "12345678Aa";
        private readonly string _encryptedPassword = "$2a$10$P13f37S6P4kN3JaY8i/a/O700M4NCqRv4LtOiLZ3IIozaj3ozqFnG";

        [TestInitialize]
        public void Setup()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _googleOAuthServiceMock = new Mock<IGoogleOAuthService>();
            _settingsServiceMock = new Mock<ISettingsService>();
            _encryptionServiceMock = new Mock<IEncryptionService>();

            _loginViewModel = new LoginViewModel(_employeeRepositoryMock.Object, _googleOAuthServiceMock.Object, _settingsServiceMock.Object, _encryptionServiceMock.Object);
        }

        [TestMethod()]
        public void Login_WhenEmailAndPasswordAreCorrect_ShouldReturnName()
        {
            // Arrange
            _loginViewModel.Email = _email;
            _loginViewModel.Password = _password;

            _employeeRepositoryMock.Setup(repo => repo.GetEmployeeByEmail(_email)).ReturnsAsync(new Employee
            {
                Id = _id,
                Name = _name,
                Email = _email,
                Password = _encryptedPassword
            });

            // Act
            string name = _loginViewModel.Login().Result;

            // Assert
            Assert.AreEqual(_name, name);
        }

        [TestMethod()]
        public void Login_WhenEmailIsIncorrect_ShouldReturnNull()
        {
            // Arrange
            _loginViewModel.Email = _email;
            _loginViewModel.Password = _password;

            _employeeRepositoryMock.Setup(repo => repo.GetEmployeeByEmail(_email)).ReturnsAsync((Employee)null);

            // Act
            string name = _loginViewModel.Login().Result;

            // Assert
            Assert.IsNull(name);
        }

        [TestMethod()]
        public void Login_WhenPasswordIsIncorrect_ShouldReturnsNull()
        {
            // Arrange
            string wrongPassword = "12345678Bb";

            _loginViewModel.Email = _email;
            _loginViewModel.Password = wrongPassword;

            _employeeRepositoryMock.Setup(repo => repo.GetEmployeeByEmail(_email)).ReturnsAsync(new Employee
            {
                Id = _id,
                Name = _name,
                Email = _email,
                Password = _encryptedPassword
            });

            // Act
            string name = _loginViewModel.Login().Result;

            // Assert
            Assert.IsNull(name);
        }

        [TestMethod()]
        public void Login_WhenRememberMeIsChecked_ShouldSaveCredentials()
        {
            // Arrange
            _loginViewModel.Email = _email;
            _loginViewModel.Password = _password;

            _employeeRepositoryMock.Setup(repo => repo.GetEmployeeByEmail(_email)).ReturnsAsync(new Employee
            {
                Id = _id,
                Name = _name,
                Email = _email,
                Password = _encryptedPassword,
            });

            _loginViewModel.IsRememberMeChecked = true;

            // Act
            string name = _loginViewModel.Login().Result;

            // Assert
            _settingsServiceMock.Verify(service => service.Save("Email", _email), Times.Once);
            _settingsServiceMock.Verify(service => service.Save("Password", It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public void Login_WhenRememberMeIsNotChecked_ShouldClearSavedCredentials()
        {
            // Arrange
            _loginViewModel.Email = _email;
            _loginViewModel.Password = _password;

            _employeeRepositoryMock.Setup(repo => repo.GetEmployeeByEmail(_email)).ReturnsAsync(new Employee
            {
                Id = _id,
                Name = _name,
                Email = _email,
                Password = _encryptedPassword,
            });

            _loginViewModel.IsRememberMeChecked = false;

            // Act
            string name = _loginViewModel.Login().Result;

            // Assert
            _settingsServiceMock.Verify(service => service.Remove("Email"), Times.Once);
            _settingsServiceMock.Verify(service => service.Remove("Password"), Times.Once);
        }

        [TestMethod()]
        public async Task SaveCredentialsAsync_ShouldSaveCredentials()
        {
            // Arrange
            _loginViewModel.Email = _email;
            _loginViewModel.Password = _password;

            // Act
            await _loginViewModel.SaveCredentialsAsync();

            // Assert
            _settingsServiceMock.Verify(service => service.Save("Email", _email), Times.Once);
            _settingsServiceMock.Verify(service => service.Save("Password", It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public async Task LoadSavedCredentials_ShouldLoadsSavedCredentials()
        {
            // Arrange
            _settingsServiceMock.Setup(service => service.Load("Email")).Returns(_email);
            _settingsServiceMock.Setup(service => service.Load("Password")).Returns(_encryptedPassword);

            _encryptionServiceMock.Setup(service => service.DecryptAsync(_encryptedPassword)).ReturnsAsync(_password);

            // Act
            await _loginViewModel.LoadSavedCredentials();

            // Assert
            Assert.AreEqual(_email, _loginViewModel.Email);
            Assert.AreEqual(_password, _loginViewModel.Password);
            Assert.IsTrue(_loginViewModel.IsRememberMeChecked);
        }

        [TestMethod()]
        public void ClearSavedCredentials_ShouldRemovesSavedCredentials()
        {
            // Arrange
            _settingsServiceMock.Setup(service => service.Load("Email")).Returns(_email);
            _settingsServiceMock.Setup(service => service.Load("Password")).Returns(_encryptedPassword);

            // Act
            _loginViewModel.ClearSavedCredentials();

            // Assert
            _settingsServiceMock.Verify(service => service.Remove("Email"), Times.Once);
            _settingsServiceMock.Verify(service => service.Remove("Password"), Times.Once);
        }

        [TestMethod()]
        public void AuthenticateWithGoogle_WhenTokenIsValid_ShouldReturnName()
        {
            // Arrange
            Employee employee = new Employee
            {
                Id = 2,
                Name = "eNKay",
                Email = "enkay@enkay.live",
            };

            _googleOAuthServiceMock.Setup(service => service.AuthenticateAsync()).ReturnsAsync(employee);

            // Act
            string name = _loginViewModel.AuthenticateWithGoogle().Result;

            // Assert
            _employeeRepositoryMock.Verify(repo => repo.SaveEmployeeFromGoogle(employee), Times.Once);
            Assert.AreEqual("eNKay", name);
        }
    }
}