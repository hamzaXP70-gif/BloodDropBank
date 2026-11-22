
using AutoMapper;
using BloodDropBank.DTOs.Hospitals;
using BloodDropBank.Models;

namespace BloodDropBank.Mappings.Hospitals
{
    public class HospitalProfile : Profile
    {
        public HospitalProfile()
        {
            CreateMap<Hospital, HospitalDto>();
            CreateMap<CreateHospitalDto, Hospital>();
        }
    }
}
