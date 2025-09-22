using System.Linq.Expressions;


namespace InvoiceManagement.DAL.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null,
                                         params Expression<Func<T, object>>[] includes);
    }

}
