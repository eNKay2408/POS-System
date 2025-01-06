using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using POSSystem.Helpers;
using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POSSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InvoicePrintPage : Page
    {
        public class PrintViewModel
        {
            public int InvoiceId { get; set; }
            public FullObservableCollection<InvoiceItem> InvoiceItems { get; set; }

            public decimal Total { get; set; }

            private IInvoiceItemRepository _invoiceItemRepository;
            public PrintViewModel()
            {
                InvoiceItems = new();
                InvoiceId = -1;
                Total = 0.00m;
                _invoiceItemRepository = ServiceFactory.GetChildOf<IInvoiceItemRepository>();
            }


            public async Task LoadInvoiceItems()
            {
                var invoiceItems = await _invoiceItemRepository.GetInvoiceItemsByInvoiceId(InvoiceId);
                InvoiceItems = new FullObservableCollection<InvoiceItem>(invoiceItems);
                CalculateTotal();
            }

            private void CalculateTotal()
            {
                if (InvoiceItems == null)
                {
                    return;
                }

                decimal total = 0;
                foreach (var item in InvoiceItems)
                {
                    total += item.SubTotal;
                }

                Total = total;
            }
        }

        public string EmployeeName { get; set; }

        public PrintViewModel ViewModel { get; set; }
        public InvoicePrintPage()
        {
            ViewModel = new PrintViewModel();
            this.DataContext = ViewModel;

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is Invoice invoice)
            {
                ViewModel.InvoiceId = invoice.Id;
                EmployeeName = invoice.EmployeeName;
                await ViewModel.LoadInvoiceItems();
                this.InitializeComponent();
            }    
        }

        private async void SavePDF_Click(object sender, RoutedEventArgs e)
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

                double desiredWidth = pdfPage.Width.Point * 1;
                double desiredHeight = pdfPage.Height.Point * 0.5; 

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
