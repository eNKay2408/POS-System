using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class AddProductViewModel : BaseViewModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

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

        private List<Brand> _brands;
        public List<Brand> Brands
        {
            get => _brands;
            set
            {
                _brands = value;
                OnPropertyChanged();
            }
        }

        private Product _product;
        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        private Brand _selectedBrand;
        public Brand SelectedBrand
        {
            get => _selectedBrand;
            set
            {
                _selectedBrand = value;
                OnPropertyChanged();
            }
        }

        public AddProductViewModel()
        {
            _productRepository = ServiceFactory.GetChildOf<IProductRepository>();
            _categoryRepository = ServiceFactory.GetChildOf<ICategoryRepository>();
            _brandRepository = ServiceFactory.GetChildOf<IBrandRepository>();

            Categories = new List<Category>();
            Brands = new List<Brand>();

            LoadData();
        }

        // Constructor for unit testing
        public AddProductViewModel(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        private async void LoadData()
        {
            await LoadCategories();
            await LoadBrands();

            LoadSelectedValues();
        }

        public void LoadSelectedValues()
        {
            if (Product.Id != 0)
            {
                SelectedCategory = Categories.Find(c => c.Id == Product.CategoryId);
                SelectedBrand = Brands.Find(b => b.Id == Product.BrandId);
            }
            else
            {
                SelectedCategory = null;
                SelectedBrand = null;
            }
        }

        public async Task LoadCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();

            Categories = categories;
        }

        public async Task LoadBrands()
        {
            var brands = await _brandRepository.GetAllBrands();

            Brands = brands;
        }

        public async Task SaveProduct()
        {
            if (string.IsNullOrWhiteSpace(Product.Name))
            {
                throw new ArgumentException("Product name is required and cannot only contain white spaces.");
            }

            if (Product.Price == null)
            {
                throw new ArgumentNullException("Product price is required and must be a decimal.");
            }

            if (Product.Price <= 0)
            {
                throw new ArgumentOutOfRangeException("Product price must be greater than 0.");
            }

            if (Product.Stock == null)
            {
                throw new ArgumentNullException("Product stock is required and must be an integer.");
            }

            if (Product.Stock <= 0)
            {
                throw new ArgumentOutOfRangeException("Product stock must be greater than 0.");
            }

            if (SelectedCategory == null)
            {
                throw new ArgumentNullException("Category is required.");
            }

            if (SelectedBrand == null)
            {
                throw new ArgumentNullException("Brand is required.");
            }

            Product.CategoryId = SelectedCategory.Id;
            Product.BrandId = SelectedBrand.Id;

            if (Product.Id == 0)
            {
                try
                {
                    await _productRepository.AddProduct(Product);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    await _productRepository.UpdateProduct(Product);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
