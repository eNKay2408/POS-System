using Microsoft.UI.Xaml.Media;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            _productRepository = new ProductRepository(ConnectionString);
            _categoryRepository = new CategoryRepository(ConnectionString);
            _brandRepository = new BrandRepository(ConnectionString);

            Categories = new List<Category>();
            Brands = new List<Brand>();

            LoadData();
        }
        
        private async void LoadData()
        {
            await LoadCategories();
            await LoadBrands();

            LoadSelectedValues();
        }

        private void LoadSelectedValues()
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

        private async Task LoadCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();

            Categories = categories;
        }

        private async Task LoadBrands()
        {
            var brands = await _brandRepository.GetAllBrands();

            Brands = brands;
        }

        public async Task SaveProduct()
        {
            Product.CategoryId = SelectedCategory.Id;
            Product.BrandId = SelectedBrand.Id;

            if (Product.Id == 0)
            {
                await _productRepository.AddProduct(Product);
            }
            else
            {
                await _productRepository.UpdateProduct(Product);
            }
        }
    }
}
