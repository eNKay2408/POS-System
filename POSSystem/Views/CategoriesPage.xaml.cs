using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;

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
                string newCategoryName = nameTextBox.Text;

                try
                {
                    var viewModel = (CategoryViewModel)this.DataContext;
                    await viewModel.AddCategory(newCategoryName);
                }
                catch (Exception ex)
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "ERROR",
                        Content = ex.Message,
                        CloseButtonText = "Ok",
                        XamlRoot = App.AppMainWindow.Content.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                }
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
                string updatedCategoryName = nameTextBox.Text;

                try
                {
                    var viewModel = (CategoryViewModel)this.DataContext;
                    await viewModel.UpdateCategory(category.Id, updatedCategoryName);
                }
                catch (Exception ex)
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "ERROR",
                        Content = ex.Message,
                        CloseButtonText = "Ok",
                        XamlRoot = App.AppMainWindow.Content.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                }
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
                await viewModel.DeleteCategory(category.Id);
            }
        }
    }
}
