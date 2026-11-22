
using AutoMapper;
using BloodDropBank.DTOs;
using BloodDropBank.DTOs.Admins;
using BloodDropBank.DTOs.Users;
using BloodDropBank.DTOs.Hospitals;
using BloodDropBank.Models;
using BloodDropBank.Repositories;

namespace BloodDropBank.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserService _userService;
        private readonly IHospitalService _hospitalService;
        private readonly IMapper _mapper;

        public AdminService(
            IAdminRepository adminRepository,
            IUserService userService,
            IHospitalService hospitalService,
            IMapper mapper)
        {
            _adminRepository = adminRepository;
            _userService = userService;
            _hospitalService = hospitalService;
            _mapper = mapper;
        }

        // Admin Account

        public async Task<IEnumerable<AdminDto>> GetAllAsync()
        {
            var admins = await _adminRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AdminDto>>(admins);
        }

        public async Task<AdminDto?> GetByIdAsync(int id)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            return admin == null ? null : _mapper.Map<AdminDto>(admin);
        }

        public async Task<AdminDto> AddAsync(CreateAdminDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName) ||
                string.IsNullOrWhiteSpace(dto.PhoneNumber) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Required admin information is missing.");

            if (await _adminRepository.GetByEmailAsync(dto.Email) != null)
                throw new ArgumentException("Email already exists.");

            if (await _adminRepository.GetByPhoneNumberAsync(dto.PhoneNumber) != null)
                throw new ArgumentException("Phone number already exists.");

            var admin = _mapper.Map<Admin>(dto);
            admin.CreatedAt = DateTime.UtcNow;

            var addedAdmin = await _adminRepository.AddAsync(admin);
            return _mapper.Map<AdminDto>(addedAdmin);
        }

        public async Task<AdminDto?> UpdateAsync(int id, UpdateAdminDto dto)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null) return null;

            if (dto.FullName != null)
            {
                if (string.IsNullOrWhiteSpace(dto.FullName))
                    throw new ArgumentException("Invalid full name.");

                admin.FullName = dto.FullName;
            }

            if (dto.PhoneNumber != null)
            {
                if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                    throw new ArgumentException("Invalid phone number.");

                var existingPhone = await _adminRepository.GetByPhoneNumberAsync(dto.PhoneNumber);

                if (existingPhone != null && existingPhone.AdminId != id)
                    throw new ArgumentException("Phone number already exists.");

                admin.PhoneNumber = dto.PhoneNumber;
            }

            if (dto.Email != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Email))
                    throw new ArgumentException("Invalid email.");

                var existingEmail = await _adminRepository.GetByEmailAsync(dto.Email);

                if (existingEmail != null && existingEmail.AdminId != id)
                    throw new ArgumentException("Email already exists.");

                admin.Email = dto.Email;
            }

            var updatedAdmin = await _adminRepository.UpdateAsync(id, admin);
            return _mapper.Map<AdminDto>(updatedAdmin);
        }

        public async Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null) return false;

            if (admin.Password != dto.CurrentPassword)
                throw new ArgumentException("Current password is incorrect.");

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
                throw new ArgumentException("New password is required.");

            admin.Password = dto.NewPassword;
            await _adminRepository.UpdateAsync(id, admin);
            return true;
        }

        // Users Management

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _userService.GetAllAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            return await _userService.GetByIdAsync(userId);
        }

        public async Task<UserDto> AddUserAsync(CreateUserDto dto)
        {
            return await _userService.AddAsync(dto);
        }

        // Hospitals Management

        public async Task<IEnumerable<HospitalDto>> GetAllHospitalsAsync()
        {
            return await _hospitalService.GetAllAsync();
        }

        public async Task<HospitalDto?> GetHospitalByIdAsync(int hospitalId)
        {
            return await _hospitalService.GetByIdAsync(hospitalId);
        }

        public async Task<HospitalDto> AddHospitalAsync(CreateHospitalDto dto)
        {
            return await _hospitalService.AddAsync(dto);
        }
    }
}
