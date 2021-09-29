using CardFile.DAL.Entities;
using CardFile.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CardFile.DAL.Interfaces
{
    public interface ICardFileRepository : IRepository<CardFileEntitie>
    {
        IQueryable<CardFileEntitie> GetAllWithDetails();

        Task<CardFileEntitie> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken);
    }
}