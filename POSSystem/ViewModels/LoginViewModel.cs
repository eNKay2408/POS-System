using POSSystem.Repository;
using POSSystem.Models;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using static System.Net.WebRequestMethods;
using System.Threading;
using Microsoft.UI.Xaml.Controls;
using System;
using POSSystem.Services;
using POSSystem.Views;
using Windows.Security.Cryptography.DataProtection;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Microsoft.Windows.Storage;


namespace POSSystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly GoogleOAuthService _googleOAuthService;
        private ApplicationDataContainer _localSettings;

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private bool _isRememberMeChecked;
        public bool IsRememberMeChecked
        {
            get { return _isRememberMeChecked; }
            set
            {
                _isRememberMeChecked = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel()
        {
            _employeeRepository = new EmployeeRepository(ConnectionString);
            _googleOAuthService = new GoogleOAuthService();
            _localSettings = ApplicationData.GetDefault().LocalSettings;
        }

        public async Task<string> Login()
        {
            Employee employee = await _employeeRepository.GetEmployeeByEmail(_email);

            if (employee == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.Verify(_password, employee.Password))
            {

                if (_isRememberMeChecked == true)
                {
                    await SaveCredentialsAsync();
                }
                else
                {
                    ClearSavedCredentials();
                }

                return employee.Name;
            }
            else
            {
                return null;
            }
        }

        public async Task SaveCredentialsAsync()
        {
            var provider = new DataProtectionProvider("LOCAL=user");
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(Password, BinaryStringEncoding.Utf8);
            IBuffer protectedBuffer = await provider.ProtectAsync(buffer);
            string encryptedPassword = CryptographicBuffer.EncodeToBase64String(protectedBuffer);

            _localSettings.Values["Email"] = _email;
            _localSettings.Values["Password"] = encryptedPassword;
        }

        public async void LoadSavedCredentials()
        {
            if (_localSettings.Values.ContainsKey("Email") && _localSettings.Values.ContainsKey("Password"))
            {
                string savedEmail = _localSettings.Values["Email"] as string;
                string encryptedPassword = _localSettings.Values["Password"] as string;

                Email = savedEmail;

                var provider = new DataProtectionProvider("LOCAL=user");
                IBuffer protectedBuffer = CryptographicBuffer.DecodeFromBase64String(encryptedPassword);
                IBuffer unprotectedBuffer = await provider.UnprotectAsync(protectedBuffer);
                string decryptedPassword = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, unprotectedBuffer);

                Password = decryptedPassword;

                IsRememberMeChecked = true;
            }
        }

        public void ClearSavedCredentials()
        {
            if (_localSettings.Values.ContainsKey("Email"))
            {
                _localSettings.Values.Remove("Email");
            }
            if (_localSettings.Values.ContainsKey("Password"))
            {
                _localSettings.Values.Remove("Password");
            }
        }

        public async Task<string> AuthenticateWithGoogle()
        {
            try
            {
                var employee = await _googleOAuthService.AuthenticateAsync();

                await _employeeRepository.SaveEmployeeFromGoogle(employee);

                return employee.Name;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
