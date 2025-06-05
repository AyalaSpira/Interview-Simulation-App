//using InterviewSim.BLL.Implementations;
//using InterviewSim.BLL.Interfaces;
//using InterviewSim.Shared.DTOs;
//using InterviewSim.Shared.Helpers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using  InterviewSim.BLL.Implementations;

//namespace InterviewSim.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize(Roles = "Admin")]
//    public class AdminController : ControllerBase
//    {
//        private readonly S3Service _s3Service;
//        private readonly IUserService _userService;
//        private readonly AdminService _AdminService;
//        private readonly IInterviewService _interviewService;
//        private readonly string _bucketName = "";

//        public AdminController(S3Service s3Service, IUserService userService, IInterviewService interviewService, AdminService adminService)
//        {
//            _s3Service = s3Service;
//            _userService = userService;
//            _interviewService = interviewService;
//            _AdminService = adminService;
//        }

//        [HttpDelete("delete-resume")]
//        public async Task<IActionResult> DeleteResume()
//        {
//            var username = GetCurrentUserName();
//            var password = GetCurrentUserPassword();
//            var user = await _userService.GetUserByIdAsync(int.Parse(GetCurrentUserId()));

//            if (user == null)
//            {
//                return NotFound("User not found.");
//            }

//            if (!string.IsNullOrEmpty(user.ResumePath))
//            {
//                await _s3Service.DeleteFileByUrlAsync(user.ResumePath, _bucketName);
//                user.ResumePath = null;
//                await _userService.UpdateUserAsync(user);
//            }

//            return Ok(new { message = "Resume deleted successfully." });
//        }

//        [HttpGet("get")]
//        public async Task<IActionResult> GetUserDetails()
//        {
//            var username = GetCurrentUserName();
//            var password = GetCurrentUserPassword();
//            var user = await _userService.GetUserByIdAsync(int.Parse(password));

//            if (user == null)
//            {
//                return NotFound("User not found.");
//            }

//            return Ok(new { Username = user.Name, ResumePath = user.ResumePath });
//        }

//        [HttpGet("all-users")]
//        public async Task<IActionResult> GetAllUsers()
//        {
//            var users = await _userService.GetAllUsersAsync();
//            return Ok(users);
//        }

//        [NonAction]

//        public string GetCurrentUserName()
//        {
//            var userNameClaim = User.FindFirst(ClaimTypes.Name);
//            if (userNameClaim == null)
//            {
//                throw new UnauthorizedAccessException("User not authorized.");
//            }
//            return userNameClaim.Value;
//        }
//        [NonAction]

//        public string GetCurrentUserPassword()
//        {
//            var passwordClaim = User.FindFirst("Password");
//            if (passwordClaim == null)
//            {
//                throw new UnauthorizedAccessException("User not authorized.");
//            }
//            return passwordClaim.Value;
//        }
//        [NonAction]

//        public string GetCurrentUserId()
//        {
//            return User.Claims
//                .Where(c => c.Type == ClaimTypes.NameIdentifier)
//                .Select(c => c.Value)
//                .FirstOrDefault(v => int.TryParse(v, out _));
//        }

//        [HttpDelete("delete/{id}")]
//        public async Task<IActionResult> DeleteUser(int id)
//        {
//            var result = await _AdminService.DeleteUserAsync(id);
//            return result ? Ok() : NotFound("User not found");
//        }

//        [HttpGet("resumes")]
//        public async Task<ActionResult<IEnumerable<string>>> GetResumes()
//        {
//            try
//            {
//                var resumeUrls = await _userService.GetAllResumeUrlsAsync();
//                return Ok(resumeUrls);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error retrieving resume URLs: {ex.Message}");
//            }
//        }

//        [HttpGet("reports")]
//        public async Task<ActionResult<IEnumerable<string>>> GetReports()
//        {
//            try
//            {
//                var reportUrls = await _userService.GetAllReportUrlsAsync();
//                return Ok(reportUrls);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error retrieving report URLs: {ex.Message}");
//            }
//        }

//        [HttpDelete("delete-file")]
//        public async Task<IActionResult> DeleteFile([FromQuery] string fileUrl, [FromQuery] string fileType, [FromQuery] int? interviewId)
//        {
//            try
//            {
//                var success = await _s3Service.DeleteFileByUrlAsync(fileUrl, _bucketName);

//                if (success)
//                {
//                    if (fileType == "resume")
//                    {
//                        await _userService.DeleteFileAsync(fileUrl, "resume");
//                    }
//                    else if (fileType == "report")
//                    {
//                        await _userService.DeleteFileAsync(fileUrl, "report");
//                    }

//                    return Ok($"{fileType} deleted successfully.");
//                }

//                return NotFound($"{fileType} not found.");
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Error deleting {fileType}: {ex.Message}");
//            }
//        }

//        [AllowAnonymous] // ✅ זה השינוי היחיד שצריך
//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
//        {
//            try
//            {
//                var token = await _AdminService.LoginAdminAsync(loginDto.Email, loginDto.Password);
//                return Ok(new { Token = token });
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(new { message = ex.Message });
//            }
//        }

//        [HttpPost("add-user")]
//        public async Task<IActionResult> AddUser([FromBody] UserDTO userDto)
//        {
//            await _userService.AddUserAsync(userDto);
//            return Ok("User created successfully");
//        }

//        [HttpPut("update-user")]
//        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDto)
//        {
//            await _userService.UpdateUserByAdminAsync(userDto);
//            return Ok("User updated successfully");
//        }

//        // פונקציה לשליפת הציון מהראיון האחרון של המשתמש
//        [HttpGet("GetLastInterviewScore/{userId}")]
//        public async Task<IActionResult> GetLastInterviewScore(int userId)
//        {
//            var score = await _interviewService.GetLastInterviewScoreAsync(userId);

//            if (score == null)
//            {
//                // Can return a more detailed message
//                return NotFound($"No last interview found for user {userId}, or score (MARK=) not found in summary.");
//            }

//            return Ok(score);
//        }
//    }

//}

using InterviewSim.BLL.Implementations;
using InterviewSim.BLL.Interfaces;
using InterviewSim.Shared.DTOs;
using InterviewSim.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // Added for IFormFile

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
        private readonly string _bucketName = "ayala-spira-testpnoren"; // Ensure your bucket name is set here

        public AdminController(S3Service s3Service, IUserService userService, IInterviewService interviewService, AdminService adminService)
        {
            _s3Service = s3Service;
            _userService = userService;
            _interviewService = interviewService;
            _AdminService = adminService;
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
                .FirstOrDefault(v => int.TryParse(v, out _)) ?? throw new UnauthorizedAccessException("User ID not found or invalid.");
        }

        [HttpDelete("delete-resume")]
        public async Task<IActionResult> DeleteResume()
        {
            var userIdString = GetCurrentUserId();
            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!string.IsNullOrEmpty(user.ResumePath))
            {
                var fileKey = user.ResumePath.Substring(user.ResumePath.LastIndexOf("/") + 1);
                var success = await _s3Service.DeleteFileByUrlAsync(fileKey, _bucketName);
                if (success)
                {
                    user.ResumePath = null;
                    await _userService.UpdateUserAsync(user);
                }
            }
            return Ok(new { message = "Resume deleted successfully." });
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userIdString = GetCurrentUserId();
            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _userService.GetUserByIdAsync(userId);

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
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (result)
                {
                    return NoContent(); 
                }
                else
                {
                    return NotFound("User not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
                var success = await _userService.DeleteFileAsync(fileUrl, fileType, interviewId);

                if (success)
                {
                    return Ok($"{fileType} deleted successfully.");
                }

                return NotFound($"{fileType} not found or could not be deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting {fileType}: {ex.Message}");
            }
        }

        [AllowAnonymous]
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser(
                   [FromForm] string name,
                   [FromForm] string email,
                   [FromForm] string password,
                   [FromForm] IFormFile? resumeFile)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new { message = "Name, email, and password are required." }); // החזרת JSON
            }

            try
            {
                await _userService.AddUserWithResumeAsync(name, email, password, resumeFile);
                return Ok(new { message = "User created successfully." }); // שינוי כאן: החזרת אובייקט JSON
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // החזרת JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" }); // החזרת JSON
            }
        }

        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(
            int userId,
            [FromForm] string name,
            [FromForm] string email,
            [FromForm] string? password = null,
            [FromForm] IFormFile? newResumeFile = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Name and email are required for update." });
            }

            try
            {
                await _userService.UpdateUserByAdminAsync(userId, name, email, password, newResumeFile);
                return Ok(new { message = "User updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("GetLastInterviewScore/{userId}")]
        public async Task<IActionResult> GetLastInterviewScore(int userId)
        {
            try
            {
                var score = await _interviewService.GetLastInterviewScoreAsync(userId);

                if (score == null)
                {
                    return NotFound($"No last interview found for user {userId}, or score (MARK=) not found in summary.");
                }

                return Ok(score);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}