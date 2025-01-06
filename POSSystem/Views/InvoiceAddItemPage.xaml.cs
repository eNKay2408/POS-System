using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using System.Threading.Tasks;
using static POSSystem.Views.InvoicesPage;

namespace POSSystem.Views
{
    public sealed partial class InvoiceAddItemPage : Page
    {
        public delegate void InvoiceItemEventHandler(InvoiceItem invoiceItem, ParentPageOrigin origin);
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
                    UpdateInvoiceItemHanlder?.Invoke(ViewModel.InvoiceItem, _parentPageOrigin);
                }
                else
                {
                    AddInvoiceItemHanlder?.Invoke(ViewModel.InvoiceItem, _parentPageOrigin);
                }

                Frame.GoBack();


                //Frame.GoBack();
            }
            catch (Exception ex)
            {
                await DisplayErrorDialog(ex.Message);
            }
        }

        private bool _toUpdateInvoiceItem = false;
        private ParentPageOrigin _parentPageOrigin;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ValueTuple<InvoiceItem, ParentPageOrigin> parameters)
            {
                _toUpdateInvoiceItem = true;

                // To avoid direct binding to the item because the user may
                // try to update to an already existed item in the list
                // In that case, we want to notify to the user and cancel the operation
                // If direct binding, the object is already altered by the user
                // when they confirm the update-operation

                if(ViewModel.InvoiceItem == null)
                {
                    ViewModel.InvoiceItem = new InvoiceItem();
                }
                ViewModel.InvoiceItem.ProductId = parameters.Item1.ProductId;
                ViewModel.InvoiceItem.Quantity = parameters.Item1.Quantity;
                ViewModel.InvoiceItem.UnitPrice = parameters.Item1.UnitPrice;
                ViewModel.InvoiceItem.ProductName = parameters.Item1.ProductName;
                ViewModel.InvoiceItem.InvoiceId = parameters.Item1.InvoiceId;
                ViewModel.InvoiceItem.Id = parameters.Item1.Id;
                ViewModel.InvoiceItem.Index = parameters.Item1.Index;

                _parentPageOrigin = parameters.Item2;
            }
            else if(e.Parameter is ParentPageOrigin origin)
            {
                _parentPageOrigin = origin;
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
