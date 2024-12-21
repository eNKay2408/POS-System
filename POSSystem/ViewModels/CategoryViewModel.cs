using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
        private readonly ICategoryRepository _categoryRepository;

        private List<Category> _categories;
        public List<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public CategoryViewModel()
        {
            _categoryRepository = ServiceFactory.GetChildOf<ICategoryRepository>();

            Categories = new List<Category>();

            _ = LoadCategories();
        }

        // Constructor for unit testing
        public CategoryViewModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task LoadCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();

            foreach (var category in categories)
            {
                category.Index = categories.IndexOf(category) + 1;
            }

            Categories = categories;
        }

        public async Task AddCategory(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            try
            {
                var category = new Category
                {
                    Name = categoryName
                };

                await _categoryRepository.AddCategory(category);

                await LoadCategories();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCategory(int categoryId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }

            var category = await _categoryRepository.GetCategoryById(categoryId);

            category.Name = newName;

            await _categoryRepository.UpdateCategory(category);

            await LoadCategories();
        }

        public async Task DeleteCategory(int categoryId)
        {
            await _categoryRepository.DeleteCategory(categoryId);

            await LoadCategories();
        }
    }
}
