
using System.Data;
using BloodDropBank.Data;
using BloodDropBank.Models;
using BloodDropBank.Enums;
using Microsoft.EntityFrameworkCore;

namespace BloodDropBank.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BloodDbContext _context;

        public UserRepository(BloodDbContext context)
        {
            _context = context;
        }

        // User Account

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<User?> GetByNationalIdAsync(string nationalId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.NationalId == nationalId);
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return null;

            existingUser.FullName = user.FullName;
            existingUser.City = user.City;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Email = user.Email;
            existingUser.BloodGroup = user.BloodGroup;
            existingUser.BirthDate = user.BirthDate;
            existingUser.WeightKg = user.WeightKg;

            if (!string.IsNullOrEmpty(user.Password))
                existingUser.Password = user.Password;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        // Blood Requests

        public async Task<BloodRequest?> GetBloodRequestByIdAsync(int id)
        {
            return await _context.BloodRequests.FindAsync(id);
        }

        public async Task<IEnumerable<BloodRequest>> GetBloodRequestsByUserIdAsync(int userId)
        {
            return await _context.BloodRequests
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<BloodRequest>> GetAvailableBloodRequestsAsync(
            int userId, BloodGroup[] compatibleGroups)
        {
            return await _context.BloodRequests
                .Where(r => r.UserId != userId &&
                    compatibleGroups.Contains(r.BloodGroup) &&
                    r.RemainingUnits > 0 &&
                    (r.Status == RequestStatus.Pending ||
                     r.Status == RequestStatus.PartiallyFulfilled))
                .ToListAsync();
        }

        public async Task<BloodRequest> AddBloodRequestAsync(BloodRequest request)
        {
            await _context.BloodRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<BloodRequest?> UpdateBloodRequestAsync(
            int id, BloodRequest request)
        {
            var existingRequest = await _context.BloodRequests.FindAsync(id);
            if (existingRequest == null) return null;

            existingRequest.RemainingUnits = request.RemainingUnits;
            existingRequest.Status = request.Status;
            existingRequest.CompletedAt = request.CompletedAt;

            await _context.SaveChangesAsync();
            return existingRequest;
        }

        // User Donations

        public async Task<UserDonation?> GetUserDonationByIdAsync(int id)
        {
            return await _context.UserDonations.FindAsync(id);
        }

        public async Task<IEnumerable<UserDonation>> GetUserDonationsByUserIdAsync(int userId)
        {
            return await _context.UserDonations
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> HasPendingDonationAsync(int userId)
        {
            return await _context.UserDonations
                .AnyAsync(d => d.UserId == userId &&
                    d.DonationStatus == DonationStatus.Pending);
        }

        public async Task<DateTime?> GetLastApprovedDonationDateAsync(int userId)
        {
            return await _context.UserDonations
                .Where(d => d.UserId == userId &&
                    d.DonationStatus == DonationStatus.Approved &&
                    d.DonatedAt.HasValue)
                .OrderByDescending(d => d.DonatedAt)
                .Select(d => d.DonatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDonation>> GetPendingDonationsByBloodRequestIdAsync(
            int bloodRequestId)
        {
            return await _context.UserDonations
                .Where(d => d.BloodRequestId == bloodRequestId &&
                    d.DonationStatus == DonationStatus.Pending)
                .ToListAsync();
        }

        public async Task<UserDonation> AddUserDonationAsync(UserDonation donation)
        {
            await _context.UserDonations.AddAsync(donation);
            await _context.SaveChangesAsync();
            return donation;
        }

        public async Task<UserDonation?> UpdateUserDonationAsync(
            int id, UserDonation donation)
        {
            var existingDonation = await _context.UserDonations.FindAsync(id);
            if (existingDonation == null) return null;

            existingDonation.DonationStatus = donation.DonationStatus;
            existingDonation.DonatedAt = donation.DonatedAt;

            await _context.SaveChangesAsync();
            return existingDonation;
        }

        // Transaction

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
        {
            await using var transaction = await _context.Database
                .BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var result = await operation();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
