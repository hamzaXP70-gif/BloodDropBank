
using AutoMapper;
using BloodDropBank.DTOs.Admins;
using BloodDropBank.Models;

namespace BloodDropBank.Mappings.Admins
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, AdminDto>();
            CreateMap<CreateAdminDto, Admin>();
        }
    }
}
