using POSSystem.Models;
using POSSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.ViewModels
{
    public class InvoiceViewModel: BaseViewModel
    {
        private readonly IInvoiceRepository _invoiceRepository;
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
            _invoiceRepository = new InvoiceRepository();
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

        // Constructor for Unit testing
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
                }
                Invoices = invoices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
