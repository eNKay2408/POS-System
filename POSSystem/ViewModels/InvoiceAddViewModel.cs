using POSSystem.Helpers;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using POSSystem.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class InvoiceAddViewModel : BaseViewModel
    {
        private static readonly Lazy<InvoiceAddViewModel> _instance = new Lazy<InvoiceAddViewModel>(() => new InvoiceAddViewModel());

        public static InvoiceAddViewModel Instance => _instance.Value;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;

        private List<Employee> _employees;
        private Employee _selectedEmployee;
        private FullObservableCollection<InvoiceItem> _invoiceItems;
        //private int _invoiceId;
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

        public FullObservableCollection<InvoiceItem> InvoiceItems
        {
            get => _invoiceItems;
            set
            {
                _invoiceItems = value;
                OnPropertyChanged(nameof(InvoiceItems));
            }
        }

        public decimal Total
        {
            get => decimal.Round(_total, 2);
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        private InvoiceAddViewModel()
        {
            _employeeRepository = ServiceFactory.GetChildOf<IEmployeeRepository>();
            _invoiceItemRepository = ServiceFactory.GetChildOf<IInvoiceItemRepository>();
            _invoiceRepository = ServiceFactory.GetChildOf<IInvoiceRepository>();
            _productRepository = ServiceFactory.GetChildOf<IProductRepository>();

            Employees = new List<Employee>();
            InvoiceItems = new();

            LoadData();
        }

        // Constructor for unit testing
        public InvoiceAddViewModel(IEmployeeRepository employeeRepository, IInvoiceItemRepository invoiceItemRepository, IInvoiceRepository invoiceRepository, IProductRepository productRepository)
        {
            _employeeRepository = employeeRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;

            Employees = new List<Employee>();
            InvoiceItems = new List<InvoiceItem>();
        }

        public async void LoadData()
        {
            await LoadEmployees();
        }


        public async Task LoadEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            Employees = employees;
        }

<<<<<<< HEAD
        public void DeleteInvoiceItem(int index)
=======
        public async Task LoadInvoiceItems()
>>>>>>> f4258c7b8e83325b4041d1b900430b5a58a3cd8e
        {
            InvoiceItems.RemoveAt(index);
            CalculateTotal();
        }

        public async Task DeleteItem(InvoiceItem invoiceItem)
        {
            await _invoiceItemRepository.DeleteInvoiceItem(invoiceItem.Id);
            //await LoadInvoiceItems();
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
                EmployeeId = SelectedEmployee.Id,
                Timestamp = DateTime.Now,
                Total = Total,
                IsPaid = false
            };

            int invoiceId = await _invoiceRepository.SaveInvoice(invoice);

            foreach(var item in InvoiceItems)
            {
                item.InvoiceId = invoiceId;
                await _invoiceItemRepository.AddInvoiceItem(item);
            }    
            Clear();
        }

        public void DiscardChanges()
        {
            Clear();
        }

        public void Clear()
        {
            InvoiceItems.Clear();
            Total = 0;
            SelectedEmployee = null;

        }

        public void AddItemToInvoice(InvoiceItem newItem)
        {
            if (InvoiceItems == null)
            {
                InvoiceItems = new FullObservableCollection<InvoiceItem>();
            }
            decimal newCost = newItem.Quantity * newItem.UnitPrice;

            InvoiceItem existingItem = null;
            int maxIndex = -1;
            foreach(var item in InvoiceItems)
            {
                if (item.ProductId == newItem.ProductId)
                {
                    existingItem = item;
                }
                if(item.Index > maxIndex)
                {
                    maxIndex = item.Index;
                }
            }

            if (existingItem != null)
            {
                existingItem.Quantity += newItem.Quantity;
                existingItem.SubTotal = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                newItem.Index = maxIndex + 1;
                newItem.SubTotal = newCost;
                InvoiceItems.Add(newItem);
            }
            Total += newCost;

            OnPropertyChanged(nameof(InvoiceItems));
            OnPropertyChanged(nameof(Total));
        }

        private void CalculateTotal() 
        {
            if (InvoiceItems == null)
            {
                return;
            }

            decimal total = 0;
            foreach (var item in InvoiceItems)
            {
                total += item.SubTotal;
            }

            Total = total;
        }

        public void DeleteItemFromInvoice(InvoiceItem item)
        {
            InvoiceItems.Remove(item);
            Total -= item.SubTotal;
        }

        public void UpdateInvoiceItem(InvoiceItem newItem)
        {
            if (InvoiceItems == null || newItem == null) 
            {
                return;
            }

            bool itemExists = false;
            int existingIndex = 0;
            for(int i = 0; i < InvoiceItems.Count; i++)
            {
                if (InvoiceItems[i].ProductId == newItem.ProductId && InvoiceItems[i].Index == newItem.Index)
                {
                    existingIndex = i;
                }

                if(InvoiceItems[i].ProductId == newItem.ProductId && InvoiceItems[i].Index != newItem.Index)
                {
                    itemExists = true;
                }
            }
            newItem.SubTotal = newItem.Quantity * newItem.UnitPrice;

            if (itemExists)
            {
                throw new Exception("Item already exists in the invoice.");
            }

            InvoiceItems[existingIndex] = newItem;

            CalculateTotal();
            
        }

        public async Task ModifyEmployeeAsync(Employee employee)
        {
            if(SelectedEmployee != null && SelectedEmployee.Id == employee.Id)
            {
                SelectedEmployee = null;
            }
            await LoadEmployees();
        }
    }
}
