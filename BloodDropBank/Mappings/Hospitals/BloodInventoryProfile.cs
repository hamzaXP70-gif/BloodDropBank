
using AutoMapper;
using BloodDropBank.DTOs.Hospitals;
using BloodDropBank.Models;

namespace BloodDropBank.Mappings.Hospitals
{
    public class BloodInventoryProfile : Profile
    {
        public BloodInventoryProfile()
        {
            CreateMap<BloodInventory, BloodInventoryDto>();
        }
    }
}
