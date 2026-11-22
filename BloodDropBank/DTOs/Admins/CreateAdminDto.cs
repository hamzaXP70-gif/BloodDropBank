
using System.ComponentModel.DataAnnotations;

namespace BloodDropBank.DTOs.Admins
{
    public class CreateAdminDto
    {
        [Required, MinLength(2), MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
