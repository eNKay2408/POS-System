using POSSystem.Helpers;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using POSSystem.Views;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    //TODO: need to use FullyObservableCollection instead of keep querying the database
    public class EmployeeViewModel : BaseViewModel
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
            _employeeRepository = ServiceFactory.GetChildOf<IEmployeeRepository>();

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

        public async Task LoadEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployees();

                foreach (var employee in employees)
                {
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
        
        public async Task DeleteEmployee(int employeeID)
        {
            try
            {
                if (await _employeeRepository.HasReferencingInvoices(employeeID))
                {
                    await DialogHelper.DisplayErrorDialog("Cannot delete the employee because there are some invoice(s) created by this employee.\nPLEASE DELETE THE INVOICE(S) OR UPDATE IT NOT TO BE REGISTERED WITH THE EMPLOYEE!");
                    return;
                }

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
                if (string.IsNullOrWhiteSpace(newName))
                {
                    throw new ArgumentException("Employee name cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(newEmail))
                {
                    throw new ArgumentException("Employee email cannot be empty.");
                }

                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(newEmail, emailPattern))
                {
                    throw new ArgumentException("Employee email is not in the correct format.");
                }

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
