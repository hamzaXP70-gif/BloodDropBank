
using AutoMapper;
using BloodDropBank.DTOs;
using BloodDropBank.DTOs.Users;
using BloodDropBank.Enums;
using BloodDropBank.Helpers;
using BloodDropBank.Models;
using BloodDropBank.Repositories;

namespace BloodDropBank.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        // User Account

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> AddAsync(CreateUserDto dto)
        {
            if (!BloodMedicalRulesHelper.IsValidBloodGroup(dto.BloodGroup))
                throw new ArgumentException("Invalid blood group.");

            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
                throw new ArgumentException("Email already exists.");

            if (await _userRepository.GetByPhoneNumberAsync(dto.PhoneNumber) != null)
                throw new ArgumentException("Phone number already exists.");

            if (await _userRepository.GetByNationalIdAsync(dto.NationalId) != null)
                throw new ArgumentException("National ID already exists.");

            var user = _mapper.Map<User>(dto);
            user.CreatedAt = DateTime.UtcNow;

            var addedUser = await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(addedUser);
        }

        public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            if (dto.FullName != null && string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Invalid full name.");

            if (dto.City != null && string.IsNullOrWhiteSpace(dto.City))
                throw new ArgumentException("Invalid city.");

            if (dto.BloodGroup.HasValue &&
                !BloodMedicalRulesHelper.IsValidBloodGroup(dto.BloodGroup.Value))
                throw new ArgumentException("Invalid blood group.");

            if (dto.Email != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Email))
                    throw new ArgumentException("Invalid email.");

                var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);

                if (existingEmail != null && existingEmail.UserId != id)
                    throw new ArgumentException("Email already exists.");
            }

            if (dto.PhoneNumber != null)
            {
                if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                    throw new ArgumentException("Invalid phone number.");

                var existingPhone = await _userRepository.GetByPhoneNumberAsync(dto.PhoneNumber);

                if (existingPhone != null && existingPhone.UserId != id)
                    throw new ArgumentException("Phone number already exists.");
            }

            if (dto.FullName != null) user.FullName = dto.FullName;
            if (dto.City != null) user.City = dto.City;
            if (dto.PhoneNumber != null) user.PhoneNumber = dto.PhoneNumber;
            if (dto.Email != null) user.Email = dto.Email;
            if (dto.BloodGroup.HasValue) user.BloodGroup = dto.BloodGroup.Value;
            if (dto.BirthDate.HasValue) user.BirthDate = dto.BirthDate.Value;
            if (dto.WeightKg.HasValue) user.WeightKg = dto.WeightKg.Value;

            var updatedUser = await _userRepository.UpdateAsync(id, user);
            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            if (user.Password != dto.CurrentPassword)
                throw new ArgumentException("Current password is incorrect.");

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
                throw new ArgumentException("New password is required.");

            user.Password = dto.NewPassword;
            await _userRepository.UpdateAsync(id, user);
            return true;
        }

        // Blood Requests

        public async Task<BloodRequestDto?> GetBloodRequestByIdAsync(int id)
        {
            var request = await _userRepository.GetBloodRequestByIdAsync(id);
            return request == null ? null : _mapper.Map<BloodRequestDto>(request);
        }

        public async Task<IEnumerable<BloodRequestDto>> GetBloodRequestsByUserIdAsync(int userId)
        {
            var requests = await _userRepository.GetBloodRequestsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<BloodRequestDto>>(requests);
        }

        public async Task<IEnumerable<BloodRequestDto>> GetAvailableBloodRequestsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new ArgumentException("User not found.");

            var groups = BloodMedicalRulesHelper.GetCompatibleRecipientGroups(user.BloodGroup);
            var requests = await _userRepository.GetAvailableBloodRequestsAsync(userId, groups);

            return _mapper.Map<IEnumerable<BloodRequestDto>>(requests);
        }

        public async Task<BloodRequestDto> AddBloodRequestAsync(CreateBloodRequestDto dto)
        {
            if (await _userRepository.GetByIdAsync(dto.UserId) == null)
                throw new ArgumentException("User not found.");

            if (await _hospitalRepository.GetByIdAsync(dto.HospitalId) == null)
                throw new ArgumentException("Hospital not found.");

            if (!BloodMedicalRulesHelper.IsValidBloodGroup(dto.BloodGroup))
                throw new ArgumentException("Invalid blood group.");

            if (dto.UnitsNeeded < 1 || dto.UnitsNeeded > 40)
                throw new ArgumentException("Units needed must be between 1 and 40.");

            var request = _mapper.Map<BloodRequest>(dto);
            request.Status = RequestStatus.Pending;
            request.RemainingUnits = dto.UnitsNeeded;
            request.CreatedAt = DateTime.UtcNow;
            request.CompletedAt = null;

            var addedRequest = await _userRepository.AddBloodRequestAsync(request);
            return _mapper.Map<BloodRequestDto>(addedRequest);
        }

        public async Task<bool> CancelBloodRequestAsync(int id, int userId)
        {
            return await _userRepository.ExecuteInTransactionAsync(async () =>
            {
                var request = await _userRepository.GetBloodRequestByIdAsync(id);
                if (request == null) return false;

                if (request.UserId != userId)
                    throw new InvalidOperationException("This blood request does not belong to the user.");

                if (request.Status != RequestStatus.Pending &&
                    request.Status != RequestStatus.PartiallyFulfilled)
                    throw new InvalidOperationException("This blood request cannot be cancelled.");

                request.Status = RequestStatus.Cancelled;
                request.CompletedAt = null;

                await _userRepository.UpdateBloodRequestAsync(id, request);

                var pendingDonations = await _userRepository.GetPendingDonationsByBloodRequestIdAsync(id);

                foreach (var donation in pendingDonations)
                {
                    donation.DonationStatus = DonationStatus.Rejected;
                    donation.DonatedAt = null;

                    await _userRepository.UpdateUserDonationAsync(donation.UserDonationId, donation);
                }

                return true;
            });
        }

        // User Donations

        public async Task<UserDonationDto?> GetUserDonationByIdAsync(int id)
        {
            var donation = await _userRepository.GetUserDonationByIdAsync(id);
            return donation == null ? null : _mapper.Map<UserDonationDto>(donation);
        }

        public async Task<IEnumerable<UserDonationDto>> GetUserDonationsByUserIdAsync(int userId)
        {
            var donations = await _userRepository.GetUserDonationsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<UserDonationDto>>(donations);
        }

        public async Task<UserDonationDto> AddUserDonationAsync(CreateUserDonationDto dto)
        {
            return await _userRepository.ExecuteInTransactionAsync(async () =>
            {
                var user = await _userRepository.GetByIdAsync(dto.UserId);

                if (user == null)
                    throw new ArgumentException("User not found.");

                if (await _hospitalRepository.GetByIdAsync(dto.HospitalId) == null)
                    throw new ArgumentException("Hospital not found.");

                if (!BloodMedicalRulesHelper.IsValidAge(user.BirthDate))
                    throw new ArgumentException("User age must be between 18 and 65.");

                if (!BloodMedicalRulesHelper.IsValidWeight(user.WeightKg))
                    throw new ArgumentException("User weight must be between 50 and 250 kg.");

                if (!BloodMedicalRulesHelper.IsValidBloodGroup(user.BloodGroup))
                    throw new ArgumentException("Invalid blood group.");

                if (await _userRepository.HasPendingDonationAsync(dto.UserId))
                    throw new InvalidOperationException("User already has a pending donation.");

                var lastDate = await _userRepository.GetLastApprovedDonationDateAsync(dto.UserId);

                if (!BloodMedicalRulesHelper.IsValidDonationInterval(lastDate))
                    throw new InvalidOperationException("At least 60 days must pass between donations.");

                if (dto.UserDonationType != UserDonationType.ToInventory &&
                    dto.UserDonationType != UserDonationType.ForBloodRequest)
                    throw new ArgumentException("Invalid donation type.");

                if (dto.UserDonationType == UserDonationType.ToInventory)
                {
                    if (dto.BloodRequestId != null)
                        throw new ArgumentException("BloodRequestId must be null for inventory donation.");
                }
                else
                {
                    if (dto.BloodRequestId == null)
                        throw new ArgumentException("BloodRequestId is required.");

                    var request = await _userRepository.GetBloodRequestByIdAsync(dto.BloodRequestId.Value);

                    if (request == null)
                        throw new ArgumentException("Blood request not found.");

                    if (request.UserId == dto.UserId)
                        throw new InvalidOperationException("User cannot donate to their own blood request.");

                    if (request.HospitalId != dto.HospitalId)
                        throw new InvalidOperationException("Hospital does not match the blood request.");

                    if (request.Status != RequestStatus.Pending &&
                        request.Status != RequestStatus.PartiallyFulfilled)
                        throw new InvalidOperationException("Blood request is not available.");

                    if (request.RemainingUnits <= 0)
                        throw new InvalidOperationException("Blood request does not need more units.");

                    if (!BloodMedicalRulesHelper.CanDonateRedCellsTo(user.BloodGroup, request.BloodGroup))
                        throw new InvalidOperationException("Blood groups are not compatible.");
                }

                var donation = _mapper.Map<UserDonation>(dto);
                donation.BloodGroup = user.BloodGroup;
                donation.UnitDonated = 1;
                donation.DonationStatus = DonationStatus.Pending;
                donation.CreatedAt = DateTime.UtcNow;
                donation.DonatedAt = null;

                var addedDonation = await _userRepository.AddUserDonationAsync(donation);
                return _mapper.Map<UserDonationDto>(addedDonation);
            });
        }
    }
}
