using POSSystem.Repositories;
using POSSystem.Models;
using System.Threading.Tasks;
using System;
using POSSystem.Services;

namespace POSSystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IGoogleOAuthService _googleOAuthService;
        private readonly ISettingsService _settingsService;
        private readonly IEncryptionService _encryptionService;

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
            _employeeRepository = ServiceFactory.GetChildOf<IEmployeeRepository>();
            _googleOAuthService = new GoogleOAuthService();
            _settingsService = new SettingsService();
            _encryptionService = new EncryptionService();
        }

        // Constructor for unit testing
        public LoginViewModel(IEmployeeRepository employeeRepository, IGoogleOAuthService googleOAuthService, ISettingsService settingsService, IEncryptionService encryptionService)
        {
            _employeeRepository = employeeRepository;
            _googleOAuthService = googleOAuthService;
            _settingsService = settingsService;
            _encryptionService = encryptionService;
        }

        public async Task<string> Login()
        {
            Employee employee = await _employeeRepository.GetEmployeeByEmail(Email);

            if (employee == null)
            {
                return null;
            }

            if (BCrypt.Net.BCrypt.Verify(Password, employee.Password))
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
            var encryptedPassword = await _encryptionService.EncryptAsync(Password);

            _settingsService.Save("Email", Email);
            _settingsService.Save("Password", encryptedPassword);
        }

        public async Task LoadSavedCredentials()
        {
            if (_settingsService.Load("Email") != null && _settingsService.Load("Password") != null)
            {
                string savedEmail = _settingsService.Load("Email");
                string encryptedPassword = _settingsService.Load("Password");

                Email = savedEmail;

                Password = await _encryptionService.DecryptAsync(encryptedPassword);

                IsRememberMeChecked = true;
            }
        }

        public void ClearSavedCredentials()
        {
            _settingsService.Remove("Email");
            _settingsService.Remove("Password");
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
