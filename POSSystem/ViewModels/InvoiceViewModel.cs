using POSSystem.Models;
using POSSystem.Repositories;
using POSSystem.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class InvoiceViewModel : BaseViewModel
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IProductRepository _productRepository;

        private readonly IStripeService _stripeService;
        private readonly IUriLauncher _uriLauncher;

        private List<Invoice> _invoices;

        public List<Invoice> Invoices
        {
            get => _invoices;
            set
            {
                _invoices = value;
                OnPropertyChanged();
            }
        }

        public InvoiceViewModel()
        {
            _invoiceRepository = ServiceFactory.GetChildOf<IInvoiceRepository>();
            _invoiceItemRepository = ServiceFactory.GetChildOf<IInvoiceItemRepository>();
            _productRepository = ServiceFactory.GetChildOf<IProductRepository>();

            _stripeService = ServiceFactory.GetChildOf<IStripeService>();
            _uriLauncher = ServiceFactory.GetChildOf<IUriLauncher>();

            Invoices = new List<Invoice>();

            try
            {
                _ = LoadInvoices();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // Constructor for Integration Testing
        public InvoiceViewModel(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        private async Task LoadInvoices()
        {
            try
            {
                var invoices = await _invoiceRepository.GetAllInvoices();

                foreach (var invoice in invoices)
                {
                    invoice.Index = invoices.IndexOf(invoice) + 1;
                    invoice.Timestamp = invoice.Timestamp.ToLocalTime();
                }

                Invoices = invoices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task PayInvoice(Invoice invoice)
        {
            try
            {
                var invoiceItems = await _invoiceItemRepository.GetInvoiceItemsByInvoiceId(invoice.Id);

                foreach (var item in invoiceItems)
                {
                    item.ProductName = (await _productRepository.GetProductById(item.ProductId)).Name;
                }

                var checkoutUrl = await _stripeService.CreateCheckoutSession(invoiceItems, invoice.Total);

                await _uriLauncher.LaunchUriAsync(new Uri(checkoutUrl));

                await _invoiceRepository.UpdateInvoiceIsPaid(invoice.Id, true);

                await LoadInvoices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
