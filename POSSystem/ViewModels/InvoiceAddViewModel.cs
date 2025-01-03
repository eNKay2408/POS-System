using POSSystem.Helpers;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        //public int InvoiceId
        //{
        //    get => _invoiceId;
        //    set
        //    {
        //        _invoiceId = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        private async void LoadData()
        {
            await LoadEmployees();
            //await LoadInvoiceItems();
            //await CreateInvoice();
        }

        //public async Task CreateInvoice()
        //{
        //    if (InvoiceId == 0)
        //    {
        //        var invoice = new Invoice
        //        {
        //            EmployeeId = 1,
        //            Timestamp = DateTime.Now,
        //            Total = 0,
        //            IsPaid = false
        //        };

        //        InvoiceId = await _invoiceRepository.CreateInvoice(invoice);
        //    }
        //}

        private async Task LoadEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            Employees = employees;
        }

        //private async Task LoadInvoiceItems()
        //{
        //    var invoiceItems = await _invoiceItemRepository.GetInvoiceItemsByInvoiceId(InvoiceId);
        //    Total = 0;

        //    foreach (var item in invoiceItems)
        //    {
        //        var product = await _productRepository.GetProductById(item.ProductId);
        //        item.ProductName = product.Name;

        //        Total += item.SubTotal;

        //        item.Index = invoiceItems.IndexOf(item) + 1;
        //    }

        //    InvoiceItems = new FullObservableCollection<InvoiceItem>(invoiceItems);
        //}

        public void DeleteInvoiceItem(int index)
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
            //await _invoiceItemRepository.DeleteInvoiceItemsByInvoiceId(InvoiceId);
            //await _invoiceRepository.DeleteInvoice(InvoiceId);
            Clear();
        }

        public void Clear()
        {
            InvoiceItems.Clear();
            Total = 0;
            SelectedEmployee = null;
        }

        public void AddItemToInvoice(InvoiceItem invoiceItem)
        {
            if (InvoiceItems == null)
            {
                InvoiceItems = new FullObservableCollection<InvoiceItem>();
            }
            decimal newCost = invoiceItem.Quantity * invoiceItem.UnitPrice;

            var existingItem = InvoiceItems.FirstOrDefault(i => i.ProductId == invoiceItem.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += invoiceItem.Quantity;
                existingItem.SubTotal = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                invoiceItem.Index = InvoiceItems.Count;
                invoiceItem.SubTotal = newCost;
                InvoiceItems.Add(invoiceItem);
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
            //OnPropertyChanged(nameof(_total));
        }

        public void DeleteItemFromInvoice(InvoiceItem item)
        {
            InvoiceItems.Remove(item);
            Total -= item.SubTotal;
        }
    }
}
