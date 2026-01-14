using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BirFikrimVar.DataAccess.Context;
using System.Linq.Expressions;

namespace BirFikrimVar.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync() { 
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id) {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity) { 
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(T entity) { 
            _dbSet.Update(entity);  
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        { 
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties) {
            IQueryable<T> query = _dbSet;
            foreach (var includeProperty in includeProperties) { 
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }


    }
}
