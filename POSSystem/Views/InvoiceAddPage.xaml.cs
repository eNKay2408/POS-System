using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static POSSystem.Views.InvoicesPage;

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
            InvoiceAddViewModel.Instance.AddItemToInvoice(item);
        }

        public void DeleteItemFromInvoice(InvoiceItem item)
        {
            ViewModel.DeleteItemFromInvoice(item);
        }

        public static async void UpdateInvoiceItem(InvoiceItem item)
        {
            try
            {
                InvoiceAddViewModel.Instance.UpdateInvoiceItem(item);
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
                var viewModel = (InvoiceAddViewModel)DataContext;
                viewModel.DiscardChanges();
                Frame.GoBack();
            }
        }

        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var invoiceItem = (InvoiceItem)button.DataContext;

            var viewModel = (InvoiceAddViewModel)DataContext;
            await viewModel.DeleteItem(invoiceItem);
        }


        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InvoiceAddItemPage), ParentPageOrigin.InvoiceAddPage);
        }

        public void UpdateInvoiceItem_Click(InvoiceItem item)
        {
            Frame.Navigate(typeof(InvoiceAddItemPage), (item, ParentPageOrigin.InvoiceAddPage));
        }

        public async static Task ModifyEmployee_Handler(Employee employee)
        {
            await InvoiceAddViewModel.Instance.ModifyEmployeeAsync(employee);
        }

    }
}
