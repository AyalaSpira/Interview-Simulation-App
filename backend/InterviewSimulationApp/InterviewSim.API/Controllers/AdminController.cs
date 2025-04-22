using InterviewSim.BLL.Implementations;
using InterviewSim.BLL.Interfaces;
using InterviewSim.Shared.DTOs;
using InterviewSim.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using  InterviewSim.BLL.Implementations;

namespace InterviewSim.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly S3Service _s3Service;
        private readonly IUserService _userService;
        private readonly AdminService _AdminService;
        private readonly IInterviewService _interviewService;
        private readonly string _bucketName = "";

        public AdminController(S3Service s3Service, IUserService userService, IInterviewService interviewService, AdminService adminService)
        {
            _s3Service = s3Service;
            _userService = userService;
            _interviewService = interviewService;
            _AdminService = adminService;
        }

        [HttpDelete("delete-resume")]
        public async Task<IActionResult> DeleteResume()
        {
            var username = GetCurrentUserName();
            var password = GetCurrentUserPassword();
            var user = await _userService.GetUserByIdAsync(int.Parse(GetCurrentUserId()));

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!string.IsNullOrEmpty(user.ResumePath))
            {
                await _s3Service.DeleteFileByUrlAsync(user.ResumePath, _bucketName);
                user.ResumePath = null;
                await _userService.UpdateUserAsync(user);
            }

            return Ok(new { message = "Resume deleted successfully." });
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUserDetails()
        {
            var username = GetCurrentUserName();
            var password = GetCurrentUserPassword();
            var user = await _userService.GetUserByIdAsync(int.Parse(password));

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { Username = user.Name, ResumePath = user.ResumePath });
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [NonAction]

        public string GetCurrentUserName()
        {
            var userNameClaim = User.FindFirst(ClaimTypes.Name);
            if (userNameClaim == null)
            {
                throw new UnauthorizedAccessException("User not authorized.");
            }
            return userNameClaim.Value;
        }
        [NonAction]

        public string GetCurrentUserPassword()
        {
            var passwordClaim = User.FindFirst("Password");
            if (passwordClaim == null)
            {
                throw new UnauthorizedAccessException("User not authorized.");
            }
            return passwordClaim.Value;
        }
        [NonAction]

        public string GetCurrentUserId()
        {
            return User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault(v => int.TryParse(v, out _));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _AdminService.DeleteUserAsync(id);
            return result ? Ok() : NotFound("User not found");
        }

        [HttpGet("resumes")]
        public async Task<ActionResult<IEnumerable<string>>> GetResumes()
        {
            try
            {
                var resumeUrls = await _userService.GetAllResumeUrlsAsync();
                return Ok(resumeUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving resume URLs: {ex.Message}");
            }
        }

        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<string>>> GetReports()
        {
            try
            {
                var reportUrls = await _userService.GetAllReportUrlsAsync();
                return Ok(reportUrls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving report URLs: {ex.Message}");
            }
        }

        [HttpDelete("delete-file")]
        public async Task<IActionResult> DeleteFile([FromQuery] string fileUrl, [FromQuery] string fileType, [FromQuery] int? interviewId)
        {
            try
            {
                var success = await _s3Service.DeleteFileByUrlAsync(fileUrl, _bucketName);

                if (success)
                {
                    if (fileType == "resume")
                    {
                        await _userService.DeleteFileAsync(fileUrl, "resume");
                    }
                    else if (fileType == "report")
                    {
                        await _userService.DeleteFileAsync(fileUrl, "report");
                    }

                    return Ok($"{fileType} deleted successfully.");
                }

                return NotFound($"{fileType} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting {fileType}: {ex.Message}");
            }
        }

        [AllowAnonymous] // ✅ זה השינוי היחיד שצריך
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var token = await _AdminService.LoginAdminAsync(loginDto.Email, loginDto.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO userDto)
        {
            await _userService.AddUserAsync(userDto);
            return Ok("User created successfully");
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDto)
        {
            await _userService.UpdateUserByAdminAsync(userDto);
            return Ok("User updated successfully");
        }

    }
}
