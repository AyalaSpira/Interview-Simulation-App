//using InterviewSim.BLL.Interfaces;
//using InterviewSim.Shared.DTOs;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using InterviewSim.DAL.Entities;
//using Microsoft.AspNetCore.Identity.Data;

//namespace InterviewSim.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly IAuthService _authService;

//        public AuthController(IAuthService authService)
//        {
//            _authService = authService;

//        }

//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromForm] string username, [FromForm] string email, [FromForm] string password, [FromForm] IFormFile resume)
//        {
//            if (resume == null)
//            {
//                return BadRequest(new { message = "Resume file is required." });
//            }

//            var result = await _authService.RegisterUserAsync(username,email, password, resume);

//            return Ok(new { message = "User Registered Successfully", success = true });
//        }


//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginRequest request)
//        {
//            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
//            {
//                return BadRequest("Email or password is missing");
//            }

//            Console.WriteLine($"Attempting login for user: {request.Email}");

//            // התחברות על פי שם משתמש וסיסמה
//            var token = await _authService.LoginUserAsync(request.Email, request.Password);
//            Console.WriteLine("token!!!!!!!!!!!!!!!!!!!!!!!!!"+token);
//            Console.WriteLine($"Login success for user: {request.Email}");
//            Console.WriteLine("token", token);

//            return Ok(new { Token = token });
//        }
//        [HttpPost("upload-new-resume")]
//        public async Task<IActionResult> UploadNewResume([FromForm] IFormFile resume)
//        {
//            try
//            {
//                if (!Request.Headers.ContainsKey("Authorization"))
//                {
//                    return Unauthorized(new { error = "Authorization header is missing." });
//                }

//                int userId = _authService.GetUserIdFromToken(Request);
//                Console.WriteLine($"✅ Extracted User ID: {userId}");

//                var result = await _authService.UploadNewResumeAsync(Request, resume);
//                return result;
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(new { error = ex.Message });
//            }
//        }

//        // פונקציה שמפענחת את הטוקן ומחזירה את מזהה המשתמש

//    }
//}
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
// If using Microsoft.AspNetCore.Identity.Data, ensure it's imported correctly.
// Using it usually implies a different authentication flow. For now, assuming LoginRequest is your DTO.
// using Microsoft.AspNetCore.Identity.Data; // Consider if this is truly needed for your setup

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
                Console.WriteLine("Register: Resume file is required but missing.");
                return BadRequest(new { message = "Resume file is required." });
            }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Register: Missing username, email, or password in request.");
                return BadRequest(new { message = "Username, email, and password are required." });
            }

            try
            {
                var result = await _authService.RegisterUserAsync(username, email, password, resume);
                Console.WriteLine($"Register: User {email} registered successfully.");
                return Ok(new { message = result, success = true }); // Use result from service
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Register: Error during registration for {email}: {ex.Message}");
                // This will return an appropriate error message from the exception caught in AuthService.
                return BadRequest(new { message = ex.Message, success = false });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                Console.WriteLine("Login: Email or password missing from request.");
                return BadRequest("Email or password is missing");
            }

            Console.WriteLine($"Login: Attempting login for user: {request.Email}");

            // Call to AuthService
            var token = await _authService.LoginUserAsync(request.Email, request.Password);

            // *** התיקון נמצא כאן: בדיקה אם הטוקן הוא null ***
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine($"Login: Login failed for user: {request.Email} - Invalid credentials or token generation failure.");
                // החזרת Unauthorized (401) או BadRequest (400) עם הודעת שגיאה ברורה
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // אם הגענו לכאן, הטוקן אינו null, כלומר ההתחברות הצליחה
            Console.WriteLine($"Login: Successfully generated token for user: {request.Email}");
            Console.WriteLine($"Login: Token (truncated for log): {token.Substring(0, Math.Min(token.Length, 30))}..."); // הדפסת טוקן מקוצרת לביטחון

            return Ok(new { Token = token }); // החזרת טוקן עם סטטוס 200 OK
        }

        [HttpPost("upload-new-resume")]
        public async Task<IActionResult> UploadNewResume([FromForm] IFormFile resume)
        {
            try
            {
                if (resume == null)
                {
                    Console.WriteLine("UploadNewResume: No resume file provided.");
                    return BadRequest(new { error = "Resume file is required." });
                }

                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    Console.WriteLine("UploadNewResume: Authorization header is missing.");
                    return Unauthorized(new { error = "Authorization header is missing." });
                }

                int userId = _authService.GetUserIdFromToken(Request);
                Console.WriteLine($"UploadNewResume: ✅ Extracted User ID: {userId}");

                // Assuming UploadNewResumeAsync in AuthService returns IActionResult already
                var result = await _authService.UploadNewResumeAsync(Request, resume);
                return result;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"UploadNewResume: Unauthorized access attempt: {ex.Message}");
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UploadNewResume: An unexpected error occurred: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An internal server error occurred." });
            }
        }
        // Removed the commented-out function as it seems to be in AuthService now.
    }
}