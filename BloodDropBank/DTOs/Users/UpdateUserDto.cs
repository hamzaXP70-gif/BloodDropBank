
using System.ComponentModel.DataAnnotations;
using BloodDropBank.Enums;

namespace BloodDropBank.DTOs.Users
{
    public class UpdateUserDto
    {
        [MinLength(2), MaxLength(150)]
        public string? FullName { get; set; }

        [MinLength(2), MaxLength(100)]
        public string? City { get; set; }

        [Phone, MinLength(7), MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [EmailAddress, MaxLength(256)]
        public string? Email { get; set; }

        public BloodGroup? BloodGroup { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public decimal? WeightKg { get; set; }
    }
}
