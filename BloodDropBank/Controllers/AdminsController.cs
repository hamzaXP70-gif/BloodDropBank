
using BloodDropBank.DTOs;
using BloodDropBank.DTOs.Admins;
using BloodDropBank.DTOs.Users;
using BloodDropBank.DTOs.Hospitals;
using BloodDropBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BloodDropBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminsController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // Admin Account

        [HttpGet]
        public async Task<IActionResult> GetAllAdmins()
        {
            return Ok(await _adminService.GetAllAsync());
        }

        [HttpGet("{adminId}/profile")]
        public async Task<IActionResult> GetAdminProfile(int adminId)
        {
            var admin = await _adminService.GetByIdAsync(adminId);
            if (admin == null) return NotFound("Admin not found.");

            return Ok(admin);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromForm] CreateAdminDto dto)
        {
            try
            {
                var admin = await _adminService.AddAsync(dto);
                return Ok(admin);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{adminId}/profile")]
        public async Task<IActionResult> UpdateAdminProfile(
            int adminId, [FromForm] UpdateAdminDto dto)
        {
            try
            {
                var admin = await _adminService.UpdateAsync(adminId, dto);
                if (admin == null) return NotFound("Admin not found.");

                return Ok(admin);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{adminId}/change-password")]
        public async Task<IActionResult> ChangeAdminPassword(
            int adminId, [FromForm] ChangePasswordDto dto)
        {
            try
            {
                var result = await _adminService.ChangePasswordAsync(adminId, dto);
                if (!result) return NotFound("Admin not found.");

                return Ok("Password changed successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Users Management

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _adminService.GetAllUsersAsync());
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _adminService.GetUserByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDto dto)
        {
            try
            {
                var user = await _adminService.AddUserAsync(dto);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Hospitals Management

        [HttpGet("hospitals")]
        public async Task<IActionResult> GetAllHospitals()
        {
            return Ok(await _adminService.GetAllHospitalsAsync());
        }

        [HttpGet("hospitals/{hospitalId}")]
        public async Task<IActionResult> GetHospitalById(int hospitalId)
        {
            var hospital = await _adminService.GetHospitalByIdAsync(hospitalId);
            if (hospital == null) return NotFound("Hospital not found.");

            return Ok(hospital);
        }

        [HttpPost("hospitals")]
        public async Task<IActionResult> CreateHospital([FromForm] CreateHospitalDto dto)
        {
            try
            {
                var hospital = await _adminService.AddHospitalAsync(dto);
                return Ok(hospital);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
