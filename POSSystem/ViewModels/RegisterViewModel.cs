using POSSystem.Models;
using POSSystem.Repository;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;

        public RegisterViewModel()
        {
            _employeeRepository = new EmployeeRepository(ConnectionString);
        }

        private bool IsStrongPassword(string password)
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

        bool IsEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w)+)+)$");
        }

        public async Task SaveEmployee(string Name, string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                throw new System.Exception("Passwords do not match");
            }

            if (!IsStrongPassword(Password))
            {
                throw new System.Exception("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one number");
            }

            if (!IsEmail(Email))
            {
                throw new System.Exception("Invalid email address");
            }

            Employee employee = new Employee
            {
                Name = Name,
                Email = Email,
                Password = Password
            };

            await _employeeRepository.SaveEmployee(employee);
        }
    }
}
