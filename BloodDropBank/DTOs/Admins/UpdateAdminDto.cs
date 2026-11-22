
using System.ComponentModel.DataAnnotations;

namespace BloodDropBank.DTOs.Admins
{
    public class UpdateAdminDto
    {
        [MinLength(2), MaxLength(100)]
        public string? FullName { get; set; }

        [Phone, MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [EmailAddress, MaxLength(256)]
        public string? Email { get; set; }
    }
}
