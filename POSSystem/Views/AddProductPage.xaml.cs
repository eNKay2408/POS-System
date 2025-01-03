using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;

namespace POSSystem.Views
{
    public sealed partial class AddProductPage : Page
    {
        public AddProductPage()
        {
            this.InitializeComponent();
            this.DataContext = new AddProductViewModel();
        }

        private void Discard_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var viewModel = (AddProductViewModel)this.DataContext;
                await viewModel.SaveProduct();

                Frame.GoBack();
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Ok",
                    XamlRoot = App.AppMainWindow.Content.XamlRoot
                };

                await errorDialog.ShowAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Product product)
            {
                var viewModel = (AddProductViewModel)this.DataContext;
                viewModel.Product = product;
            }
        }
    }
}
