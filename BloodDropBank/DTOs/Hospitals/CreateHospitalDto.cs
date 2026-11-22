
using System.ComponentModel.DataAnnotations;

namespace BloodDropBank.DTOs.Hospitals
{
    public class CreateHospitalDto
    {
        [Required, MinLength(2), MaxLength(150)]
        public string HospitalName { get; set; } = string.Empty;

        [Required, MinLength(2), MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MinLength(2), MaxLength(200)]
        public string? Address { get; set; }

        [Required, Phone, MinLength(7), MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Initial Blood Inventory
        [Range(0, 10000)]
        public int APosUnits { get; set; } = 0;

        [Range(0, 10000)]
        public int ANegUnits { get; set; } = 0;

        [Range(0, 10000)]
        public int BPosUnits { get; set; } = 0;

        [Range(0, 10000)]
        public int BNegUnits { get; set; } = 0;

        [Range(0, 10000)]
        public int ABPosUnits { get; set; } = 0;

        [Range(0, 10000)]
        public int ABNegUnits { get; set; } = 0;

        [Range(0, 10000)]
        public int OPosUnits { get; set; } = 0;

        [Range(0, 10000)]
        public int ONegUnits { get; set; } = 0;
    }
}
