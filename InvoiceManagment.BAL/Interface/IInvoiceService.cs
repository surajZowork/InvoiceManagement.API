using InvoiceManagement.Entities.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InvoiceManagement.BAL.DTOs.InvoiceDtos;

namespace InvoiceManagement.BAL.Interface
{
    public interface IInvoiceService
    {
        Task<InvoiceReadDto> Create(InvoiceCreateDto dto);
 
        Task<InvoiceReadDto?> GetById(int id);

        Task<PagedResult<InvoiceReadDto>> GetPaged(
       int pageNumber,
       int pageSize,
       string? sortBy,
       string? sortDir,
       string? customerName,
       DateTime? startDate,
       DateTime? endDate);
    }

}

