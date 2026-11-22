
using BloodDropBank.Enums;

namespace BloodDropBank.DTOs.Users
{
    public class BloodRequestDto
    {
        public int BloodRequestId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int HospitalId { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public int UnitsNeeded { get; set; }
        public int RemainingUnits { get; set; }
        public RequestStatus Status { get; set; }
        public string? Notes { get; set; }
        public string? MedicalCondition { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
