using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using System.Threading.Tasks;

namespace POSSystem.Views
{
    public sealed partial class InvoiceAddItemPage : Page
    {
        public delegate void InvoiceItemEventHandler(InvoiceItem invoiceItem);
        public static event InvoiceItemEventHandler AddInvoiceItemHanlder;

        public InvoiceAddItemViewModel ViewModel { get; set; }
        public InvoiceAddItemPage()
        { 
            ViewModel = new InvoiceAddItemViewModel();
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        private void Discard_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InvoiceAddPage), ViewModel.InvoiceItem.InvoiceId);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel.CurrentProduct.Id == 0)
                {
                    await DisplayErrorDialog("Please select a product.");
                    return;
                }

                if (!int.TryParse(ViewModel.InvoiceItem.Quantity.ToString(), out int quantity) || quantity <= 0)
                {
                    await DisplayErrorDialog("Please enter a valid quantity (positive integer).");
                    return;
                }

                if (quantity > ViewModel.CurrentProduct.Stock)
                {
                    await DisplayErrorDialog("Quantity exceeds stock.");
                    return;
                }

                ViewModel.InvoiceItem.ProductId = ViewModel.CurrentProduct.Id;
                ViewModel.InvoiceItem.UnitPrice = ViewModel.CurrentProduct.Price.Value;
                ViewModel.InvoiceItem.ProductName = ViewModel.CurrentProduct.Name;

                if (ViewModel.InvoiceItem.Id == 0)
                {
                    AddInvoiceItemHanlder?.Invoke(ViewModel.InvoiceItem);
                }

                //Frame.Navigate(typeof(InvoiceAddPage), ViewModel.InvoiceItem.InvoiceId);
                //Frame.Navigate(typeof(InvoiceAddPage), ViewModel.InvoiceItem);

                Frame.GoBack();
            }
            catch (Exception ex)
            {
                await DisplayErrorDialog(ex.Message);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is InvoiceItem invoiceItem)
            {
                ViewModel.InvoiceItem = invoiceItem;
            }
        }

        private async Task DisplayErrorDialog(string contentMessage)
        {
            ContentDialog errorDialog = new()
            {
                Title = "ERROR",
                Content = contentMessage,
                CloseButtonText = "Ok",
                XamlRoot = this.XamlRoot
            };

            await errorDialog.ShowAsync();
        }
    }
}
