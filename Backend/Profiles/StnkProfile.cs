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
            CreateMap<StnkWriteDto, Stnk>();
            CreateMap<Stnk, AllStnkDto>();
        }
    }
}
