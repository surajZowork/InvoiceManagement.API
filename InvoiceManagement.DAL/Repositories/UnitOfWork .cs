using InvoiceManagement.DAL.Data;
using InvoiceManagement.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IInvoiceRepository Invoices { get; }

        public UnitOfWork(AppDbContext context, IInvoiceRepository invoiceRepository)
        {
            _context = context;
            Invoices = invoiceRepository;
        }

        public Task<int> SaveChanges() => _context.SaveChangesAsync();

        public async Task ExecuteInTransaction(Func<Task> action)
        {
            await using var trx = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            catch
            {
                await trx.RollbackAsync();
                throw;
            }
        }
    }
}
