using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CardFile.DAL.Repositories
{
    public class CardTextFileRepository : Repository<CardFileEntitie>, ICardFileRepository
    {
        public CardTextFileRepository(CardFileDbContext cardFileDbContext) : base(cardFileDbContext) 
        { 

        }

        public IQueryable<CardFileEntitie> GetAllWithDetails()
        {
            return _cardFileDbContext
                .Set<CardFileEntitie>()
                .Include(x => x.FileInfoEntitie);
        }

        public async Task<CardFileEntitie> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken)
        {
            return await FindByCondition(x => x.Id.Equals(id))
                 .Include(x => x.FileInfoEntitie)
                 .FirstOrDefaultAsync(); ;
        }
    }
}