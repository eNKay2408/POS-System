using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;

        public RegisterViewModel()
        {
            _employeeRepository = ServiceFactory.GetChildOf<IEmployeeRepository>();
        }

        // Constructor for unit testing
        public RegisterViewModel(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public static bool IsStrongPassword(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasNumber = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpperCase = true;
                }
                if (char.IsLower(c))
                {
                    hasLowerCase = true;
                }
                if (char.IsDigit(c))
                {
                    hasNumber = true;
                }
            }

            if (!hasUpperCase || !hasLowerCase || !hasNumber)
            {
                return false;
            }

            return true;
        }

        public static bool IsEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w)+)+)$");
        }

        public async Task SaveEmployee(string Name, string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                throw new ArgumentException("Passwords do not match");
            }

            if (!IsStrongPassword(Password))
            {
                throw new ArgumentException("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number");
            }

            if (!IsEmail(Email))
            {
                throw new FormatException("Invalid email address");
            }

            Employee employee = new Employee
            {
                Name = Name,
                Email = Email,
                Password = Password
            };

            try
            {
                await _employeeRepository.SaveEmployee(employee);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
