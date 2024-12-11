using Microsoft.UI.Xaml.Controls;
using POSSystem.ViewModels;
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

        private async void AddInvoiceBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InvoiceAddPage));
        }

        private async void DeleteInvoice_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "delete invoice clicked",
                PrimaryButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void UpdateInvoice_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "update invoice clicked",
                PrimaryButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();

        }
    }
}
