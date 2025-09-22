using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.Entities.Invoice
{
    public class InvoiceLine
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
