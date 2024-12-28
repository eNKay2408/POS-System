using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class InvoiceAddViewModel : BaseViewModel
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;

        private List<Employee> _employees;
        private Employee _selectedEmployee;
        private List<InvoiceItem> _invoiceItems;
        private int _invoiceId;
        private decimal _total;

        public List<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        public List<InvoiceItem> InvoiceItems
        {
            get => _invoiceItems;
            set
            {
                _invoiceItems = value;
                OnPropertyChanged();
            }
        }

        public int InvoiceId
        {
            get => _invoiceId;
            set
            {
                _invoiceId = value;
                OnPropertyChanged();
            }
        }

        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged();
            }
        }

        public InvoiceAddViewModel()
        {
            _employeeRepository = ServiceFactory.GetChildOf<IEmployeeRepository>();
            _invoiceItemRepository = ServiceFactory.GetChildOf<IInvoiceItemRepository>();
            _invoiceRepository = ServiceFactory.GetChildOf<IInvoiceRepository>();
            _productRepository = ServiceFactory.GetChildOf<IProductRepository>();

            Employees = new List<Employee>();
            InvoiceItems = new List<InvoiceItem>();

            LoadData();
        }

        private async void LoadData()
        {
            await LoadEmployees();
            await LoadInvoiceItems();
            await CreateInvoice();
        }

        public async Task CreateInvoice()
        {
            if (InvoiceId == 0)
            {
                var invoice = new Invoice
                {
                    EmployeeId = 1,
                    Timestamp = DateTime.Now,
                    Total = 0,
                    IsPaid = false
                };

                InvoiceId = await _invoiceRepository.CreateInvoice(invoice);
            }
        }

        private async Task LoadEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            Employees = employees;
        }

        private async Task LoadInvoiceItems()
        {
            var invoiceItems = await _invoiceItemRepository.GetInvoiceItemsByInvoiceId(InvoiceId);
            Total = 0;

            foreach (var item in invoiceItems)
            {
                var product = await _productRepository.GetProductById(item.ProductId);
                item.ProductName = product.Name;

                Total += item.SubTotal;

                item.Index = invoiceItems.IndexOf(item) + 1;
            }

            InvoiceItems = invoiceItems;
        }

        public async Task DeleteItem(InvoiceItem invoiceItem)
        {
            await _invoiceItemRepository.DeleteInvoiceItem(invoiceItem.Id);
            await LoadInvoiceItems();
        }

        public async Task SaveInvoice()
        {
            if (SelectedEmployee == null)
            {
                throw new Exception("Employee is required.");
            }

            if (InvoiceItems.Count == 0)
            {
                throw new Exception("Invoice must have at least one item.");
            }

            var invoice = new Invoice
            {
                Id = InvoiceId,
                EmployeeId = SelectedEmployee.Id,
                Timestamp = DateTime.Now,
                Total = Total,
            };

            await _invoiceRepository.SaveInvoice(invoice);
        }

        public async Task DiscardChanges()
        {
            await _invoiceItemRepository.DeleteInvoiceItemsByInvoiceId(InvoiceId);
            await _invoiceRepository.DeleteInvoice(InvoiceId);
        }
    }
}
