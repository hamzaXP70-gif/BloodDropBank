
using BloodDropBank.DTOs;
using BloodDropBank.DTOs.Users;
using BloodDropBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BloodDropBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // User Account

        [HttpGet("{userId}/profile")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        [HttpPut("{userId}/profile")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromForm] UpdateUserDto dto)
        {
            try
            {
                var user = await _userService.UpdateAsync(userId, dto);
                if (user == null) return NotFound("User not found.");
                return Ok(user);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{userId}/change-password")]
        public async Task<IActionResult> ChangePassword(int userId, [FromForm] ChangePasswordDto dto)
        {
            try
            {
                var result = await _userService.ChangePasswordAsync(userId, dto);
                if (!result) return NotFound("User not found.");
                return Ok("Password changed successfully.");
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        // Blood Requests

        [HttpGet("{userId}/my-blood-requests")]
        public async Task<IActionResult> GetMyBloodRequests(int userId)
        {
            if (await _userService.GetByIdAsync(userId) == null)
                return NotFound("User not found.");

            var requests = await _userService.GetBloodRequestsByUserIdAsync(userId);
            return Ok(requests);
        }

        [HttpGet("{userId}/available-blood-requests")]
        public async Task<IActionResult> GetAvailableBloodRequests(int userId)
        {
            try
            {
                var requests = await _userService.GetAvailableBloodRequestsAsync(userId);
                return Ok(requests);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{userId}/blood-requests/{requestId}")]
        public async Task<IActionResult> GetBloodRequestDetails(int userId, int requestId)
        {
            if (await _userService.GetByIdAsync(userId) == null)
                return NotFound("User not found.");

            var request = await _userService.GetBloodRequestByIdAsync(requestId);
            if (request == null) return NotFound("Blood request not found.");

            if (request.UserId != userId)
            {
                var available = await _userService.GetAvailableBloodRequestsAsync(userId);

                if (!available.Any(r => r.BloodRequestId == requestId))
                    return NotFound("Blood request is not available.");
            }

            return Ok(request);
        }

        [HttpPost("blood-requests")]
        public async Task<IActionResult> CreateBloodRequest([FromForm] CreateBloodRequestDto dto)
        {
            try
            {
                var request = await _userService.AddBloodRequestAsync(dto);
                return Ok(request);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{userId}/blood-requests/{requestId}/cancel")]
        public async Task<IActionResult> CancelBloodRequest(int userId, int requestId)
        {
            try
            {
                var result = await _userService.CancelBloodRequestAsync(requestId, userId);
                if (!result) return NotFound("Blood request not found.");
                return Ok("Blood request cancelled successfully.");
            }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }

        // User Donations

        [HttpGet("{userId}/my-donations")]
        public async Task<IActionResult> GetMyDonations(int userId)
        {
            if (await _userService.GetByIdAsync(userId) == null)
                return NotFound("User not found.");

            var donations = await _userService.GetUserDonationsByUserIdAsync(userId);
            return Ok(donations);
        }

        [HttpGet("{userId}/donations/{donationId}")]
        public async Task<IActionResult> GetDonationDetails(int userId, int donationId)
        {
            var donation = await _userService.GetUserDonationByIdAsync(donationId);

            if (donation == null || donation.UserId != userId)
                return NotFound("Donation not found.");

            return Ok(donation);
        }

        [HttpPost("donations")]
        public async Task<IActionResult> CreateDonation([FromForm] CreateUserDonationDto dto)
        {
            try
            {
                var donation = await _userService.AddUserDonationAsync(dto);
                return Ok(donation);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
        }
    }
}
