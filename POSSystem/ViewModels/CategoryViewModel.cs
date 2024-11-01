using CommunityToolkit.Mvvm.Input;
using POSSystem.Models;
using POSSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Windows.Input;

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
            _categoryRepository = new CategoryRepository(ConnectionString);

            Categories = new List<Category>();

            LoadCategories();
        }

        private async void LoadCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();

            foreach (var category in categories)
            {
                category.Index = categories.IndexOf(category) + 1;
            }
                
            Categories = categories;
        }

        public async void AddCategory(string categoryName)
        {
            var category = new Category
            {
                Name = categoryName
            };

            await _categoryRepository.AddCategory(category);

            LoadCategories();
        }

        public async void UpdateCategory(int categoryId, string newName)
        {
            var category = await _categoryRepository.GetCategoryById(categoryId);

            category.Name = newName;

            await _categoryRepository.UpdateCategory(category);

            LoadCategories();
        }

        public async void DeleteCategory(int categoryId)
        {
            await _categoryRepository.DeleteCategory(categoryId);

            LoadCategories();
        }
    }
}
