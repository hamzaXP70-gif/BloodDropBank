using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BloodDropBank.Models
{
    public class InventoryDonation
    {
        [Key]
        public int InventoryDonationId { get; set; }

        // Blood Request
        [Required]
        public int BloodRequestId { get; set; }
        [JsonIgnore]
        public BloodRequest BloodRequest { get; set; } = null!;

        // Blood Inventory
        [Required]
        public int BloodInventoryId { get; set; }
        [JsonIgnore]
        public BloodInventory BloodInventory { get; set; } = null!;

        // Number of issued units
        [Required]
        [Range(1, 40)]
        public int UnitsDonated { get; set; }

        // Issue Date
        public DateTime DonatedAt { get; set; } = DateTime.UtcNow;
    }
}
