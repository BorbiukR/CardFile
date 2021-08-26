using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.DAL.Entities;

namespace CardFile.BLL.MappingProfiles
{
    public class BLLAutomapperProfile : Profile
    {
        public BLLAutomapperProfile()
        {
            CreateMap<CardFileEntitie,CardFileDTO>().ReverseMap();
        }
    }
}