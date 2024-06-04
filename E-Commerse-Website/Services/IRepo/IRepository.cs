using System.Linq.Expressions;

namespace E_Commerse_Website.Services.IRepo
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T obj);
        Task<T> GetAsync(Expression<Func<T, bool>> entity);
        Task RemoveAsync(T entity);
        Task<IEnumerable<T>> GetAsyncRange(Expression<Func<T, bool>> entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<T> GetWithIncludeAsync(Expression<Func<T, bool>> entity, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetWithIncludeAsyncRange(Expression<Func<T, bool>> entity, params Expression<Func<T, object>>[] includes);
    }
}
