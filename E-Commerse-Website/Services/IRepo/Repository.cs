
using E_Commerse_Website.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;

namespace E_Commerse_Website.Services.IRepo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal readonly myContext _db;
        internal DbSet<T> dbSet;
        public Repository(myContext db)
        {
            _db = db;
            this.dbSet=_db.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IQueryable<T> query = dbSet;
            return await query.ToListAsync();
        }
        public async Task AddAsync(T obj)
        {
            await dbSet.AddAsync(obj);
        }
        
        public async Task<T> GetAsync(Expression<Func<T, bool>> entity)
        {
            IQueryable<T> query = dbSet;
            query=query.Where(entity);
            return await query.FirstOrDefaultAsync();
        }
        
        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAsyncRange(Expression<Func<T, bool>> entity)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(entity);
            return await query.ToListAsync();
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }
        public async Task<T> GetWithIncludeAsync(Expression<Func<T, bool>> entity,params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            query = query.Where(entity);
            return await query.FirstOrDefaultAsync();
        }
    }
}
