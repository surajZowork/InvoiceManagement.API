using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceManagement.Entities.Invoice;

namespace InvoiceManagement.DAL.Interface
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetAllWithLines();
        Task<Invoice?> GetByIdWithLines(int id);
        Task<(IReadOnlyList<Invoice> Items, int TotalCount)> GetPagedWithLinesAsync(
            int pageNumber,
            int pageSize,
            string? sortBy,
            bool desc,
            string? customerName,
            DateTime? startDate,
            DateTime? endDate
            );
    }
}
