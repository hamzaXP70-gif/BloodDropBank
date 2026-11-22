
namespace BloodDropBank.DTOs.Admins
{
    public class AdminDto
    {
        public int AdminId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
