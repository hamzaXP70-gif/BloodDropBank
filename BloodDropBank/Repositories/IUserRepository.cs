
using BloodDropBank.Models;
using BloodDropBank.Enums;

namespace BloodDropBank.Repositories
{
    public interface IUserRepository
    {
        // User Account
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        Task<User?> GetByNationalIdAsync(string nationalId);
        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(int id, User user);

        // Blood Requests
        Task<BloodRequest?> GetBloodRequestByIdAsync(int id);
        Task<IEnumerable<BloodRequest>> GetBloodRequestsByUserIdAsync(int userId);
        Task<IEnumerable<BloodRequest>> GetAvailableBloodRequestsAsync(
            int userId, BloodGroup[] compatibleGroups);
        Task<BloodRequest> AddBloodRequestAsync(BloodRequest request);
        Task<BloodRequest?> UpdateBloodRequestAsync(int id, BloodRequest request);

        // User Donations
        Task<UserDonation?> GetUserDonationByIdAsync(int id);
        Task<IEnumerable<UserDonation>> GetUserDonationsByUserIdAsync(int userId);
        Task<bool> HasPendingDonationAsync(int userId);
        Task<DateTime?> GetLastApprovedDonationDateAsync(int userId);
        Task<IEnumerable<UserDonation>> GetPendingDonationsByBloodRequestIdAsync(int bloodRequestId);
        Task<UserDonation> AddUserDonationAsync(UserDonation donation);
        Task<UserDonation?> UpdateUserDonationAsync(int id, UserDonation donation);

        // Transaction
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation);
    }
}
