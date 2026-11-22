
using AutoMapper;
using BloodDropBank.DTOs.Hospitals;
using BloodDropBank.Models;

namespace BloodDropBank.Mappings.Hospitals
{
    public class InventoryDonationProfile : Profile
    {
        public InventoryDonationProfile()
        {
            CreateMap<InventoryDonation, InventoryDonationDto>();
            CreateMap<CreateInventoryDonationDto, InventoryDonation>();
        }
    }
}
