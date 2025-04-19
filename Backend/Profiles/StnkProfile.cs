using AutoMapper;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Profiles
{
    public class StnkProfile : Profile
    {
        public StnkProfile()
        {
            CreateMap<STNK, StnkInsertReadDto>();
            CreateMap<STNK, StnkUpdateReadDto>();
            CreateMap<StnkWriteDto, STNK>();
        }
    }
}
