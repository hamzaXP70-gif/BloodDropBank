using System.ComponentModel.DataAnnotations;
using BloodDropBank.Enums;

namespace BloodDropBank.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(150)]
        [MinLength(2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string NationalId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string City { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(20)]
        [MinLength(7)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public BloodGroup BloodGroup { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public decimal? WeightKg { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // قائمة طلبات الدم التي رفعها هذا المريض
        public ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();
        public ICollection<UserDonation> UserDonations { get; set; } = new List<UserDonation>();
    }
}
