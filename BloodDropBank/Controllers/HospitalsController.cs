
using BloodDropBank.DTOs;
using BloodDropBank.DTOs.Hospitals;
using BloodDropBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BloodDropBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly IHospitalService _hospitalService;

        public HospitalsController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        // Hospital Account

        [HttpGet("{hospitalId}/profile")]
        public async Task<IActionResult> GetProfile(int hospitalId)
        {
            var hospital = await _hospitalService.GetByIdAsync(hospitalId);
            if (hospital == null) return NotFound("Hospital not found.");
            return Ok(hospital);
        }

        [HttpPut("{hospitalId}/profile")]
        public async Task<IActionResult> UpdateProfile(int hospitalId, [FromForm] UpdateHospitalDto dto)
        {
            try
            {
                var hospital = await _hospitalService.UpdateAsync(hospitalId, dto);
                if (hospital == null) return NotFound("Hospital not found.");
                return Ok(hospital);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{hospitalId}/change-password")]
        public async Task<IActionResult> ChangePassword(int hospitalId, [FromForm] ChangePasswordDto dto)
        {
            try
            {
                var result = await _hospitalService.ChangePasswordAsync(hospitalId, dto);
                if (!result) return NotFound("Hospital not found.");
                return Ok("Password changed successfully.");
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        // Blood Inventory

        [HttpGet("{hospitalId}/blood-inventories")]
        public async Task<IActionResult> GetBloodInventories(int hospitalId)
        {
            try
            {
                var inventories = await _hospitalService.GetBloodInventoriesByHospitalIdAsync(hospitalId);
                return Ok(inventories);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{hospitalId}/blood-inventories/{inventoryId}")]
        public async Task<IActionResult> GetBloodInventoryDetails(int hospitalId, int inventoryId)
        {
            var inventory = await _hospitalService.GetBloodInventoryByIdAsync(hospitalId, inventoryId);
            if (inventory == null) return NotFound("Blood inventory not found.");
            return Ok(inventory);
        }

        // Blood Requests

        [HttpGet("{hospitalId}/blood-requests")]
        public async Task<IActionResult> GetBloodRequests(int hospitalId)
        {
            try
            {
                var requests = await _hospitalService.GetBloodRequestsByHospitalIdAsync(hospitalId);
                return Ok(requests);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{hospitalId}/blood-requests/{requestId}")]
        public async Task<IActionResult> GetBloodRequestDetails(int hospitalId, int requestId)
        {
            var request = await _hospitalService.GetBloodRequestByIdAsync(hospitalId, requestId);
            if (request == null) return NotFound("Blood request not found.");
            return Ok(request);
        }

        // User Donations

        [HttpGet("{hospitalId}/user-donations")]
        public async Task<IActionResult> GetUserDonations(int hospitalId)
        {
            try
            {
                var donations = await _hospitalService.GetUserDonationsByHospitalIdAsync(hospitalId);
                return Ok(donations);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{hospitalId}/user-donations/{donationId}")]
        public async Task<IActionResult> GetUserDonationDetails(int hospitalId, int donationId)
        {
            var donation = await _hospitalService.GetUserDonationByIdAsync(hospitalId, donationId);
            if (donation == null) return NotFound("Donation not found.");
            return Ok(donation);
        }

        [HttpPut("{hospitalId}/user-donations/{donationId}/approve")]
        public async Task<IActionResult> ApproveUserDonation(int hospitalId, int donationId)
        {
            try
            {
                var donation = await _hospitalService.ApproveUserDonationAsync(hospitalId, donationId);
                if (donation == null) return NotFound("Donation not found.");
                return Ok(donation);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{hospitalId}/user-donations/{donationId}/reject")]
        public async Task<IActionResult> RejectUserDonation(int hospitalId, int donationId)
        {
            try
            {
                var donation = await _hospitalService.RejectUserDonationAsync(hospitalId, donationId);
                if (donation == null) return NotFound("Donation not found.");
                return Ok(donation);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }

        // Inventory Donations

        [HttpGet("{hospitalId}/inventory-donations")]
        public async Task<IActionResult> GetInventoryDonations(int hospitalId)
        {
            try
            {
                var donations = await _hospitalService.GetInventoryDonationsByHospitalIdAsync(hospitalId);
                return Ok(donations);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{hospitalId}/inventory-donations/{donationId}")]
        public async Task<IActionResult> GetInventoryDonationDetails(int hospitalId, int donationId)
        {
            var donation = await _hospitalService.GetInventoryDonationByIdAsync(hospitalId, donationId);
            if (donation == null) return NotFound("Inventory donation not found.");
            return Ok(donation);
        }

        [HttpPost("inventory-donations")]
        public async Task<IActionResult> CreateInventoryDonation([FromForm] CreateInventoryDonationDto dto)
        {
            try
            {
                var donation = await _hospitalService.AddInventoryDonationAsync(dto);
                return Ok(donation);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }
    }
}
