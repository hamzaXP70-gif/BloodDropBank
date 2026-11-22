
using BloodDropBank.Enums;

namespace BloodDropBank.DTOs.Users
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public BloodGroup BloodGroup { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? WeightKg { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
