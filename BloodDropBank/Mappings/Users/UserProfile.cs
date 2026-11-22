
using AutoMapper;
using BloodDropBank.DTOs.Users;
using BloodDropBank.Models;

namespace BloodDropBank.Mappings.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
        }
    }
}
