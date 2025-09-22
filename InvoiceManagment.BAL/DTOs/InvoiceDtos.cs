using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.BAL.DTOs
{
    public class InvoiceDtos
    {
        public record InvoiceLineCreateDto(string Description, int Quantity, decimal UnitPrice);


        public record InvoiceCreateDto(string CustomerName, DateTime? InvoiceDate, List<InvoiceLineCreateDto> Lines);


        public record InvoiceLineReadDto(int Id, string Description, int Quantity, decimal UnitPrice, decimal LineTotal);


        public record InvoiceReadDto(int Id, string CustomerName, DateTime InvoiceDate, decimal TotalAmount, List<InvoiceLineReadDto> Lines);
    }
}
