using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        IEnumerable<TModel> GetAll(CancellationToken cancellationToken);

        Task<TModel> GetByIdAsync(int id);

        Task<bool> DeleteByIdAsync(int modelId);
    }
}