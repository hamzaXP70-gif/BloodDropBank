using System.ComponentModel.DataAnnotations;

namespace BloodDropBank.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
