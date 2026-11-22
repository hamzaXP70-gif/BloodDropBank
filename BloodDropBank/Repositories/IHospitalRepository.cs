
using BloodDropBank.Models;
using BloodDropBank.Enums;

namespace BloodDropBank.Repositories
{
    public interface IHospitalRepository
    {
        // Hospital Account
        Task<IEnumerable<Hospital>> GetAllAsync();
        Task<Hospital?> GetByIdAsync(int id);
        Task<Hospital?> GetByEmailAsync(string email);
        Task<Hospital?> GetByPhoneNumberAsync(string phoneNumber);
        Task<Hospital> AddAsync(Hospital hospital);
        Task<Hospital?> UpdateAsync(int id, Hospital hospital);

        // Blood Inventory
        Task<IEnumerable<BloodInventory>> GetBloodInventoriesByHospitalIdAsync(int hospitalId);
        Task<BloodInventory?> GetBloodInventoryByIdAsync(int id);
        Task<BloodInventory?> GetBloodInventoryByHospitalAndGroupAsync(int hospitalId, BloodGroup bloodGroup);
        Task AddBloodInventoriesAsync(IEnumerable<BloodInventory> inventories);
        Task<BloodInventory?> UpdateBloodInventoryAsync(int id, BloodInventory inventory);

        // Blood Requests
        Task<BloodRequest?> GetBloodRequestByIdAsync(int id);
        Task<IEnumerable<BloodRequest>> GetBloodRequestsByHospitalIdAsync(int hospitalId);
        Task<BloodRequest?> UpdateBloodRequestAsync(int id, BloodRequest request);

        // User Donations
        Task<UserDonation?> GetUserDonationByIdAsync(int id);
        Task<IEnumerable<UserDonation>> GetUserDonationsByHospitalIdAsync(int hospitalId);
        Task<IEnumerable<UserDonation>> GetPendingDonationsByBloodRequestIdAsync(int bloodRequestId);
        Task<UserDonation?> UpdateUserDonationAsync(int id, UserDonation donation);

        // Inventory Donations
        Task<InventoryDonation?> GetInventoryDonationByIdAsync(int id);
        Task<IEnumerable<InventoryDonation>> GetInventoryDonationsByHospitalIdAsync(int hospitalId);
        Task<InventoryDonation> AddInventoryDonationAsync(InventoryDonation donation);

        // Transaction
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
    }
}
