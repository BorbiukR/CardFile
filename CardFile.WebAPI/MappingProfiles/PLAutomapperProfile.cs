using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.Contracts.Requests.Queries;
using CardFile.Contracts.Response;
using CardFile.WebAPI.Contracts.Request;

namespace CardFile.WebAPI.MappingProfiles
{
    public class PLAutomapperProfile : Profile
    {
        public PLAutomapperProfile()
        {          
            CreateMap<CardFileRequest, CardFileDTO>().ReverseMap();
            CreateMap<CardFileResponse, CardFileDTO>().ReverseMap();

            CreateMap<PaginationQuery, PaginationFilter>().ReverseMap();
        }       
    }
}