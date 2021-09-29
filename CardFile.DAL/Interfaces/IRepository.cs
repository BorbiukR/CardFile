using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CardFile.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
        
        Task AddAsync(TEntity entity);

        Task<int> UpdateAsync(TEntity entity);

        Task<int> DeleteAsync(TEntity entity);

        Task DeleteByIdAsync(int id);

        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
    }
}