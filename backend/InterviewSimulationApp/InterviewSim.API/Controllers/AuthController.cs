using InterviewSim.BLL.Interfaces;
using InterviewSim.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Amazon.Util.Internal;

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
        public async Task<IActionResult> Register([FromForm] string username, [FromForm] string password, [FromForm] IFormFile resume)
        {
            if (resume == null)
            {
                return BadRequest("Resume file is required.");
            }

            // קריאה לפונקציה להירשם עם קובץ קורות חיים
            var result = await _authService.RegisterUserAsync(username, password, resume);

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDto)
        {
            // התחברות על פי שם משתמש וסיסמה
            var token = await _authService.LoginUserAsync(userDto.Username, userDto.Password);
            return Ok(new { Token = token });
        }
    }
}
