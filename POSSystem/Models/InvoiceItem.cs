using PropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace POSSystem.Models
{
    public class InvoiceItem: INotifyPropertyChanged
    {
        
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => Quantity * UnitPrice;
        public int Index { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
