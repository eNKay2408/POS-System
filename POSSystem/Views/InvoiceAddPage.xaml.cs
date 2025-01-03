using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;

namespace POSSystem.Views
{
    public sealed partial class InvoiceAddPage : Page
    {
        public InvoiceAddViewModel ViewModel { get; set; }
        public InvoiceAddPage()
        {
            InitializeComponent();
            ViewModel = InvoiceAddViewModel.Instance;
            this.DataContext = ViewModel;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.SaveInvoice();
                Frame.Navigate(typeof(InvoicesPage));
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
            InvoiceAddViewModel.Instance.AddItemToInvoice(item);

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
                var viewModel = (InvoiceAddViewModel)DataContext;
                viewModel.DiscardChanges();
                Frame.Navigate(typeof(InvoicesPage));
            }
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var invoiceItem = (InvoiceItem)button.DataContext;

            var viewModel = (InvoiceAddViewModel)DataContext;
            await viewModel.DeleteItem(invoiceItem);
        }

        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var invoiceItem = (InvoiceItem)button.DataContext;

            Frame.Navigate(typeof(InvoiceAddItemPage), invoiceItem);
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (InvoiceAddViewModel)DataContext;
            //var invoiceItem = new InvoiceItem { InvoiceId = viewModel.InvoiceId };

            //Frame.Navigate(typeof(InvoiceAddItemPage), invoiceItem);
            Frame.Navigate(typeof(InvoiceAddItemPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //if (e.Parameter is int invoiceId)
            //{
            //    var viewModel = (InvoiceAddViewModel)DataContext;
            //    viewModel.InvoiceId = invoiceId;
            //}
            if(e.Parameter is InvoiceItem invoiceItem)
            {
                var viewModel = (InvoiceAddViewModel)DataContext;
                viewModel.InvoiceItems.Add(invoiceItem);
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteInvoiceItem(0);
        }
    }
}
