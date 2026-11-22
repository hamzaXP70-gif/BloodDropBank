
using BloodDropBank.Data;
using BloodDropBank.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodDropBank.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly BloodDbContext _context;

        public AdminRepository(BloodDbContext context)
        {
            _context = context;
        }

        // Admin Account

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            return await _context.Admins.ToListAsync();
        }

        public async Task<Admin?> GetByIdAsync(int id)
        {
            return await _context.Admins.FindAsync(id);
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Admin?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Admins
                .FirstOrDefaultAsync(a => a.PhoneNumber == phoneNumber);
        }

        public async Task<Admin> AddAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<Admin?> UpdateAsync(int id, Admin admin)
        {
            var existingAdmin = await _context.Admins.FindAsync(id);
            if (existingAdmin == null) return null;

            existingAdmin.FullName = admin.FullName;
            existingAdmin.PhoneNumber = admin.PhoneNumber;
            existingAdmin.Email = admin.Email;

            if (!string.IsNullOrEmpty(admin.Password))
                existingAdmin.Password = admin.Password;

            await _context.SaveChangesAsync();
            return existingAdmin;
        }
    }
}
