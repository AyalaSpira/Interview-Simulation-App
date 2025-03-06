using InterviewSim.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register([FromBody] string username, string password)
        {
           // var result = await _authService.RegisterAsync(username, password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] string username, string password)
        {
          //  var token = await _authService.LoginAsync(username, password);
            return Ok();
        }
    }
}
