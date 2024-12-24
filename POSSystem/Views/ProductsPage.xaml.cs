using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;

namespace POSSystem.Views
{
    public sealed partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            this.InitializeComponent();
            this.DataContext = new ProductViewModel();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddProductPage), new Product());
        }

        private void UpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var product = (Product)button.DataContext;

            Frame.Navigate(typeof(AddProductPage), product);
        }

        private async void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var product = (Product)button.DataContext;

            ContentDialog deleteProductDialog = new ContentDialog
            {
                Title = "DELETE PRODUCT",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                XamlRoot = App.AppMainWindow.Content.XamlRoot
            };

            TextBlock textBlock = new TextBlock
            {
                Text = "Are you sure you want to delete '" + product.Name + "' product?",
            };

            deleteProductDialog.Content = textBlock;

            var result = await deleteProductDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var viewModel = (ProductViewModel)this.DataContext;
                await viewModel.DeleteProduct(product.Id);
            }
        }

        private void SortByPrice_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ProductViewModel)this.DataContext;
            viewModel.SortByPrice();
        }

        private void DecimalTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = !string.IsNullOrWhiteSpace(args.NewText) && !decimal.TryParse(args.NewText, out _);
        }
    }
}
