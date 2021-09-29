using CardFile.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CardFile.DAL.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        public readonly CardFileDbContext _cardFileDbContext;

        public Repository(CardFileDbContext context)
        {
            _cardFileDbContext = context;
        }

        public async Task AddAsync(T entity) => 
            await _cardFileDbContext.Set<T>().AddAsync(entity);

        public async Task<int> DeleteAsync(T entity)
        {
            _cardFileDbContext.Set<T>().Remove(entity);
            return await _cardFileDbContext.SaveChangesAsync();
        }

        public IQueryable<T> GetAll() => 
            _cardFileDbContext.Set<T>().AsNoTracking();

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken) => 
            await _cardFileDbContext.Set<T>().FindAsync(id);

        public async Task<int> UpdateAsync(T entity)
        {
            _cardFileDbContext.Set<T>().Update(entity);
            return await _cardFileDbContext.SaveChangesAsync();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
             _cardFileDbContext.Set<T>().Where(expression);

        public async Task DeleteByIdAsync(int id)
        {
            var res = await _cardFileDbContext.Set<T>().FindAsync(id);

            _cardFileDbContext.Set<T>().Remove(res);
        }
    }
}