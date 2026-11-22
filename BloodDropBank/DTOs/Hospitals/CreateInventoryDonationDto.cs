
using System.ComponentModel.DataAnnotations;

namespace BloodDropBank.DTOs.Hospitals
{
    public class CreateInventoryDonationDto
    {
        [Required, Range(1, int.MaxValue)]
        public int HospitalId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int BloodRequestId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int BloodInventoryId { get; set; }

        [Required, Range(1, 40)]
        public int UnitsDonated { get; set; }
    }
}
