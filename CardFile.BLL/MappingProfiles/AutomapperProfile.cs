using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.DAL.Entities;

namespace CardFile.BLL.MappingProfiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<CardFileEntitie, CardFileDTO>();
        }
    }
}