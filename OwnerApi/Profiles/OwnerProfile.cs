using AutoMapper;
using OwnerApi.Dtos;
using OwnerApi.Models;

namespace OwnerApi.Profiles
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<Owner, OwnerReadDto>();
        }
    }
}
