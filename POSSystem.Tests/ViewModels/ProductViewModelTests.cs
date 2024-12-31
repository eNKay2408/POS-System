using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.ViewModels;
using POSSystem.Models;
using POSSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using POSSystem.Services;
using System;
namespace POSSystem.Tests.ViewModels
{
    [TestClass()]
    public class ProductViewModelTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IBrandRepository> _brandRepositoryMock;
        private Mock<IStripeService> _stripeServiceMock;
        private Mock<IUriLauncher> _uriLauncherMock;

        private ProductViewModel _productViewModel;

        private List<Product> _products;
        private Brand _brand;
        private Category _category;

        [TestInitialize]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _brandRepositoryMock = new Mock<IBrandRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _stripeServiceMock = new Mock<IStripeService>();
            _uriLauncherMock = new Mock<IUriLauncher>();

            _productViewModel = new ProductViewModel(_productRepositoryMock.Object, _categoryRepositoryMock.Object, _brandRepositoryMock.Object, _stripeServiceMock.Object, _uriLauncherMock.Object);

            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Product A", Price= 5.5m , Stock =10,  CategoryId = 1, BrandId = 1 },
                new Product { Id = 2, Name = "Product B", Price = 10.10m , Stock=20, CategoryId = 1, BrandId = 1 }
            };
            _brand = new Brand() { Id = 1, Name = "Brand A" };
            _category = new Category() { Id = 1, Name = "Category A" };
        }

        [TestMethod()]
        public async Task LoadProducts_ShouldLoadProductsIntoViewModel()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(_products);
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(It.IsAny<int>())).ReturnsAsync(_category);
            _brandRepositoryMock.Setup(repo => repo.GetBrandById(It.IsAny<int>())).ReturnsAsync(_brand);


            // Act
            await _productViewModel.LoadProducts();

            // Assert
            Assert.AreEqual(2, _productViewModel.Products.Count);
            Assert.AreEqual("Product A", _productViewModel.Products[0].Name);
            Assert.AreEqual("Category A", _productViewModel.Products[0].CategoryName);
            Assert.AreEqual("Brand A", _productViewModel.Products[0].BrandName);
            Assert.AreEqual(1, _productViewModel.Products[0].Index);
        }

        [TestMethod()]
        public async Task LoadCategories_ShouldLoadCategoriesIntoViewModel()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Name = "Category A" },
                new Category { Name = "Category B" }
            };
            _categoryRepositoryMock.Setup(repo => repo.GetAllCategories()).ReturnsAsync(categories);

            // Act
            await _productViewModel.LoadCategories();

            // Assert
            Assert.AreEqual(3, _productViewModel.Categories.Count);
            Assert.AreEqual("All", _productViewModel.Categories[0].Name);
            Assert.AreEqual("Category A", _productViewModel.Categories[1].Name);
        }

        [TestMethod()]
        public async Task AddProduct_ShouldAddProductAndReloadProducts()
        {
            // Arrange
            string name = "Product C";
            decimal price = 15.15m;
            int stock = 30;
            int categoryId = 1;
            int brandId = 1;

            _productRepositoryMock.Setup(repo => repo.AddProduct(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(() =>
            {
                var newProduct = new Product
                {
                    Name = name,
                    Price = price,
                    Stock = stock,
                    CategoryId = categoryId,
                    BrandId = brandId
                };
                _products.Add(newProduct);
                return _products;
            });
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(It.IsAny<int>())).ReturnsAsync(_category);
            _brandRepositoryMock.Setup(repo => repo.GetBrandById(It.IsAny<int>())).ReturnsAsync(_brand);

            // Act
            await _productViewModel.AddProduct(name, price, stock, categoryId, brandId);

            // Assert
            _productRepositoryMock.Verify(repo => repo.AddProduct(It.Is<Product>(p => p.Name == name && p.Price == price && p.Stock == stock && p.CategoryId == categoryId && p.BrandId == brandId)), Times.Once);
            Assert.AreEqual(3, _productViewModel.Products.Count);
            Assert.AreEqual(name, _productViewModel.Products[2].Name);
            Assert.AreEqual(price, _productViewModel.Products[2].Price);
            Assert.AreEqual(stock, _productViewModel.Products[2].Stock);
            Assert.AreEqual("Category A", _productViewModel.Products[2].CategoryName);
            Assert.AreEqual("Brand A", _productViewModel.Products[2].BrandName);
        }

        [TestMethod()]
        public async Task UpdateProduct_ShouldUpdateProductAndReloadProducts()
        {
            // Arrange
            int id = 1;
            string newName = "Product A Updated";
            decimal newPrice = 6.6m;
            int newStock = 12;
            int newCategoryId = 2;
            int newBrandId = 2;

            _productRepositoryMock.Setup(repo => repo.GetProductById(id)).ReturnsAsync(_products[0]);
            _productRepositoryMock.Setup(repo => repo.UpdateProduct(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(() =>
            {
                _products[0].Name = newName;
                _products[0].Price = newPrice;
                _products[0].Stock = newStock;
                _products[0].CategoryId = newCategoryId;
                _products[0].BrandId = newBrandId;
                return _products;
            });
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(It.IsAny<int>())).ReturnsAsync(new Category { Id = newCategoryId, Name = "Category B" });
            _brandRepositoryMock.Setup(repo => repo.GetBrandById(It.IsAny<int>())).ReturnsAsync(new Brand { Id = newBrandId, Name = "Brand B" });

            // Act
            await _productViewModel.UpdateProduct(id, newName, newPrice, newStock, newCategoryId, newBrandId);

            // Assert
            _productRepositoryMock.Verify(repo => repo.UpdateProduct(It.Is<Product>(p => p.Id == id && p.Name == newName && p.Price == newPrice && p.Stock == newStock && p.CategoryId == newCategoryId && p.BrandId == newBrandId)), Times.Once);
            Assert.AreEqual(2, _productViewModel.Products.Count);
            Assert.AreEqual(newName, _productViewModel.Products[0].Name);
            Assert.AreEqual(newPrice, _productViewModel.Products[0].Price);
            Assert.AreEqual(newStock, _productViewModel.Products[0].Stock);
            Assert.AreEqual("Category B", _productViewModel.Products[0].CategoryName);
            Assert.AreEqual("Brand B", _productViewModel.Products[0].BrandName);
        }

        [TestMethod()]
        public async Task DeleteProduct_ShouldDeleteProductAndReloadProducts()
        {
            // Arrange
            int id = 1;

            _productRepositoryMock.Setup(repo => repo.DeleteProduct(id)).Returns(Task.CompletedTask);
            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(() =>
            {
                _products.RemoveAt(0);
                return _products;
            });
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(It.IsAny<int>())).ReturnsAsync(_category);
            _brandRepositoryMock.Setup(repo => repo.GetBrandById(It.IsAny<int>())).ReturnsAsync(_brand);

            // Act
            await _productViewModel.DeleteProduct(id);

            // Assert
            _productRepositoryMock.Verify(repo => repo.DeleteProduct(id), Times.Once);
            Assert.AreEqual(1, _productViewModel.Products.Count);
            Assert.AreEqual("Product B", _productViewModel.Products[0].Name);
        }

        [TestMethod()]
        public async Task FilterProducts_ByNameProduct_ShouldFilterProductsByName()
        {
            var filterText = "A";

            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(_products);
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(It.IsAny<int>())).ReturnsAsync(_category);
            _brandRepositoryMock.Setup(repo => repo.GetBrandById(It.IsAny<int>())).ReturnsAsync(_brand);

            _productViewModel.FilterText = filterText;

            // Act
            await _productViewModel.FilterProducts();

            // Assert
            Assert.AreEqual(1, _productViewModel.Products.Count);
            Assert.AreEqual("Product A", _productViewModel.Products[0].Name);
        }

        [TestMethod()]
        public async Task FilterProducts_ByCategory_ShouldFilterProductsByCategory()
        {
            var selectedCategory = new Category { Id = 2, Name = "Category B" };


            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(_products);
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(It.IsAny<int>())).ReturnsAsync(_category);
            _brandRepositoryMock.Setup(repo => repo.GetBrandById(It.IsAny<int>())).ReturnsAsync(_brand);

            _productViewModel.SelectedCategory = selectedCategory;

            // Act
            await _productViewModel.FilterProducts();

            // Assert
            Assert.AreEqual(0, _productViewModel.Products.Count);
        }

        public async Task FilterProducts_ByMaxPrice_ShouldFilterProductsByMaxPrice()
        {
            var maxPrice = 6.0m;

            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(_products);
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(It.IsAny<int>())).ReturnsAsync(_category);
            _brandRepositoryMock.Setup(repo => repo.GetBrandById(It.IsAny<int>())).ReturnsAsync(_brand);

            _productViewModel.MaxPrice = maxPrice;

            // Act
            await _productViewModel.FilterProducts();

            // Assert
            Assert.AreEqual(1, _productViewModel.Products.Count);
            Assert.AreEqual("Product A", _productViewModel.Products[0].Name);
        }

        [TestMethod()]
        public async Task FilterProducts_ByProductNameCategoryAndMaxPrice_ShouldFilterProducts()
        {
            var filterText = "A";
            var maxPrice = 6.0m;
            var selectedCategory = new Category { Id = 1, Name = "Category A" };

            _products.Add(new Product { Id = 3, Name = "Product A New", Price = 7.0m, Stock = 14, CategoryId = 1, BrandId = 1 });
            _products.Add(new Product { Id = 4, Name = "Product A Super New", Price = 6.0m, Stock = 12, CategoryId = 2, BrandId = 2 });

            _productRepositoryMock.Setup(repo => repo.GetAllProducts()).ReturnsAsync(_products);
            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(It.IsAny<int>())).ReturnsAsync(_category);
            _brandRepositoryMock.Setup(repo => repo.GetBrandById(It.IsAny<int>())).ReturnsAsync(_brand);

            _productViewModel.FilterText = filterText;
            _productViewModel.SelectedCategory = selectedCategory;
            _productViewModel.MaxPrice = maxPrice;

            // Act
            await _productViewModel.FilterProducts();

            // Assert
            Assert.AreEqual(1, _productViewModel.Products.Count);
            Assert.AreEqual("Product A", _productViewModel.Products[0].Name);
        }

        [TestMethod()]
        public void SortByPrice_ShouldSortProductsByPriceDescending()
        {
            _productViewModel.Products = _products;

            // Act
            _productViewModel.SortByPrice();

            // Assert
            Assert.AreEqual("Product B", _productViewModel.Products[0].Name);
            Assert.AreEqual("Product A", _productViewModel.Products[1].Name);
        }

        [TestMethod()]
        public void SortByPrice_ShouldSortProductsByPriceAscending()
        {
            _products.Reverse();
            _productViewModel.Products = _products;

            // Act
            _productViewModel.SortByPrice();

            // Assert
            Assert.AreEqual("Product A", _productViewModel.Products[0].Name);
            Assert.AreEqual("Product B", _productViewModel.Products[1].Name);
        }
    }
}