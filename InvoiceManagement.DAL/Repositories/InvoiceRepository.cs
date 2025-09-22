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
    }
}
