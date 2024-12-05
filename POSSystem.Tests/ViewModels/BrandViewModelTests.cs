using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Repositories;
using System.Threading.Tasks;
using POSSystem.Models;
using System.Collections.Generic;
using POSSystem.ViewModels;
using System;

namespace POSSystem.Tests.ViewModels
{
    [TestClass()]
    public class BrandViewModelTests
    {
        private Mock<IBrandRepository> _brandRepositoryMock;
        private BrandViewModel _brandViewModel;
        private List<Brand> _brands;

        [TestInitialize]
        public void Setup()
        {
            _brandRepositoryMock = new Mock<IBrandRepository>();
            _brandViewModel = new BrandViewModel(_brandRepositoryMock.Object);

            _brands = new List<Brand>()
            {
                new Brand { Id = 1, Name = "Brand A" },
                new Brand { Id = 2, Name = "Brand B" }
            };
        }

        [TestMethod()]
        public async Task LoadBrands_ShouldLoadBrandsIntoViewModel()
        {
            // Arrange
            _brandRepositoryMock.Setup(repo => repo.GetAllBrands()).ReturnsAsync(_brands);

            // Act
            await _brandViewModel.LoadBrands();

            // Assert
            Assert.AreEqual(2, _brandViewModel.Brands.Count);
            Assert.AreEqual("Brand A", _brandViewModel.Brands[0].Name);
            Assert.AreEqual(1, _brandViewModel.Brands[0].Index);
        }

        [TestMethod()]
        public async Task AddBrand_ShouldAddBrandAndReloadBrands()
        {
            // Arrange
            var newBrandName = "Brand C";

            _brandRepositoryMock.Setup(repo => repo.AddBrand(It.IsAny<Brand>())).Returns(Task.CompletedTask);
            _brandRepositoryMock.Setup(repo => repo.GetAllBrands()).ReturnsAsync(() =>
            {
                _brands.Add(new Brand { Name = newBrandName });
                return _brands;
            });

            // Act
            await _brandViewModel.AddBrand(newBrandName);

            // Assert
            _brandRepositoryMock.Verify(repo => repo.AddBrand(It.Is<Brand>(b => b.Name == newBrandName)), Times.Once);
            Assert.AreEqual(3, _brandViewModel.Brands.Count);
            Assert.AreEqual(newBrandName, _brandViewModel.Brands[2].Name);
        }

        [TestMethod()]
        public async Task AddBrand_WhenNameIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var newBrandName = string.Empty;

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _brandViewModel.AddBrand(newBrandName));
            Assert.AreEqual("Brand name cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task AddBrand_WhenNameWhiteAllSpaces_ShouldThrowArgumentException()
        {
            // Arrange
            var newBrandName = "   ";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _brandViewModel.AddBrand(newBrandName));
            Assert.AreEqual("Brand name cannot be empty.", ex.Message);
        }


        [TestMethod()]
        public async Task UpdateBrand_ShouldUpdateBrandAndReloadBrands()
        {
            // Arrange
            int brandId = 1;
            string updatedBrandName = "Brand A Updated";

            _brandRepositoryMock.Setup(repo => repo.GetBrandById(brandId)).ReturnsAsync(new Brand { Id = brandId, Name = "Brand A" });
            _brandRepositoryMock.Setup(repo => repo.UpdateBrand(It.IsAny<Brand>())).Returns(Task.CompletedTask);
            _brandRepositoryMock.Setup(repo => repo.GetAllBrands()).ReturnsAsync(() =>
            {
                _brands[0].Name = updatedBrandName;
                return _brands;
            });

            // Act
            await _brandViewModel.UpdateBrand(brandId, updatedBrandName);

            // Assert
            _brandRepositoryMock.Verify(repo => repo.UpdateBrand(It.Is<Brand>(b => b.Id == brandId && b.Name == updatedBrandName)), Times.Once);
            Assert.AreEqual(2, _brandViewModel.Brands.Count);
            Assert.AreEqual(updatedBrandName, _brandViewModel.Brands[0].Name);
        }

        [TestMethod()]
        public async Task UpdateBrand_WhenNameIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            int brandId = 1;
            string updatedBrandName = string.Empty;

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _brandViewModel.UpdateBrand(brandId, updatedBrandName));
            Assert.AreEqual("Brand name cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateBrand_WhenNameAllWhiteSpaces_ShouldThrowArgumentException()
        {
            // Arrange
            int brandId = 1;
            string updatedBrandName = "   ";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _brandViewModel.UpdateBrand(brandId, updatedBrandName));
            Assert.AreEqual("Brand name cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task DeleteBrand_ShouldDeleteBrandAndReloadBrands()
        {
            // Arrange
            int brandId = 1;

            _brandRepositoryMock.Setup(repo => repo.DeleteBrand(brandId)).Returns(Task.CompletedTask);
            _brandRepositoryMock.Setup(repo => repo.GetAllBrands()).ReturnsAsync(() =>
            {
                _brands.RemoveAt(0);
                return _brands;
            });

            // Act
            await _brandViewModel.DeleteBrand(brandId);

            // Assert
            _brandRepositoryMock.Verify(repo => repo.DeleteBrand(brandId), Times.Once);
            Assert.AreEqual(1, _brandViewModel.Brands.Count);
            Assert.AreEqual("Brand B", _brandViewModel.Brands[0].Name);
        }
    }
}