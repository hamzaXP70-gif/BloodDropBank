
using System.ComponentModel.DataAnnotations;

namespace BloodDropBank.DTOs.Hospitals
{
    public class UpdateHospitalDto
    {
        [MinLength(2), MaxLength(150)]
        public string? HospitalName { get; set; }

        [MinLength(2), MaxLength(100)]
        public string? City { get; set; }

        [MinLength(2), MaxLength(200)]
        public string? Address { get; set; }

        [Phone, MinLength(7), MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [EmailAddress, MaxLength(256)]
        public string? Email { get; set; }
    }
}
