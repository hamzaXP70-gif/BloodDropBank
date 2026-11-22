
using System.ComponentModel.DataAnnotations;
using BloodDropBank.Enums;

namespace BloodDropBank.DTOs.Users
{
    public class CreateUserDonationDto
    {
        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int HospitalId { get; set; }

        public int? BloodRequestId { get; set; }

        [Required]
        public UserDonationType UserDonationType { get; set; }
    }
}
