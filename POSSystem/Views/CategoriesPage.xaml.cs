using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using System.Windows.Input;

namespace POSSystem.Views
{
    public sealed partial class CategoriesPage : Page
    {

        public CategoriesPage()
        {
            this.InitializeComponent();

            this.DataContext = new CategoryViewModel();
        }

        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog addCategoryDialog = new ContentDialog
            {
                Title = "NEW CATEGORY",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                XamlRoot = App.AppMainWindow.Content.XamlRoot
            };

            TextBox nameTextBox = new TextBox
            {
                PlaceholderText = "Category Name",
            };

            addCategoryDialog.Content = nameTextBox;

            var result = await addCategoryDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string newCategoryName = nameTextBox.Text.Trim();

                var viewModel = (CategoryViewModel)this.DataContext;
                viewModel.AddCategory(newCategoryName);
            }
        }

        public async void UpdateCategory_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var category = (Category)button.DataContext;

            ContentDialog updateCategoryDialog = new ContentDialog
            {
                Title = "UPDATE CATEGORY",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                XamlRoot = App.AppMainWindow.Content.XamlRoot
            };

            TextBox nameTextBox = new TextBox
            {
                PlaceholderText = "Category Name",
                Text = category.Name
            };

            updateCategoryDialog.Content = nameTextBox;

            var result = await updateCategoryDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string updatedCategoryName = nameTextBox.Text.Trim();

                var viewModel = (CategoryViewModel)this.DataContext;
                viewModel.UpdateCategory(category.Id, updatedCategoryName);
            }
        }

        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var category = (Category)button.DataContext;

            ContentDialog deleteCategoryDialog = new ContentDialog
            {
                Title = "DELETE CATEGORY",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                XamlRoot = App.AppMainWindow.Content.XamlRoot
            };

            TextBlock textBlock = new TextBlock
            {
                Text = "Are you sure you want to delete '" + category.Name + "' category?",
            };

            deleteCategoryDialog.Content = textBlock;

            var result = await deleteCategoryDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var viewModel = (CategoryViewModel)this.DataContext;
                viewModel.DeleteCategory(category.Id);
            }
        }
    }
}
