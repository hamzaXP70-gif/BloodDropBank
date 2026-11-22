
using BloodDropBank.Models;

namespace BloodDropBank.Repositories
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAllAsync();
        Task<Admin?> GetByIdAsync(int id);
        Task<Admin?> GetByEmailAsync(string email);
        Task<Admin?> GetByPhoneNumberAsync(string phoneNumber);
        Task<Admin> AddAsync(Admin admin);
        Task<Admin?> UpdateAsync(int id, Admin admin);
    }
}
