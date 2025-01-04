using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POSSystem.Helpers;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using POSSystem.Views;

namespace POSSystem.ViewModels
{
    public class InvoiceEditViewModel : BaseViewModel
    {
        private static readonly Lazy<InvoiceEditViewModel> _instance = new Lazy<InvoiceEditViewModel>(() => new InvoiceEditViewModel());

        public static InvoiceEditViewModel Instance => _instance.Value;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;

        private List<Employee> _employees;
        private Employee _selectedEmployee;
        private FullObservableCollection<InvoiceItem> _invoiceItems;
        private int _invoiceId;
        private decimal _total;

        public List<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
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

        public int InvoiceId { get; set; }

        private InvoiceEditViewModel()
        {
            _employeeRepository = ServiceFactory.GetChildOf<IEmployeeRepository>();
            _invoiceItemRepository = ServiceFactory.GetChildOf<IInvoiceItemRepository>();
            _invoiceRepository = ServiceFactory.GetChildOf<IInvoiceRepository>();
            _productRepository = ServiceFactory.GetChildOf<IProductRepository>();

            Employees = new List<Employee>();
            SelectedEmployee = new();
            InvoiceItems = new FullObservableCollection<InvoiceItem>();
            Total = 0;

            // Register event handlers
            InvoiceAddItemPage.AddInvoiceItemHanlder += InvoiceEditPage.AddItemToInvoice;
            InvoiceAddItemPage.UpdateInvoiceItemHanlder += InvoiceEditPage.UpdateInvoiceItem;

        }

        public async 
        Task
LoadData()
        {
            await LoadEmployees();
            await LoadInvoiceItems();
        }

        public void SetSelectedEmployee(int employeeId)
        {
            SelectedEmployee = Employees.Find(e => e.Id == employeeId);
        }


        private async Task LoadEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            Employees = employees;
        }

        private async Task LoadInvoiceItems()
        {
            var invoiceItems = await _invoiceItemRepository.GetInvoiceItemsByInvoiceId(InvoiceId);
            InvoiceItems = new FullObservableCollection<InvoiceItem>(invoiceItems);
            CalculateTotal();
        }

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
                IsPaid = false,
                Id = InvoiceId
            };

            await _invoiceRepository.UpdateInvoice(invoice);

            await _invoiceItemRepository.DeleteInvoiceItemsByInvoiceId(InvoiceId);
            foreach (var item in InvoiceItems)
            {
                item.InvoiceId = InvoiceId;
                await _invoiceItemRepository.AddInvoiceItem(item);
            }
            Clear();
        }

        public void Clear()
        {
            InvoiceItems.Clear();
            Total = 0;
            SelectedEmployee = null;

            // Unregister event handlers
            InvoiceAddItemPage.AddInvoiceItemHanlder -= InvoiceEditPage.AddItemToInvoice;
            InvoiceAddItemPage.UpdateInvoiceItemHanlder -= InvoiceEditPage.UpdateInvoiceItem;
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
            foreach (var item in InvoiceItems)
            {
                if (item.ProductId == newItem.ProductId)
                {
                    existingItem = item;
                }
                if (item.Index > maxIndex)
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
            for (int i = 0; i < InvoiceItems.Count; i++)
            {
                if (InvoiceItems[i].ProductId == newItem.ProductId && InvoiceItems[i].Index == newItem.Index)
                {
                    existingIndex = i;
                }

                if (InvoiceItems[i].ProductId == newItem.ProductId && InvoiceItems[i].Index != newItem.Index)
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
    }
}

