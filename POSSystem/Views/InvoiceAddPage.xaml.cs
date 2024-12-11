using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using POSSystem.ViewModels;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POSSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InvoiceAddPage : Page
    {
        public InvoiceAddViewModel ViewModel { get; set; } = new InvoiceAddViewModel();


        public InvoiceAddPage()
        {
            this.InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "Save invoice clicked",
                PrimaryButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private void Discard_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InvoiceAddProductPage));
        }
    }

}
