using CardFile.DAL.Interfaces;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork 
    {
        ICardFileRepository CardFileRepository { get; }
        
        Task<int> SaveAsync();
    }
}