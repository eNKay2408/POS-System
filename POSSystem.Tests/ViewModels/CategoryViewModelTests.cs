using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using POSSystem.Repositories;
using POSSystem.ViewModels;
using System.Collections.Generic;
using POSSystem.Models;
using System.Threading.Tasks;
using System;

namespace POSSystem.Tests.ViewModels
{
    [TestClass()]
    public class CategoryViewModelTests
    {
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private CategoryViewModel _categoryViewModel;
        private List<Category> _categories;

        [TestInitialize]
        public void Setup()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryViewModel = new CategoryViewModel(_categoryRepositoryMock.Object);

            _categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category A" },
                new Category { Id = 2, Name = "Category B" }
            };
        }

        [TestMethod()]
        public async Task LoadCategories_ShouldLoadCategoriesIntoViewModel()
        {
            // Arrange
            _categoryRepositoryMock.Setup(repo => repo.GetAllCategories()).ReturnsAsync(_categories);

            // Act
            await _categoryViewModel.LoadCategories();

            // Assert
            Assert.AreEqual(2, _categoryViewModel.Categories.Count);
            Assert.AreEqual("Category A", _categoryViewModel.Categories[0].Name);
            Assert.AreEqual(1, _categoryViewModel.Categories[0].Index);
        }

        [TestMethod()]
        public async Task AddCategory_ShouldAddCategoryAndReloadCategories()
        {
            // Arrange
            var newCategoryName = "Category C";

            _categoryRepositoryMock.Setup(repo => repo.AddCategory(It.IsAny<Category>())).Returns(Task.CompletedTask);
            _categoryRepositoryMock.Setup(repo => repo.GetAllCategories()).ReturnsAsync(() =>
            {
                _categories.Add(new Category { Name = newCategoryName });
                return _categories;
            });

            // Act
            await _categoryViewModel.AddCategory(newCategoryName);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.AddCategory(It.Is<Category>(c => c.Name == newCategoryName)), Times.Once);
            Assert.AreEqual(3, _categoryViewModel.Categories.Count);
            Assert.AreEqual(newCategoryName, _categoryViewModel.Categories[2].Name);
        }

        [TestMethod()]
        public async Task AddCategory_WhenNameIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var newCategoryName = "";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _categoryViewModel.AddCategory(newCategoryName));
            Assert.AreEqual("Category name cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task AddCategory_WhenNameAllWhiteSpaces_ShouldThrowArgumentException()
        {
            // Arrange
            var categoryId = 1;
            var updatedCategoryName = "   ";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _categoryViewModel.UpdateCategory(categoryId, updatedCategoryName));
            Assert.AreEqual("Category name cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateCategory_ShouldUpdateCategoryAndReloadCategories()
        {
            // Arrange
            var categoryId = 1;
            var updatedCategoryName = "Category A Updated";

            _categoryRepositoryMock.Setup(repo => repo.GetCategoryById(categoryId)).ReturnsAsync(new Category { Id = categoryId, Name = "Category A" });
            _categoryRepositoryMock.Setup(repo => repo.UpdateCategory(It.IsAny<Category>())).Returns(Task.CompletedTask);
            _categoryRepositoryMock.Setup(repo => repo.GetAllCategories()).ReturnsAsync(() =>
            {
                _categories[0].Name = updatedCategoryName;
                return _categories;
            });

            // Act
            await _categoryViewModel.UpdateCategory(categoryId, updatedCategoryName);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.UpdateCategory(It.Is<Category>(c => c.Id == categoryId && c.Name == updatedCategoryName)), Times.Once);
            Assert.AreEqual(2, _categoryViewModel.Categories.Count);
            Assert.AreEqual(updatedCategoryName, _categoryViewModel.Categories[0].Name);
        }

        [TestMethod()]
        public async Task UpdateCategory_WhenNameIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var categoryId = 1;
            var updatedCategoryName = "";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _categoryViewModel.UpdateCategory(categoryId, updatedCategoryName));
            Assert.AreEqual("Category name cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task UpdateCategory_WhenNameAllWhiteSpaces_ShouldThrowArgumentException()
        {
            // Arrange
            var categoryId = 1;
            var updatedCategoryName = "   ";

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _categoryViewModel.UpdateCategory(categoryId, updatedCategoryName));
            Assert.AreEqual("Category name cannot be empty.", ex.Message);
        }

        [TestMethod()]
        public async Task DeleteCategory_ShouldDeleteCategoryAndReloadCategories()
        {
            var categoryId = 1;

            _categoryRepositoryMock.Setup(repo => repo.DeleteCategory(categoryId)).Returns(Task.CompletedTask);
            _categoryRepositoryMock.Setup(repo => repo.GetAllCategories()).ReturnsAsync(() =>
            {
                _categories.RemoveAt(0);
                return _categories;
            });

            // Act
            await _categoryViewModel.DeleteCategory(categoryId);

            // Assert
            _categoryRepositoryMock.Verify(repo => repo.DeleteCategory(categoryId), Times.Once);
            Assert.AreEqual(1, _categoryViewModel.Categories.Count);
            Assert.AreEqual("Category B", _categoryViewModel.Categories[0].Name);
        }
    }
}
