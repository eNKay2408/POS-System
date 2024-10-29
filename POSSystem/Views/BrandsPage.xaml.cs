using Microsoft.UI.Xaml.Controls;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using System.Net.WebSockets;

namespace POSSystem.Views
{
    public sealed partial class BrandsPage : Page
    {
        public BrandsPage()
        {
            this.InitializeComponent();
            this.DataContext = new BrandViewModel();
        }

        private async void AddBrand_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ContentDialog addBrandDialog = new ContentDialog
            {
                Title = "NEW BRAND",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                XamlRoot = App.AppMainWindow.Content.XamlRoot
            };

            TextBox nameTextBox = new TextBox
            {
                PlaceholderText = "Brand Name",
            };

            addBrandDialog.Content = nameTextBox;

            var result = await addBrandDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string newBrandName = nameTextBox.Text.Trim();

                var viewModel = (BrandViewModel)this.DataContext;
                viewModel.AddBrand(newBrandName);
            }
        }

        private async void UpdateBrand_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var button = (Button)sender;
            var brand = (Brand)button.DataContext;

            ContentDialog updateBrandDialog = new ContentDialog
            {
                Title = "UPDATE BRAND",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                XamlRoot = App.AppMainWindow.Content.XamlRoot
            };

            TextBox nameTextBox = new TextBox
            {
                PlaceholderText = "Brand Name",
                Text = brand.Name
            };

            updateBrandDialog.Content = nameTextBox;

            var result = await updateBrandDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string updatedBrandName = nameTextBox.Text.Trim();

                var viewModel = (BrandViewModel)this.DataContext;
                viewModel.UpdateBrand(brand.Id, updatedBrandName);
            }
        }

        private async void DeleteBrand_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var button = (Button)sender;
            var brand = (Brand)button.DataContext;

            ContentDialog deleteBrandDialog = new ContentDialog
            {
                Title = "DELETE BRAND",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                XamlRoot = App.AppMainWindow.Content.XamlRoot
            };

            TextBlock textBlock = new TextBlock
            {
                Text = "Are you sure you want to delete '" + brand.Name + "' brand?",
            };

            deleteBrandDialog.Content = textBlock;

            var result = await deleteBrandDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var viewModel = (BrandViewModel)this.DataContext;
                viewModel.DeleteBrand(brand.Id);
            }
        }
    }
}
