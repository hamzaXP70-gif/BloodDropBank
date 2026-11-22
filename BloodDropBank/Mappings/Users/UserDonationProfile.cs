
using AutoMapper;
using BloodDropBank.DTOs.Users;
using BloodDropBank.Models;

namespace BloodDropBank.Mappings.Users
{
    public class UserDonationProfile : Profile
    {
        public UserDonationProfile()
        {
            CreateMap<UserDonation, UserDonationDto>();
            CreateMap<CreateUserDonationDto, UserDonation>();
        }
    }
}
