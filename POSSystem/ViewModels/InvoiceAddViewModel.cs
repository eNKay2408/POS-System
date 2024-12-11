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
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        private List<Employee> _employees;
        private List<Product> _products;

        public List<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public InvoiceAddViewModel()
        {
            _employeeRepository = new EmployeeRepository();
            _productRepository = new ProductRepository();
            _categoryRepository = new CategoryRepository();
            _brandRepository = new BrandRepository();

            Employees = new List<Employee>();
            Products = new List<Product>();
            LoadData();
        }

        private async void LoadData()
        {
            await LoadEmployees();
            await LoadProducts();
        }

        private async Task LoadEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            Employees = employees;
        }

        private async Task LoadProducts()
        {
            var products = await _productRepository.GetAllProducts();

            foreach (var product in products)
            {
                var category = await _categoryRepository.GetCategoryById(product.CategoryId);
                product.CategoryName = category.Name;

                var brand = await _brandRepository.GetBrandById(product.BrandId);
                product.BrandName = brand.Name;

                product.Index = products.IndexOf(product) + 1;
            }

            Products = products;
        }
    }
}
