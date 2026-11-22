
namespace BloodDropBank.DTOs.Hospitals
{
    public class HospitalDto
    {
        public int HospitalId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
