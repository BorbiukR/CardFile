using CardFile.BLL.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        IEnumerable<TModel> GetAll(PaginationFilter paginationFilter = null);

        Task<TModel> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<bool> DeleteByIdAsync(int modelId, CancellationToken cancellationToken);
    }
}