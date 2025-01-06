using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using static POSSystem.Views.InvoicesPage;

namespace POSSystem.Views
{
    
    public sealed partial class InvoiceEditPage : Page
    {

        public InvoiceEditViewModel ViewModel { get; set; }
        public InvoiceEditPage()
        {
            ViewModel = InvoiceEditViewModel.Instance;
            this.DataContext = ViewModel;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.SaveInvoice();
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

        public static void AddInvoiceItem(InvoiceItem item)
        {
            InvoiceEditViewModel.Instance.AddItemToInvoice(item);

        }

        public void DeleteItemFromInvoice(InvoiceItem item)
        {
            ViewModel.DeleteItemFromInvoice(item);
        }

        public static async void UpdateInvoiceItem(InvoiceItem item)
        {
            try
            {
                InvoiceEditViewModel.Instance.UpdateInvoiceItem(item);
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Ok",
                    XamlRoot = App.AppMainWindow.Content.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }

        private async void Discard_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "Discard changes?",
                Content = "If you discard changes, they will be lost.",
                PrimaryButtonText = "Discard",
                CloseButtonText = "Cancel",
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Frame.GoBack();
            }
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var invoiceItem = (InvoiceItem)button.DataContext;

            var viewModel = (InvoiceEditViewModel)DataContext;
            await viewModel.DeleteItem(invoiceItem);
        }


        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InvoiceAddItemPage), ParentPageOrigin.InvoiceEditPage);
        }

        public void UpdateInvoiceItem_Click(InvoiceItem item)
        {
            Frame.Navigate(typeof(InvoiceAddItemPage), (item, ParentPageOrigin.InvoiceEditPage));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if(e.Parameter is Invoice invoice)
            {
                if (ViewModel.InvoiceId != invoice.Id)
                {
                    ViewModel.InvoiceId = invoice.Id;
                    await ViewModel.LoadDataFromDatabase();
                    ViewModel.SetSelectedEmployee(invoice.EmployeeId);
                }
                InitializeComponent();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            
            if(e.SourcePageType != typeof(InvoiceAddItemPage))
            {
                ViewModel.Clear();
            }
        }
    }
}
