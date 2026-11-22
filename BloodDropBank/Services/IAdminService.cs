
using BloodDropBank.DTOs;
using BloodDropBank.DTOs.Admins;
using BloodDropBank.DTOs.Users;
using BloodDropBank.DTOs.Hospitals;

namespace BloodDropBank.Services
{
    public interface IAdminService
    {
        // Admin Account
        Task<IEnumerable<AdminDto>> GetAllAsync();
        Task<AdminDto?> GetByIdAsync(int id);
        Task<AdminDto> AddAsync(CreateAdminDto dto);
        Task<AdminDto?> UpdateAsync(int id, UpdateAdminDto dto);
        Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto);

        // Users Management
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto> AddUserAsync(CreateUserDto dto);

        // Hospitals Management
        Task<IEnumerable<HospitalDto>> GetAllHospitalsAsync();
        Task<HospitalDto?> GetHospitalByIdAsync(int hospitalId);
        Task<HospitalDto> AddHospitalAsync(CreateHospitalDto dto);
    }
}
