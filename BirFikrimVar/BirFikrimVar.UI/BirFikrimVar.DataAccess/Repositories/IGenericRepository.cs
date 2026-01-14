using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BirFikrimVar.DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
        Task<List<T>> FindAsync(Expression<Func<T,bool>> predicate);
        Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter,string includeProperties = "");


    }
}
