using Microsoft.UI.Xaml.Controls;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Repository;
using POSSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        private readonly StripeService _stripeService;

        private List<Product> _products;
        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        private List<Category> _categories;
        public List<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    FilterProducts();
                }
            }
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    OnPropertyChanged();
                    FilterProducts();
                }
            }
        }

        private decimal? _maxPrice;
        public decimal? MaxPrice
        {
            get => _maxPrice;
            set
            {
                if (_maxPrice != value)
                {
                    _maxPrice = value;
                    OnPropertyChanged();
                    FilterProducts();
                }
            }
        }

        public ProductViewModel()
        {
            _productRepository = new ProductRepository(ConnectionString);
            _categoryRepository = new CategoryRepository(ConnectionString);
            _brandRepository = new BrandRepository(ConnectionString);

            _stripeService = new StripeService();

            Products = new List<Product>();
            Categories = new List<Category>();

            LoadData();
        }

        private async void LoadData()
        {
            await LoadProducts();
            LoadCategories();
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

        private async void LoadCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();

            Categories = categories;
        }

        public async void AddProduct(string name, decimal price, int stock, int categoryId, int brandId)
        {
            var product = new Product
            {
                Name = name,
                Price = price,
                Stock = stock,
                CategoryId = categoryId,
                BrandId = brandId
            };

            await _productRepository.AddProduct(product);

            await LoadProducts();
        }

        public async void UpdateProduct(int id, string newName, decimal newPrice, int newStock, int newCategoryId, int newBrandId)
        {
            var product = await _productRepository.GetProductById(id);

            product.Name = newName;
            product.Price = newPrice;
            product.Stock = newStock;
            product.CategoryId = newCategoryId;
            product.BrandId = newBrandId;

            await _productRepository.UpdateProduct(product);

            await LoadProducts();
        }

        public async void DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);

            await LoadProducts();
        }

        private async void FilterProducts()
        {
            await LoadProducts();
            var products = Products.ToList();

            if (!string.IsNullOrEmpty(FilterText))
            {
                products = Products.Where(p => p.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (SelectedCategory != null)
            {
                products = products.Where(p => p.CategoryId == SelectedCategory.Id).ToList();
            }

            if (MaxPrice != null)
            {
                products = products.Where(p => p.Price <= MaxPrice).ToList();
            }

            Products.Clear();

            foreach (var product in products)
            {
                Products.Add(product);
            }
        }

        public void SortByPrice()
        {
            if (Products.Count == 0)
            {
                return;
            }

            var orderedProducts = new List<Product>();

            if (Products[0].Price > Products[Products.Count - 1].Price)
            {
                orderedProducts = Products.OrderBy(p => p.Price).ToList();
            }
            else
            {
                orderedProducts = Products.OrderByDescending(p => p.Price).ToList();
            }

            Products.Clear();

            foreach (var product in orderedProducts)
            {
                Products.Add(product);
            }
        }

        public async void PayProduct(Product product, int quantity)
        {
            try
            {
                var checkoutUrl = await _stripeService.CreateCheckoutSession(product, quantity);

                var uri = new Uri(checkoutUrl);
                await Windows.System.Launcher.LaunchUriAsync(uri);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
