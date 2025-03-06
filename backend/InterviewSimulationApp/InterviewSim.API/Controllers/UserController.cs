using InterviewSim.BLL.Implementations;
using InterviewSim.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace InterviewSim.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDetails(int userId)
        {
            var user = await _userService.GetUserDetailsAsync(userId);
            return Ok(user);
        }

        [HttpPost("upload-resume")]
        public async Task<IActionResult> UploadResume(int userId, IFormFile resume)
        {
            await _userService.UpdateUserResumeAsync(userId, resume);
            return Ok();
        }
    }
}