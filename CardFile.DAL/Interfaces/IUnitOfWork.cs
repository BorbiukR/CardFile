using CardFile.DAL.Interfaces;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork 
    {
        ICardTextFileRepository CardTextFileRepository { get; }
        
        Task<int> SaveAsync();
    }
}