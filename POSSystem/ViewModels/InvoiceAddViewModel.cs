using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class InvoiceAddViewModel : BaseViewModel
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


        public InvoiceAddViewModel()
        {
            _employeeRepository = new EmployeeRepository();

            Employees = new List<Employee>();
            LoadData();
        }

        private async void LoadData()
        {
            await LoadEmployees();
        }

        private async Task LoadEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            Employees = employees;
        }
    }
}
