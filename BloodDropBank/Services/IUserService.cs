
using BloodDropBank.DTOs;
using BloodDropBank.DTOs.Users;

namespace BloodDropBank.Services
{
    public interface IUserService
    {
        // User Account
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> AddAsync(CreateUserDto dto);
        Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto);
        Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto);

        // Blood Requests
        Task<BloodRequestDto?> GetBloodRequestByIdAsync(int id);
        Task<IEnumerable<BloodRequestDto>> GetBloodRequestsByUserIdAsync(int userId);
        Task<IEnumerable<BloodRequestDto>> GetAvailableBloodRequestsAsync(int userId);
        Task<BloodRequestDto> AddBloodRequestAsync(CreateBloodRequestDto dto);
        Task<bool> CancelBloodRequestAsync(int id, int userId);

        // User Donations
        Task<UserDonationDto?> GetUserDonationByIdAsync(int id);
        Task<IEnumerable<UserDonationDto>> GetUserDonationsByUserIdAsync(int userId);
        Task<UserDonationDto> AddUserDonationAsync(CreateUserDonationDto dto);
    }
}
