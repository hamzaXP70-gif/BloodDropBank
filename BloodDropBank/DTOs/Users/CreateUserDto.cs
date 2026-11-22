
using System.ComponentModel.DataAnnotations;
using BloodDropBank.Enums;

namespace BloodDropBank.DTOs.Users
{
    public class CreateUserDto
    {
        [Required, MinLength(2), MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string NationalId { get; set; } = string.Empty;

        [Required, MinLength(2), MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required, Phone, MinLength(7), MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public BloodGroup BloodGroup { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public decimal? WeightKg { get; set; }
    }
}
