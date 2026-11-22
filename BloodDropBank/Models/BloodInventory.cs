using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BloodDropBank.Enums;

namespace BloodDropBank.Models
{
    public class BloodInventory
    {
        [Key]
        public int InventoryId { get; set; }

        // المفتاح الأجنبي لربط المستودع بالمستشفى
        [Required]
        public int HospitalId { get; set; }
        [JsonIgnore]
        public Hospital Hospital { get; set; } = null!;

        [Required]
        public BloodGroup BloodGroup { get; set; }

        // عدد أكياس الدم المتوفرة من هذه الفصيلة في هذا المستشفى
        [Range(0, 10000)]
        public int UnitsAvailable { get; set; } = 0;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public ICollection<InventoryDonation> InventoryDonations { get; set; } = new List<InventoryDonation>();
    }
}
