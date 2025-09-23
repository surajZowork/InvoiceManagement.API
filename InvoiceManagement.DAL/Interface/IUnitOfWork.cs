using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceManagement.DAL.Interface
{
    public interface IUnitOfWork
    {
        IInvoiceRepository Invoices { get; }
        Task<int> SaveChanges();
        Task ExecuteInTransaction(Func<Task> action);
    }

}
