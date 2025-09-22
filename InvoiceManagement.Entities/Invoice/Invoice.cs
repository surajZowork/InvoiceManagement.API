using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.Entities.Invoice
{
    public class Invoice
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();
    }
}
