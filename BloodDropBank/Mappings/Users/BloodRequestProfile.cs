
using AutoMapper;
using BloodDropBank.DTOs.Users;
using BloodDropBank.Models;

namespace BloodDropBank.Mappings.Users
{
    public class BloodRequestProfile : Profile
    {
        public BloodRequestProfile()
        {
            CreateMap<BloodRequest, BloodRequestDto>();
            CreateMap<CreateBloodRequestDto, BloodRequest>();
        }
    }
}
