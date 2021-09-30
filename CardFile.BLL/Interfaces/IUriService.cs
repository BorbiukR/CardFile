using CardFile.Contracts.Requests.Queries;
using System;

namespace CardFile.BLL.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationQuery pagination = null);
    }
}