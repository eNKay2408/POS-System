using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using POSSystem.Models;
using POSSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using WinRT.Interop;


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
            // Render the current page to a bitmap
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(this);

            // Get the pixel buffer and convert it to a stream
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            using (var stream = new InMemoryRandomAccessStream())
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)renderTargetBitmap.PixelWidth,
                    (uint)renderTargetBitmap.PixelHeight,
                    96, 96,
                    pixelBuffer.ToArray());
                await encoder.FlushAsync();

                // Create a PDF document
                var pdfDocument = new PdfDocument();
                var pdfPage = pdfDocument.AddPage();
                var xGraphics = XGraphics.FromPdfPage(pdfPage);
                var xImage = XImage.FromStream(stream.AsStream());

                double desiredWidth = pdfPage.Width.Point; // 100% of the page width
                double desiredHeight = pdfPage.Height.Point; // 100% of the page height

                // Draw the image on the PDF page with the specified dimensions
                xGraphics.DrawImage(xImage, 0, 0, desiredWidth, desiredHeight);

                // Show the file save picker
                var savePicker = new FileSavePicker
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                    SuggestedFileName = "Invoice",
                    FileTypeChoices = { { "PDF", new List<string> { ".pdf" } } }
                };

                // Get the window handle and initialize the file picker with it
                var hwnd = WindowNative.GetWindowHandle(App.AppMainWindow);
                InitializeWithWindow.Initialize(savePicker, hwnd);

                var file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    using (var fileStream = await file.OpenStreamForWriteAsync())
                    {
                        pdfDocument.Save(fileStream);
                    }
                }
            }
        }
    }
}
