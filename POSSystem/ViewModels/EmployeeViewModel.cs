using POSSystem.Models;
using POSSystem.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    //TODO: need to use FullyObservableCollection instead of keep querying the database
    public class EmployeeViewModel:BaseViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;

        private List<Employee> _employees;

        public List<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        public EmployeeViewModel()
        {
            _employeeRepository = new EmployeeRepository();

            Employees = new List<Employee>();

            try
            {
                _ = LoadEmployees();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // Constructor for Unit testing
        public EmployeeViewModel(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        private async Task LoadEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployees();

                foreach(var employee in employees) {
                    employee.Index = employees.IndexOf(employee) + 1;
                }

                Employees = employees;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task AddEmployee(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email cannot be empty.");
                }

                if(string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Password cannot be empty.");
                }

                var employee = new Employee
                {
                    Name = name,
                    Email = email,
                    Password = password
                };

                await _employeeRepository.SaveEmployee(employee);
                await LoadEmployees();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task DeleteEmployee(int employeeID)
        {
            try
            {
                await _employeeRepository.DeleteEmployee(employeeID);
                await LoadEmployees();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateEmployee(int oldId, string newName, string newEmail)
        {
            try
            {
                Employee newEmployee = new Employee
                {
                    Id = oldId,
                    Name = newName,
                    Email = newEmail
                };

                await _employeeRepository.UpdateEmployee(newEmployee);
                await LoadEmployees();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


    }
}
