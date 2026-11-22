using System.ComponentModel.DataAnnotations;

namespace BloodDropBank.Models
{
    public class Hospital
    {
        [Key]
        public int HospitalId { get; set; }

        [Required]
        [MaxLength(150)]
        [MinLength(2)]
        public string HospitalName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string City { get; set; } = string.Empty;

        [MaxLength(200)]
        [MinLength(2)]
        public string? Address { get; set; }

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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // العلاقات (Navigation Properties)
        public ICollection<BloodInventory> BloodInventories { get; set; } = new List<BloodInventory>();
        public ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();
        public ICollection<UserDonation> UserDonations { get; set; } = new List<UserDonation>();
    }
}
