using AutoMapper;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Profiles
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
