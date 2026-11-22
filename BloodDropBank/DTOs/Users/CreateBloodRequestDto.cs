
using System.ComponentModel.DataAnnotations;
using BloodDropBank.Enums;

namespace BloodDropBank.DTOs.Users
{
    public class CreateBloodRequestDto
    {
        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required, MinLength(2), MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Phone, MinLength(7), MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int HospitalId { get; set; }

        [Required]
        public BloodGroup BloodGroup { get; set; }

        [Required, Range(1, 40)]
        public int UnitsNeeded { get; set; }

        [MaxLength(300)]
        public string? Notes { get; set; }

        [MaxLength(100)]
        public string? MedicalCondition { get; set; }
    }
}
