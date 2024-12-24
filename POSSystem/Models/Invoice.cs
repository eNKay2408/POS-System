using System;

namespace POSSystem.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Total { get; set; }
        public bool IsPaid { get; set; }
        public int Index { get; set; }
    }
}