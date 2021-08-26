using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        IEnumerable<TModel> GetAll();

        Task<TModel> GetByIdAsync(int id);

        Task DeleteByIdAsync(int modelId);
    }
}