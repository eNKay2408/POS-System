using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using System.Runtime.CompilerServices;

namespace POSSystem.Views
{
    
    public sealed partial class InvoiceEditPage : Page
    {
        public Invoice Invoice;

        public InvoiceEditViewModel ViewModel { get; set; }
        public InvoiceEditPage()
        {
            InitializeComponent();
            ViewModel = InvoiceEditViewModel.Instance;
            this.DataContext = ViewModel;
            Invoice = new Invoice();
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

        public static void AddItemToInvoice(InvoiceItem item)
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
                var viewModel = (InvoiceEditViewModel)DataContext;
                viewModel.Clear();
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
            var viewModel = (InvoiceEditViewModel)DataContext;
            //var invoiceItem = new InvoiceItem { InvoiceId = viewModel.InvoiceId };

            //Frame.Navigate(typeof(InvoiceAddItemPage), invoiceItem);
            Frame.Navigate(typeof(InvoiceAddItemPage));
        }

        public void UpdateInvoiceItem_Click(InvoiceItem item)
        {
            Frame.Navigate(typeof(InvoiceAddItemPage), item);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is Invoice invoice)
            {
                ViewModel.InvoiceId = invoice.Id;
                await ViewModel.LoadData();
                ViewModel.SetSelectedEmployee(invoice.EmployeeId);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.Clear();
        }
    }
}
