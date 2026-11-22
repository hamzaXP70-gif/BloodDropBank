
using AutoMapper;
using BloodDropBank.DTOs;
using BloodDropBank.DTOs.Hospitals;
using BloodDropBank.DTOs.Users;
using BloodDropBank.Enums;
using BloodDropBank.Helpers;
using BloodDropBank.Models;
using BloodDropBank.Repositories;

namespace BloodDropBank.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IMapper _mapper;

        public HospitalService(IHospitalRepository hospitalRepository, IMapper mapper)
        {
            _hospitalRepository = hospitalRepository;
            _mapper = mapper;
        }

        // Hospital Account

        public async Task<IEnumerable<HospitalDto>> GetAllAsync()
        {
            var hospitals = await _hospitalRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<HospitalDto>>(hospitals);
        }

        public async Task<HospitalDto?> GetByIdAsync(int id)
        {
            var hospital = await _hospitalRepository.GetByIdAsync(id);
            return hospital == null ? null : _mapper.Map<HospitalDto>(hospital);
        }

        public async Task<HospitalDto> AddAsync(CreateHospitalDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.HospitalName) ||
                string.IsNullOrWhiteSpace(dto.City) ||
                string.IsNullOrWhiteSpace(dto.PhoneNumber) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Required hospital information is missing.");

            if (await _hospitalRepository.GetByEmailAsync(dto.Email) != null)
                throw new ArgumentException("Email already exists.");

            if (await _hospitalRepository.GetByPhoneNumberAsync(dto.PhoneNumber) != null)
                throw new ArgumentException("Phone number already exists.");

            BloodGroup[] groups =
            {
                BloodGroup.A_Pos, BloodGroup.A_Neg, BloodGroup.B_Pos, BloodGroup.B_Neg,
                BloodGroup.AB_Pos, BloodGroup.AB_Neg, BloodGroup.O_Pos, BloodGroup.O_Neg
            };

            int[] units =
            {
                dto.APosUnits, dto.ANegUnits, dto.BPosUnits, dto.BNegUnits,
                dto.ABPosUnits, dto.ABNegUnits, dto.OPosUnits, dto.ONegUnits
            };

            foreach (var quantity in units)
                if (quantity < 0 || quantity > 10000)
                    throw new ArgumentException("Inventory units must be between 0 and 10000.");

            var hospital = _mapper.Map<Hospital>(dto);
            hospital.CreatedAt = DateTime.UtcNow;

            return await _hospitalRepository.ExecuteInTransactionAsync(async () =>
            {
                var addedHospital = await _hospitalRepository.AddAsync(hospital);
                var inventories = new List<BloodInventory>();

                for (int i = 0; i < groups.Length; i++)
                {
                    inventories.Add(new BloodInventory
                    {
                        HospitalId = addedHospital.HospitalId,
                        BloodGroup = groups[i],
                        UnitsAvailable = units[i],
                        LastUpdated = DateTime.UtcNow
                    });
                }

                await _hospitalRepository.AddBloodInventoriesAsync(inventories);
                return _mapper.Map<HospitalDto>(addedHospital);
            });
        }

        public async Task<HospitalDto?> UpdateAsync(int id, UpdateHospitalDto dto)
        {
            var hospital = await _hospitalRepository.GetByIdAsync(id);
            if (hospital == null) return null;

            if (dto.HospitalName != null && string.IsNullOrWhiteSpace(dto.HospitalName))
                throw new ArgumentException("Invalid hospital name.");

            if (dto.City != null && string.IsNullOrWhiteSpace(dto.City))
                throw new ArgumentException("Invalid city.");

            if (dto.Address != null && string.IsNullOrWhiteSpace(dto.Address))
                throw new ArgumentException("Invalid address.");

            if (dto.Email != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Email))
                    throw new ArgumentException("Invalid email.");

                var existingEmail = await _hospitalRepository.GetByEmailAsync(dto.Email);
                if (existingEmail != null && existingEmail.HospitalId != id)
                    throw new ArgumentException("Email already exists.");
            }

            if (dto.PhoneNumber != null)
            {
                if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                    throw new ArgumentException("Invalid phone number.");

                var existingPhone = await _hospitalRepository.GetByPhoneNumberAsync(dto.PhoneNumber);
                if (existingPhone != null && existingPhone.HospitalId != id)
                    throw new ArgumentException("Phone number already exists.");
            }

            if (dto.HospitalName != null) hospital.HospitalName = dto.HospitalName;
            if (dto.City != null) hospital.City = dto.City;
            if (dto.Address != null) hospital.Address = dto.Address;
            if (dto.PhoneNumber != null) hospital.PhoneNumber = dto.PhoneNumber;
            if (dto.Email != null) hospital.Email = dto.Email;

            var updatedHospital = await _hospitalRepository.UpdateAsync(id, hospital);
            return _mapper.Map<HospitalDto>(updatedHospital);
        }

        public async Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto)
        {
            var hospital = await _hospitalRepository.GetByIdAsync(id);
            if (hospital == null) return false;

            if (hospital.Password != dto.CurrentPassword)
                throw new ArgumentException("Current password is incorrect.");

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
                throw new ArgumentException("New password is required.");

            hospital.Password = dto.NewPassword;
            await _hospitalRepository.UpdateAsync(id, hospital);
            return true;
        }

        // Blood Inventory

        public async Task<IEnumerable<BloodInventoryDto>> GetBloodInventoriesByHospitalIdAsync(int hospitalId)
        {
            if (await _hospitalRepository.GetByIdAsync(hospitalId) == null)
                throw new ArgumentException("Hospital not found.");

            var inventories = await _hospitalRepository.GetBloodInventoriesByHospitalIdAsync(hospitalId);
            return _mapper.Map<IEnumerable<BloodInventoryDto>>(inventories);
        }

        public async Task<BloodInventoryDto?> GetBloodInventoryByIdAsync(int hospitalId, int inventoryId)
        {
            var inventory = await _hospitalRepository.GetBloodInventoryByIdAsync(inventoryId);
            return inventory == null || inventory.HospitalId != hospitalId
                ? null : _mapper.Map<BloodInventoryDto>(inventory);
        }

        // Blood Requests

        public async Task<IEnumerable<BloodRequestDto>> GetBloodRequestsByHospitalIdAsync(int hospitalId)
        {
            if (await _hospitalRepository.GetByIdAsync(hospitalId) == null)
                throw new ArgumentException("Hospital not found.");

            var requests = await _hospitalRepository.GetBloodRequestsByHospitalIdAsync(hospitalId);
            return _mapper.Map<IEnumerable<BloodRequestDto>>(requests);
        }

        public async Task<BloodRequestDto?> GetBloodRequestByIdAsync(int hospitalId, int requestId)
        {
            var request = await _hospitalRepository.GetBloodRequestByIdAsync(requestId);
            return request == null || request.HospitalId != hospitalId
                ? null : _mapper.Map<BloodRequestDto>(request);
        }

        // User Donations

        public async Task<IEnumerable<UserDonationDto>> GetUserDonationsByHospitalIdAsync(int hospitalId)
        {
            if (await _hospitalRepository.GetByIdAsync(hospitalId) == null)
                throw new ArgumentException("Hospital not found.");

            var donations = await _hospitalRepository.GetUserDonationsByHospitalIdAsync(hospitalId);
            return _mapper.Map<IEnumerable<UserDonationDto>>(donations);
        }

        public async Task<UserDonationDto?> GetUserDonationByIdAsync(int hospitalId, int donationId)
        {
            var donation = await _hospitalRepository.GetUserDonationByIdAsync(donationId);
            return donation == null || donation.HospitalId != hospitalId
                ? null : _mapper.Map<UserDonationDto>(donation);
        }

        public async Task<UserDonationDto?> ApproveUserDonationAsync(int hospitalId, int donationId)
        {
            return await _hospitalRepository.ExecuteInTransactionAsync<UserDonationDto?>(async () =>
            {
                if (await _hospitalRepository.GetByIdAsync(hospitalId) == null)
                    throw new ArgumentException("Hospital not found.");

                var donation = await _hospitalRepository.GetUserDonationByIdAsync(donationId);
                if (donation == null) return null;

                if (donation.HospitalId != hospitalId)
                    throw new InvalidOperationException("Donation does not belong to this hospital.");

                if (donation.DonationStatus != DonationStatus.Pending)
                    throw new InvalidOperationException("Donation has already been processed.");

                if (donation.UnitDonated != 1)
                    throw new InvalidOperationException("Only one unit is allowed per user donation.");

                bool requestCompleted = false;
                int completedRequestId = 0;

                if (donation.UserDonationType == UserDonationType.ToInventory)
                {
                    if (donation.BloodRequestId != null)
                        throw new InvalidOperationException("Invalid inventory donation.");

                    var inventory = await _hospitalRepository.GetBloodInventoryByHospitalAndGroupAsync(
                        hospitalId, donation.BloodGroup);

                    if (inventory == null)
                        throw new ArgumentException("Blood inventory not found.");

                    if (inventory.UnitsAvailable + 1 > 10000)
                        throw new InvalidOperationException("Blood inventory capacity exceeded.");

                    inventory.UnitsAvailable += 1;
                    inventory.LastUpdated = DateTime.UtcNow;
                    await _hospitalRepository.UpdateBloodInventoryAsync(inventory.InventoryId, inventory);
                }
                else if (donation.UserDonationType == UserDonationType.ForBloodRequest)
                {
                    if (!donation.BloodRequestId.HasValue)
                        throw new ArgumentException("BloodRequestId is required.");

                    var request = await _hospitalRepository.GetBloodRequestByIdAsync(donation.BloodRequestId.Value);
                    if (request == null)
                        throw new ArgumentException("Blood request not found.");

                    if (request.HospitalId != hospitalId)
                        throw new InvalidOperationException("Blood request does not belong to this hospital.");

                    if (request.Status != RequestStatus.Pending &&
                        request.Status != RequestStatus.PartiallyFulfilled)
                        throw new InvalidOperationException("Blood request is not available.");

                    if (request.RemainingUnits < 1)
                        throw new InvalidOperationException("Blood request does not need more units.");

                    if (!BloodMedicalRulesHelper.CanDonateRedCellsTo(donation.BloodGroup, request.BloodGroup))
                        throw new InvalidOperationException("Blood groups are not compatible.");

                    request.RemainingUnits -= 1;
                    if (request.RemainingUnits == 0)
                    {
                        request.Status = RequestStatus.Completed;
                        request.CompletedAt = DateTime.UtcNow;
                        requestCompleted = true;
                        completedRequestId = request.BloodRequestId;
                    }
                    else
                    {
                        request.Status = RequestStatus.PartiallyFulfilled;
                        request.CompletedAt = null;
                    }

                    await _hospitalRepository.UpdateBloodRequestAsync(request.BloodRequestId, request);
                }
                else
                {
                    throw new ArgumentException("Invalid donation type.");
                }

                donation.DonationStatus = DonationStatus.Approved;
                donation.DonatedAt = DateTime.UtcNow;
                await _hospitalRepository.UpdateUserDonationAsync(donation.UserDonationId, donation);

                if (requestCompleted)
                {
                    var pending = await _hospitalRepository.GetPendingDonationsByBloodRequestIdAsync(completedRequestId);
                    foreach (var item in pending)
                    {
                        item.DonationStatus = DonationStatus.Rejected;
                        item.DonatedAt = null;
                        await _hospitalRepository.UpdateUserDonationAsync(item.UserDonationId, item);
                    }
                }

                return _mapper.Map<UserDonationDto>(donation);
            });
        }

        public async Task<UserDonationDto?> RejectUserDonationAsync(int hospitalId, int donationId)
        {
            return await _hospitalRepository.ExecuteInTransactionAsync<UserDonationDto?>(async () =>
            {
                if (await _hospitalRepository.GetByIdAsync(hospitalId) == null)
                    throw new ArgumentException("Hospital not found.");

                var donation = await _hospitalRepository.GetUserDonationByIdAsync(donationId);
                if (donation == null) return null;

                if (donation.HospitalId != hospitalId)
                    throw new InvalidOperationException("Donation does not belong to this hospital.");

                if (donation.DonationStatus != DonationStatus.Pending)
                    throw new InvalidOperationException("Donation has already been processed.");

                donation.DonationStatus = DonationStatus.Rejected;
                donation.DonatedAt = null;
                await _hospitalRepository.UpdateUserDonationAsync(donation.UserDonationId, donation);
                return _mapper.Map<UserDonationDto>(donation);
            });
        }

        // Inventory Donations

        public async Task<IEnumerable<InventoryDonationDto>> GetInventoryDonationsByHospitalIdAsync(int hospitalId)
        {
            if (await _hospitalRepository.GetByIdAsync(hospitalId) == null)
                throw new ArgumentException("Hospital not found.");

            var donations = await _hospitalRepository.GetInventoryDonationsByHospitalIdAsync(hospitalId);
            return _mapper.Map<IEnumerable<InventoryDonationDto>>(donations);
        }

        public async Task<InventoryDonationDto?> GetInventoryDonationByIdAsync(int hospitalId, int donationId)
        {
            var donation = await _hospitalRepository.GetInventoryDonationByIdAsync(donationId);
            if (donation == null) return null;

            var inventory = await _hospitalRepository.GetBloodInventoryByIdAsync(donation.BloodInventoryId);
            return inventory == null || inventory.HospitalId != hospitalId
                ? null : _mapper.Map<InventoryDonationDto>(donation);
        }

        public async Task<InventoryDonationDto> AddInventoryDonationAsync(CreateInventoryDonationDto dto)
        {
            return await _hospitalRepository.ExecuteInTransactionAsync(async () =>
            {
                if (await _hospitalRepository.GetByIdAsync(dto.HospitalId) == null)
                    throw new ArgumentException("Hospital not found.");

                var request = await _hospitalRepository.GetBloodRequestByIdAsync(dto.BloodRequestId);
                if (request == null)
                    throw new ArgumentException("Blood request not found.");

                var inventory = await _hospitalRepository.GetBloodInventoryByIdAsync(dto.BloodInventoryId);
                if (inventory == null)
                    throw new ArgumentException("Blood inventory not found.");

                if (request.HospitalId != dto.HospitalId || inventory.HospitalId != dto.HospitalId)
                    throw new InvalidOperationException("Request and inventory must belong to this hospital.");

                if (request.Status != RequestStatus.Pending &&
                    request.Status != RequestStatus.PartiallyFulfilled)
                    throw new InvalidOperationException("Blood request is not available.");

                if (dto.UnitsDonated < 1 || dto.UnitsDonated > 40)
                    throw new ArgumentException("Units donated must be between 1 and 40.");

                if (dto.UnitsDonated > request.RemainingUnits)
                    throw new InvalidOperationException("Units exceed remaining blood request units.");

                if (dto.UnitsDonated > inventory.UnitsAvailable)
                    throw new InvalidOperationException("Not enough blood units in inventory.");

                if (!BloodMedicalRulesHelper.CanDonateRedCellsTo(inventory.BloodGroup, request.BloodGroup))
                    throw new InvalidOperationException("Blood groups are not compatible.");

                var donation = _mapper.Map<InventoryDonation>(dto);
                donation.DonatedAt = DateTime.UtcNow;
                var addedDonation = await _hospitalRepository.AddInventoryDonationAsync(donation);

                inventory.UnitsAvailable -= dto.UnitsDonated;
                inventory.LastUpdated = DateTime.UtcNow;
                await _hospitalRepository.UpdateBloodInventoryAsync(inventory.InventoryId, inventory);

                request.RemainingUnits -= dto.UnitsDonated;
                if (request.RemainingUnits == 0)
                {
                    request.Status = RequestStatus.Completed;
                    request.CompletedAt = DateTime.UtcNow;
                }
                else
                {
                    request.Status = RequestStatus.PartiallyFulfilled;
                    request.CompletedAt = null;
                }

                await _hospitalRepository.UpdateBloodRequestAsync(request.BloodRequestId, request);

                if (request.Status == RequestStatus.Completed)
                {
                    var pending = await _hospitalRepository.GetPendingDonationsByBloodRequestIdAsync(request.BloodRequestId);
                    foreach (var item in pending)
                    {
                        item.DonationStatus = DonationStatus.Rejected;
                        item.DonatedAt = null;
                        await _hospitalRepository.UpdateUserDonationAsync(item.UserDonationId, item);
                    }
                }

                return _mapper.Map<InventoryDonationDto>(addedDonation);
            });
        }
    }
}
