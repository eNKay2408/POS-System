using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public DateTime Timestamp { get; set; }
        public string EmployeeName { get; set; }
        public decimal? Total { get; set; }

        //public List<Product> Products { get; set; }
        public int Index { get; set; }
    }
}
