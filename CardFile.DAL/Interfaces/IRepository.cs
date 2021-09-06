using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CardFile.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> FindAll(CancellationToken cancellationToken);

        Task<TEntity> GetByIdAsync(int id);
        
        Task AddAsync(TEntity entity);

        void Update(TEntity entity);
        
        void Delete(TEntity entity);

        Task DeleteByIdAsync(int id);

        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);
    }
}