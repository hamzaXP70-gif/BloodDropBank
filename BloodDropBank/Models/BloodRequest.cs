using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BloodDropBank.Enums;

namespace BloodDropBank.Models
{
    public class BloodRequest
    {
        [Key]
        public int BloodRequestId { get; set; }

        [Required]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!;

        [Required]
        [MinLength(2)]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        [MinLength(7)]
        public string? PhoneNumber { get; set; }

        // المستشفى الذي يتواجد فيه المريض
        [Required]
        public int HospitalId { get; set; }
        [JsonIgnore]
        public Hospital Hospital { get; set; } = null!;
        
        [Required]
        public BloodGroup BloodGroup { get; set; }

        [Required]
        [Range(1, 40)]
        public int UnitsNeeded { get; set; }

        [Range(0, 40)]
        public int RemainingUnits { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        // المتبرع الذي قبل طلب المريض وتبرع له (يبقى null حتى يقبله متبرع)
        public ICollection<UserDonation> UserDonations { get; set; } = new List<UserDonation>();

        public ICollection<InventoryDonation> InventoryDonations { get; set; } = new List<InventoryDonation>();

        [MaxLength(300)]
        public string? Notes { get; set; }

        [MaxLength(100)]
        public string? MedicalCondition { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
    }
}
