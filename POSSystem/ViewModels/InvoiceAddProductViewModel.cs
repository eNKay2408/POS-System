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
    public class InvoiceAddProductViewModel: BaseViewModel
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private List<Product> _products;
        private Product _currentProduct;

        public List<Product> Products
        { 
            get=> _products;
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
            }
        }

        public InvoiceAddProductViewModel()
        {
            _invoiceRepository = new InvoiceRepository();
            _productRepository = new ProductRepository();
            _categoryRepository = new CategoryRepository();
            _brandRepository = new BrandRepository();

            _products = new List<Product>();
            _currentProduct = new Product();

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

        public async Task LoadData()
        {
            await LoadProducts();
            CurrentProduct = Products.FirstOrDefault();
            Console.WriteLine("CurrentProduct: " + CurrentProduct.Name);
        }

        public async Task LoadProducts()
        {
            Products = await _productRepository.GetAllProducts();
            Category category= new Category();
            Brand brand = new Brand();
            //TODO: delegate heavy-work to db?
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


    }
}
