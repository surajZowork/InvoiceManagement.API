using InvoiceManagement.DAL.Data;
using InvoiceManagement.DAL.Interface;
using InvoiceManagement.Entities.Invoice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.DAL.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> Add(Invoice entity)
        {
            await _context.Invoices.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<Invoice>> GetAll(Expression<Func<Invoice, bool>>? filter = null,
            params Expression<Func<Invoice, object>>[] includes)
        {
            IQueryable<Invoice> query = _context.Invoices.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetAllWithLines()
        {
            return await _context.Invoices
                .Include(i => i.InvoiceLines)
                .OrderByDescending(i => i.Id)
                .ToListAsync();
        }
        public async Task<Invoice?> GetByIdWithLines(int id)
        {
            return await _context.Invoices
            .Include(i => i.InvoiceLines)
            .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<(IReadOnlyList<Invoice> Items, int TotalCount)> GetPagedWithLinesAsync(
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool desc,
        string? customerName,
        DateTime? startDate,
        DateTime? endDate)
        {
            IQueryable<Invoice> query = _context.Invoices.Include(i => i.InvoiceLines);


            // Filters
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                var term = customerName.Trim().ToLower();
                query = query.Where(i => i.CustomerName.ToLower().Contains(term));
            }
            if (startDate.HasValue)
            {
                var s = startDate.Value.Date;
                query = query.Where(i => i.InvoiceDate >= s);
            }
            if (endDate.HasValue)
            {
                var e = endDate.Value.Date.AddDays(1).AddTicks(-1); // inclusive end of day
                query = query.Where(i => i.InvoiceDate <= e);
            }


            // Sorting
            query = (sortBy?.ToLower()) switch
            {
                "customername" => desc ? query.OrderByDescending(i => i.CustomerName) : query.OrderBy(i => i.CustomerName),
                "invoicedate" => desc ? query.OrderByDescending(i => i.InvoiceDate) : query.OrderBy(i => i.InvoiceDate),
                "totalamount" => desc ? query.OrderByDescending(i => i.TotalAmount) : query.OrderBy(i => i.TotalAmount),
                _ => desc ? query.OrderByDescending(i => i.Id) : query.OrderBy(i => i.Id)
            };


            var total = await query.CountAsync();
            var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();


            return (items, total);
        }

    }
}
