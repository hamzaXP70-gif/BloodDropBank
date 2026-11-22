
using BloodDropBank.Enums;

namespace BloodDropBank.DTOs.Users
{
    public class UserDonationDto
    {
        public int UserDonationId { get; set; }
        public int UserId { get; set; }
        public int HospitalId { get; set; }
        public int? BloodRequestId { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public int UnitDonated { get; set; }
        public UserDonationType UserDonationType { get; set; }
        public DonationStatus DonationStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DonatedAt { get; set; }
    }
}
