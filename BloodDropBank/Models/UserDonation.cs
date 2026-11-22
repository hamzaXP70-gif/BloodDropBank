using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BloodDropBank.Enums;

namespace BloodDropBank.Models
{
    public class UserDonation
    {
        [Key]
        public int UserDonationId { get; set; }

        [Required]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!;

        [Required]
        public int HospitalId { get; set; }
        [JsonIgnore]
        public Hospital Hospital { get; set; } = null!;

        // يكون له قيمة إذا كان التبرع لطلب مريض، ويكون null إذا كان تبرعاً عاماً لمستودع المستشفى
        public int? BloodRequestId { get; set; }
        public BloodRequest? BloodRequest { get; set; }

        [Required]
        public BloodGroup BloodGroup { get; set; }

        public int UnitDonated { get; set; } = 1;

        [Required]
        public UserDonationType UserDonationType { get; set; }

        public DonationStatus DonationStatus { get; set; } = DonationStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DonatedAt { get; set; }
    }
}
