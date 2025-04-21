using InterviewSim.BLL.Interfaces;
using InterviewSim.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using InterviewSim.DAL.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace InterviewSim.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] string username, [FromForm] string email, [FromForm] string password, [FromForm] IFormFile resume)
        {
            if (resume == null)
            {
                return BadRequest(new { message = "Resume file is required." });
            }

            var result = await _authService.RegisterUserAsync(username,email, password, resume);

            return Ok(new { message = "User Registered Successfully", success = true });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email or password is missing");
            }

            Console.WriteLine($"Attempting login for user: {request.Email}");

            // התחברות על פי שם משתמש וסיסמה
            var token = await _authService.LoginUserAsync(request.Email, request.Password);
            Console.WriteLine("token!!!!!!!!!!!!!!!!!!!!!!!!!"+token);
            Console.WriteLine($"Login success for user: {request.Email}");
            Console.WriteLine("token", token);

            return Ok(new { Token = token });
        }
        [HttpPost("upload-new-resume")]
        public async Task<IActionResult> UploadNewResume([FromForm] IFormFile resume)
        {
            try
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return Unauthorized(new { error = "Authorization header is missing." });
                }

                int userId = _authService.GetUserIdFromToken(Request);
                Console.WriteLine($"✅ Extracted User ID: {userId}");

                var result = await _authService.UploadNewResumeAsync(Request, resume);
                return result;
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        // פונקציה שמפענחת את הטוקן ומחזירה את מזהה המשתמש

    }
}
