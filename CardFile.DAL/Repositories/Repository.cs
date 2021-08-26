using CardFile.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public async Task AddAsync(T entity) => await _cardFileDbContext.Set<T>().AddAsync(entity);

        public void Delete(T entity) => _cardFileDbContext.Set<T>().Remove(entity);

        public IQueryable<T> FindAll() => _cardFileDbContext.Set<T>().AsNoTracking();

        public async Task<T> GetByIdAsync(int id) => await _cardFileDbContext.Set<T>().FindAsync(id);

        public async Task DeleteByIdAsync(int id)
        {
            var res = await _cardFileDbContext.Set<T>().FindAsync(id);

            _cardFileDbContext.Set<T>().Remove(res);
        }

        public void Update(T entity) => _cardFileDbContext.Set<T>().Update(entity);
    }
}