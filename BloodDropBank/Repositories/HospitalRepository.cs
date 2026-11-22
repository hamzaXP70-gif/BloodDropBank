
using System.Data;
using BloodDropBank.Data;
using BloodDropBank.Models;
using BloodDropBank.Enums;
using Microsoft.EntityFrameworkCore;

namespace BloodDropBank.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly BloodDbContext _context;

        public HospitalRepository(BloodDbContext context)
        {
            _context = context;
        }

        // Hospital Account

        public async Task<IEnumerable<Hospital>> GetAllAsync()
        {
            return await _context.Hospitals.ToListAsync();
        }

        public async Task<Hospital?> GetByIdAsync(int id)
        {
            return await _context.Hospitals.FindAsync(id);
        }

        public async Task<Hospital?> GetByEmailAsync(string email)
        {
            return await _context.Hospitals
                .FirstOrDefaultAsync(h => h.Email == email);
        }

        public async Task<Hospital?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Hospitals
                .FirstOrDefaultAsync(h => h.PhoneNumber == phoneNumber);
        }

        public async Task<Hospital> AddAsync(Hospital hospital)
        {
            await _context.Hospitals.AddAsync(hospital);
            await _context.SaveChangesAsync();
            return hospital;
        }

        public async Task<Hospital?> UpdateAsync(int id, Hospital hospital)
        {
            var existingHospital = await _context.Hospitals.FindAsync(id);
            if (existingHospital == null) return null;

            existingHospital.HospitalName = hospital.HospitalName;
            existingHospital.City = hospital.City;
            existingHospital.Address = hospital.Address;
            existingHospital.PhoneNumber = hospital.PhoneNumber;
            existingHospital.Email = hospital.Email;

            if (!string.IsNullOrEmpty(hospital.Password))
                existingHospital.Password = hospital.Password;

            await _context.SaveChangesAsync();
            return existingHospital;
        }

        // Blood Inventory

        public async Task<IEnumerable<BloodInventory>> GetBloodInventoriesByHospitalIdAsync(int hospitalId)
        {
            return await _context.BloodInventories
                .Where(i => i.HospitalId == hospitalId)
                .ToListAsync();
        }

        public async Task<BloodInventory?> GetBloodInventoryByIdAsync(int id)
        {
            return await _context.BloodInventories.FindAsync(id);
        }

        public async Task<BloodInventory?> GetBloodInventoryByHospitalAndGroupAsync(
            int hospitalId, BloodGroup bloodGroup)
        {
            return await _context.BloodInventories
                .FirstOrDefaultAsync(i =>
                    i.HospitalId == hospitalId &&
                    i.BloodGroup == bloodGroup);
        }

        public async Task AddBloodInventoriesAsync(IEnumerable<BloodInventory> inventories)
        {
            await _context.BloodInventories.AddRangeAsync(inventories);
            await _context.SaveChangesAsync();
        }

        public async Task<BloodInventory?> UpdateBloodInventoryAsync(
            int id, BloodInventory inventory)
        {
            var existingInventory = await _context.BloodInventories.FindAsync(id);
            if (existingInventory == null) return null;

            existingInventory.UnitsAvailable = inventory.UnitsAvailable;
            existingInventory.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingInventory;
        }

        // Blood Requests

        public async Task<BloodRequest?> GetBloodRequestByIdAsync(int id)
        {
            return await _context.BloodRequests.FindAsync(id);
        }

        public async Task<IEnumerable<BloodRequest>> GetBloodRequestsByHospitalIdAsync(int hospitalId)
        {
            return await _context.BloodRequests
                .Where(r => r.HospitalId == hospitalId)
                .ToListAsync();
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

        public async Task<IEnumerable<UserDonation>> GetUserDonationsByHospitalIdAsync(int hospitalId)
        {
            return await _context.UserDonations
                .Where(d => d.HospitalId == hospitalId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDonation>> GetPendingDonationsByBloodRequestIdAsync(
            int bloodRequestId)
        {
            return await _context.UserDonations
                .Where(d => d.BloodRequestId == bloodRequestId &&
                    d.DonationStatus == DonationStatus.Pending)
                .ToListAsync();
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

        // Inventory Donations

        public async Task<InventoryDonation?> GetInventoryDonationByIdAsync(int id)
        {
            return await _context.InventoryDonations.FindAsync(id);
        }

        public async Task<IEnumerable<InventoryDonation>> GetInventoryDonationsByHospitalIdAsync(
            int hospitalId)
        {
            return await _context.InventoryDonations
                .Where(d => d.BloodInventory.HospitalId == hospitalId)
                .ToListAsync();
        }

        public async Task<InventoryDonation> AddInventoryDonationAsync(
            InventoryDonation donation)
        {
            await _context.InventoryDonations.AddAsync(donation);
            await _context.SaveChangesAsync();
            return donation;
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
