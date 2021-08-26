using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.WebAPI.Contracts.Request;

namespace CardFile.WebAPI.MappingProfiles
{
    public class PLAutomapperProfile : Profile
    {
        public PLAutomapperProfile()
        {          
            CreateMap<CardFileRequest, CardFileDTO>().ReverseMap();
        }       
    }
}