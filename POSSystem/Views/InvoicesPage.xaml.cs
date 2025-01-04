using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POSSystem.Models;
using POSSystem.ViewModels;
using POSSystem.Views.UserCtrl;
using System;

namespace POSSystem.Views
{
    public sealed partial class InvoicesPage : Page
    {
        private InvoiceViewModel ViewModel { get; set; }
        public InvoicesPage()
        {
            this.InitializeComponent();
            ViewModel = new InvoiceViewModel();
            this.DataContext = ViewModel;
        }

        private void AddInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InvoiceAddPage), 0);
        }

        private async void PayInvoice_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var invoice = (Invoice)button.DataContext;
            var viewModel = (InvoiceViewModel)this.DataContext;

            try
            {
                await viewModel.PayInvoice(invoice);
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
                await dialog.ShowAsync();
            }
        }

        private async void PrintInvoice_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "Print invoice clicked",
                PrimaryButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private void UpdateInvoice_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var invoice = (Invoice)button.DataContext;
            Frame.Navigate(typeof(InvoiceEditPage), invoice);
        }
    }
}
