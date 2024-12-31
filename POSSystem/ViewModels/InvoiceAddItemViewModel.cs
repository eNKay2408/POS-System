using Microsoft.UI.Xaml.Controls;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using POSSystem.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class InvoiceAddItemViewModel : BaseViewModel
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;

        private List<Product> _products;
        private Product _currentProduct;
        private InvoiceItem _invoiceItem;

        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public Product CurrentProduct
        {
            get => _currentProduct;
            set
            {
                _currentProduct = value;
                OnPropertyChanged();
            }
        }

        public InvoiceItem InvoiceItem
        {
            get => _invoiceItem;
            set
            {
                _invoiceItem = value;
                OnPropertyChanged();
            }
        }

        public InvoiceAddItemViewModel()
        {
            _invoiceRepository = ServiceFactory.GetChildOf<IInvoiceRepository>();
            _productRepository = ServiceFactory.GetChildOf<IProductRepository>();
            _categoryRepository = ServiceFactory.GetChildOf<ICategoryRepository>();
            _brandRepository = ServiceFactory.GetChildOf<IBrandRepository>();
            _invoiceItemRepository = ServiceFactory.GetChildOf<IInvoiceItemRepository>();

            InvoiceItem = new InvoiceItem();
            Products = new List<Product>();
            CurrentProduct = new Product();

            try
            {
                _ = LoadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // Constructor for unit testing
        public InvoiceAddItemViewModel(IInvoiceRepository invoiceRepository, IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IInvoiceItemRepository invoiceItemRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _invoiceItemRepository = invoiceItemRepository;
        }

        public async Task LoadData()
        {
            await LoadProducts();
            LoadSelectedProduct();
        }

        public async Task LoadProducts()
        {
            Products = await _productRepository.GetAllProducts();
            Category category = new Category();
            Brand brand = new Brand();
            int index = 1;
            foreach (var product in Products)
            {
                category = await _categoryRepository.GetCategoryById(product.CategoryId);
                product.CategoryName = category.Name;

                brand = await _brandRepository.GetBrandById(product.BrandId);
                product.BrandName = brand.Name;

                product.Index = index++;
            }
        }

        public void LoadSelectedProduct()
        {
            if (InvoiceItem.ProductId != 0)
            {
                CurrentProduct = Products.Find(p => p.Id == InvoiceItem.ProductId);
            }
        }

        public async Task SaveInvoiceItem()
        {
            if (CurrentProduct.Id == 0)
            {
                throw new Exception("Please select a product.");
            }

            if (InvoiceItem.Quantity == 0)
            {
                throw new Exception("Please enter a quantity.");
            }

            if (InvoiceItem.Quantity > CurrentProduct.Stock)
            {
                throw new Exception("Quantity exceeds stock.");
            }

            InvoiceItem.ProductId = CurrentProduct.Id;
            InvoiceItem.UnitPrice = CurrentProduct.Price.Value;

            try
            {
                if (InvoiceItem.Id == 0)
                {
                    await _invoiceItemRepository.AddInvoiceItem(InvoiceItem);
                }
                else
                {
                    await _invoiceItemRepository.UpdateInvoiceItem(InvoiceItem);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
