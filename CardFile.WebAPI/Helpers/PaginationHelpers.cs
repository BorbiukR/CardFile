using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.Contracts.Requests.Queries;
using CardFile.Contracts.Responses;
using System.Collections.Generic;
using System.Linq;

namespace CardFile.WebAPI.Helpers
{
    public class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter pagination, List<T> response)
        {
            var nextPage = pagination.PageNumber >= 1
                   ? uriService.GetPageUri(new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize))
                   : null;

            var previousPage = pagination.PageNumber - 1 >= 1
                ? uriService.GetPageUri(new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize))
                : null;

            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                PreviousPage = previousPage,
                NextPage = response.Any() ? nextPage : null,
            };
        }
    }
}