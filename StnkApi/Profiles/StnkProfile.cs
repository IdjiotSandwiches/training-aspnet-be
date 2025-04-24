using AutoMapper;
using StnkApi.Dtos;
using StnkApi.Models;

namespace StnkApi.Profiles
{
    public class StnkProfile : Profile
    {
        public StnkProfile()
        {
            CreateMap<Stnk, StnkInsertReadDto>();
            CreateMap<Stnk, StnkUpdateReadDto>();
            CreateMap<StnkInsertWriteDto, Stnk>();
            CreateMap<Stnk, AllStnkDto>();
            CreateMap<StnkUpdateWriteDto, Stnk>();
            CreateMap<StnkInsertReadDto, StnkInsertWriteDto>();
        }
    }
}
