
using BloodDropBank.DTOs;
using BloodDropBank.DTOs.Hospitals;
using BloodDropBank.DTOs.Users;

namespace BloodDropBank.Services
{
    public interface IHospitalService
    {
        // Hospital Account
        Task<IEnumerable<HospitalDto>> GetAllAsync();
        Task<HospitalDto?> GetByIdAsync(int id);
        Task<HospitalDto> AddAsync(CreateHospitalDto dto);
        Task<HospitalDto?> UpdateAsync(int id, UpdateHospitalDto dto);
        Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto);

        // Blood Inventory
        Task<IEnumerable<BloodInventoryDto>> GetBloodInventoriesByHospitalIdAsync(int hospitalId);
        Task<BloodInventoryDto?> GetBloodInventoryByIdAsync(int hospitalId, int inventoryId);

        // Blood Requests
        Task<IEnumerable<BloodRequestDto>> GetBloodRequestsByHospitalIdAsync(int hospitalId);
        Task<BloodRequestDto?> GetBloodRequestByIdAsync(int hospitalId, int requestId);

        // User Donations
        Task<IEnumerable<UserDonationDto>> GetUserDonationsByHospitalIdAsync(int hospitalId);
        Task<UserDonationDto?> GetUserDonationByIdAsync(int hospitalId, int donationId);
        Task<UserDonationDto?> ApproveUserDonationAsync(int hospitalId, int donationId);
        Task<UserDonationDto?> RejectUserDonationAsync(int hospitalId, int donationId);

        // Inventory Donations
        Task<IEnumerable<InventoryDonationDto>> GetInventoryDonationsByHospitalIdAsync(int hospitalId);
        Task<InventoryDonationDto?> GetInventoryDonationByIdAsync(int hospitalId, int donationId);
        Task<InventoryDonationDto> AddInventoryDonationAsync(CreateInventoryDonationDto dto);
    }
}
