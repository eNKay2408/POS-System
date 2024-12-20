using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Repository;
using POSSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        private readonly IStripeService _stripeService;
        private readonly IUriLauncher _uriLauncher;

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
                    _ = FilterProducts();
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
                    _ = FilterProducts();
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
                    _ = FilterProducts();
                }
            }
        }

        public ProductViewModel()
        {
            _productRepository = new ProductRepository();
            _categoryRepository = new CategoryRepository();
            _brandRepository = ServiceFactory.GetChildOf<IBrandRepository>();

            _stripeService = new StripeService();
            _uriLauncher = new UriLauncher();

            Products = new List<Product>();
            Categories = new List<Category>();

            LoadData();
        }

        // Constructor for unit testing
        public ProductViewModel(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IStripeService stripeService, IUriLauncher uriLauncher)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;

            _stripeService = stripeService;
            _uriLauncher = uriLauncher;
        }

        public async void LoadData()
        {
            await LoadProducts();
            await LoadCategories();
        }

        public async Task LoadProducts()
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

        public async Task LoadCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();

            categories.Insert(0, new Category { Id = 0, Name = "All" });

            Categories = categories;
        }

        public async Task AddProduct(string name, decimal price, int stock, int categoryId, int brandId)
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

        public async Task UpdateProduct(int id, string newName, decimal newPrice, int newStock, int newCategoryId, int newBrandId)
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

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);

            await LoadProducts();
        }

        public async Task FilterProducts()
        {
            await LoadProducts();

            var filteredProducts = Products.ToList();

            if (!string.IsNullOrEmpty(FilterText))
            {
                filteredProducts = filteredProducts.Where(p => p.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (SelectedCategory != null && SelectedCategory.Id != 0)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryId == SelectedCategory.Id).ToList();
            }

            if (MaxPrice != null && MaxPrice != 0)
            {
                filteredProducts = filteredProducts.Where(p => p.Price <= MaxPrice).ToList();
            }

            Products = filteredProducts;
        }

        public void SortByPrice()
        {
            var sortedProducts = Products.ToList();

            if (Products[0].Price > Products[Products.Count - 1].Price)
            {
                sortedProducts = sortedProducts.OrderBy(p => p.Price).ToList();
            }
            else
            {
                sortedProducts = sortedProducts.OrderByDescending(p => p.Price).ToList();
            }

            Products = sortedProducts;
        }

        public async Task PayProduct(Product product, int quantity)
        {
            try
            {
                var checkoutUrl = await _stripeService.CreateCheckoutSession(product, quantity);

                var uri = new Uri(checkoutUrl);

                await _uriLauncher.LaunchUriAsync(uri);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
