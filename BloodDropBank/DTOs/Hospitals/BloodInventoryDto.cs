
using BloodDropBank.Enums;

namespace BloodDropBank.DTOs.Hospitals
{
    public class BloodInventoryDto
    {
        public int InventoryId { get; set; }
        public int HospitalId { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public int UnitsAvailable { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
