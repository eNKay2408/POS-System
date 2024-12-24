using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;

namespace POSSystem.Views
{
    public sealed partial class InvoiceAddItemPage : Page
    {
        public InvoiceAddItemPage()
        {
            this.InitializeComponent();
            this.DataContext = new InvoiceAddItemViewModel();
        }

        private void Discard_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (InvoiceAddItemViewModel)DataContext;
            Frame.Navigate(typeof(InvoiceAddPage), viewModel.InvoiceItem.InvoiceId);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var viewModel = (InvoiceAddItemViewModel)DataContext;
                await viewModel.SaveInvoiceItem();

                Frame.Navigate(typeof(InvoiceAddPage), viewModel.InvoiceItem.InvoiceId);
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Error",
                    Content = ex.Message,
                    PrimaryButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                _ = dialog.ShowAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is InvoiceItem invoiceItem)
            {
                var viewModel = (InvoiceAddItemViewModel)DataContext;
                viewModel.InvoiceItem = invoiceItem;
            }
        }
    }
}
