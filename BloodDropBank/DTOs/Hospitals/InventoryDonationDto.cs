
namespace BloodDropBank.DTOs.Hospitals
{
    public class InventoryDonationDto
    {
        public int InventoryDonationId { get; set; }
        public int BloodRequestId { get; set; }
        public int BloodInventoryId { get; set; }
        public int UnitsDonated { get; set; }
        public DateTime DonatedAt { get; set; }
    }
}
