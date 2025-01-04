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
        public static event InvoiceItemEventHandler UpdateInvoiceItemHanlder;

        public InvoiceAddItemViewModel ViewModel { get; set; }
        public InvoiceAddItemPage()
        { 
            ViewModel = new InvoiceAddItemViewModel();
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        private void Discard_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(InvoiceAddPage), ViewModel.InvoiceItem.InvoiceId);
            Frame.GoBack();
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
                
                if (_toUpdateInvoiceItem)
                {
                    UpdateInvoiceItemHanlder?.Invoke(ViewModel.InvoiceItem);
                }
                else
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

        private bool _toUpdateInvoiceItem = false;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is InvoiceItem item)
            {
                _toUpdateInvoiceItem = true;

                // to avoid direct binding to the item

                if(ViewModel.InvoiceItem == null)
                {
                    ViewModel.InvoiceItem = new InvoiceItem();
                }
                ViewModel.InvoiceItem.ProductId = item.ProductId;
                ViewModel.InvoiceItem.Quantity = item.Quantity;
                ViewModel.InvoiceItem.UnitPrice = item.UnitPrice;
                ViewModel.InvoiceItem.ProductName = item.ProductName;
                ViewModel.InvoiceItem.InvoiceId = item.InvoiceId;
                ViewModel.InvoiceItem.Id = item.Id;
                ViewModel.InvoiceItem.Index = item.Index;

                ViewModel.LoadSelectedProduct();
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
