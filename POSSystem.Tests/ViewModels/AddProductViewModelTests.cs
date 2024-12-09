using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Repository;
using POSSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.Tests.ViewModels
{
    [TestClass()]
    public class AddProductViewModelTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IBrandRepository> _brandRepositoryMock;

        private AddProductViewModel _addProductViewModel;

        [TestInitialize]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _brandRepositoryMock = new Mock<IBrandRepository>();

            _addProductViewModel = new AddProductViewModel(_productRepositoryMock.Object, _categoryRepositoryMock.Object, _brandRepositoryMock.Object);
        }

        [TestMethod()]
        public async Task LoadCategories_ShouldLoadCategoriesIntoViewModel()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category A" },
                new Category { Id = 2, Name = "Category B" }
            };

            _categoryRepositoryMock.Setup(x => x.GetAllCategories()).ReturnsAsync(categories);

            // Act
            await _addProductViewModel.LoadCategories();

            // Assert
            Assert.AreEqual(categories, _addProductViewModel.Categories);
        }

        [TestMethod()]
        public async Task LoadBrands_ShouldLoadBrandsIntoViewModel()
        {
            // Arrange
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Brand A" },
                new Brand { Id = 2, Name = "Brand B" }
            };

            _brandRepositoryMock.Setup(x => x.GetAllBrands()).ReturnsAsync(brands);

            // Act
            await _addProductViewModel.LoadBrands();

            // Assert
            Assert.AreEqual(brands, _addProductViewModel.Brands);
        }

        [TestMethod()]
        public void LoadSelectedValues_WhenProductIsNew_ShouldSetSelectedCategoryAndBrandToNull()
        {
            // Arrange
            _addProductViewModel.Product = new Product();

            // Act
            _addProductViewModel.LoadSelectedValues();

            // Assert
            Assert.IsNull(_addProductViewModel.SelectedCategory);
            Assert.IsNull(_addProductViewModel.SelectedBrand);
        }

        [TestMethod()]
        public void LoadSelectedValues_WhenProductIsExisting_ShouldSetSelectedCategoryAndBrand()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                CategoryId = 1,
                BrandId = 1
            };

            _addProductViewModel.Product = product;

            _addProductViewModel.Categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category A" },
                new Category { Id = 2, Name = "Category B" }
            };

            _addProductViewModel.Brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Brand A" },
                new Brand { Id = 2, Name = "Brand B" }
            };

            // Act
            _addProductViewModel.LoadSelectedValues();

            // Assert
            Assert.AreEqual(_addProductViewModel.Categories[0], _addProductViewModel.SelectedCategory);
            Assert.AreEqual(_addProductViewModel.Brands[0], _addProductViewModel.SelectedBrand);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenProductIsNew_ShouldAddProduct()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product A",
                Price = 5.5m,
                Stock = 10,
            };

            _addProductViewModel.Product = product;
            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };
            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };

            // Act
            await _addProductViewModel.SaveProduct();

            // Assert
            _productRepositoryMock.Verify(x => x.AddProduct(product), Times.Once);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenProductIsExisting_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product A",
                Price = 5.5m,
                Stock = 10,
            };

            _addProductViewModel.Product = product;
            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };
            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };

            // Act
            await _addProductViewModel.SaveProduct();

            // Assert
            _productRepositoryMock.Verify(x => x.UpdateProduct(product), Times.Once);
        }

        [TestMethod()]
        public async Task SaveProduct_ShouldSetProductCategoryIdAndBrandId()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product A",
                Price = 5.5m,
                Stock = 10,
            };

            _addProductViewModel.Product = product;
            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };
            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };

            // Act
            await _addProductViewModel.SaveProduct();

            // Assert
            Assert.AreEqual(1, product.CategoryId);
            Assert.AreEqual(1, product.BrandId);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenNameIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var product = new Product
            {
                Name = "",
                Price = 5.5m,
                Stock = 10,
            };

            _addProductViewModel.Product = product;

            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };
            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _addProductViewModel.SaveProduct());
            Assert.AreEqual("Product name is required and cannot only contain white spaces.", ex.Message);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenNameAllWhiteSpaces_ShouldThrowArgumentException()
        {
            // Arrange
            var product = new Product
            {
                Name = "   ",
                Price = 5.5m,
                Stock = 10,
            };

            _addProductViewModel.Product = product;

            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };
            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _addProductViewModel.SaveProduct());
            Assert.AreEqual("Product name is required and cannot only contain white spaces.", ex.Message);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenPriceIsEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product A",
                Price = null,
                Stock = 10,
            };

            _addProductViewModel.Product = product;

            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };
            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _addProductViewModel.SaveProduct());
            Assert.AreEqual("Value cannot be null. (Parameter 'Product price is required and must be a decimal.')", ex.Message);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenPriceIsLessThanZero_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product A",
                Price = -5.5m,
                Stock = 10,
            };

            _addProductViewModel.Product = product;

            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };
            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => _addProductViewModel.SaveProduct());
            Assert.AreEqual("Specified argument was out of the range of valid values. (Parameter 'Product price must be greater than 0.')", ex.Message);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenStockIsEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product A",
                Price = 5.5m,
                Stock = null,
            };

            _addProductViewModel.Product = product;

            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };
            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _addProductViewModel.SaveProduct());
            Assert.AreEqual("Value cannot be null. (Parameter 'Product stock is required and must be an integer.')", ex.Message);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenStockIsLessThanZero_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product A",
                Price = 5.5m,
                Stock = -10,
            };

            _addProductViewModel.Product = product;

            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };
            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => _addProductViewModel.SaveProduct());
            Assert.AreEqual("Specified argument was out of the range of valid values. (Parameter 'Product stock must be greater than 0.')", ex.Message);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenCategoryIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product A",
                Price = 5.5m,
                Stock = 10,
            };

            _addProductViewModel.Product = product;

            _addProductViewModel.SelectedBrand = new Brand { Id = 1, Name = "Brand A" };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _addProductViewModel.SaveProduct());
            Assert.AreEqual("Value cannot be null. (Parameter 'Category is required.')", ex.Message);
        }

        [TestMethod()]
        public async Task SaveProduct_WhenBrandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var product = new Product
            {
                Name = "Product A",
                Price = 5.5m,
                Stock = 10,
            };

            _addProductViewModel.Product = product;

            _addProductViewModel.SelectedCategory = new Category { Id = 1, Name = "Category A" };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _addProductViewModel.SaveProduct());
            Assert.AreEqual("Value cannot be null. (Parameter 'Brand is required.')", ex.Message);
        }
    }
}
