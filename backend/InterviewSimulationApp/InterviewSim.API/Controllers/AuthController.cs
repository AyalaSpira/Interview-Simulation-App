using InterviewSim.BLL.Interfaces;
using InterviewSim.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> Register([FromForm] UserDTO userDto)  // שינוי מ-FromBody ל-FromForm
        {
            // אם ה-DTO כולל קובץ קורות חיים, הוא יישלח
            var result = await _authService.RegisterUserAsync(userDto.Username, userDto.Password, userDto.Resume);
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
