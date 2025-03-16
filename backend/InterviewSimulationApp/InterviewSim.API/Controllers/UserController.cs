using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using InterviewSim.BLL.Interfaces;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly S3Service _s3Service;
    private readonly IUserService _userService;
    private readonly string _bucketName = ""; 

    public UserController(S3Service s3Service, IUserService userService)
    {
        _s3Service = s3Service;
        _userService = userService;
    }

    [HttpPost("upload-resume")]
    public async Task<IActionResult> UploadResume([FromForm] IFormFile resume)
    {
        if (resume == null || resume.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var username = GetCurrentUserName();
        var password = GetCurrentUserPassword();
        var user = await _userService.GetUserByIdAsync(password, username);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (!string.IsNullOrEmpty(user.ResumePath))
        {
            await _s3Service.DeleteFileAsync(user.ResumePath, _bucketName);
        }

        var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
        user.ResumePath = resumeUrl;
        await _userService.UpdateUserAsync(user);

        return Ok(new { ResumeUrl = resumeUrl });
    }

    [HttpPost("update-resume")]
    public async Task<IActionResult> UpdateResume([FromForm] IFormFile resume)
    {
        if (resume == null || resume.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var username = GetCurrentUserName();
        var password = GetCurrentUserPassword();
        var user = await _userService.GetUserByIdAsync(password, username);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (!string.IsNullOrEmpty(user.ResumePath))
        {
            await _s3Service.DeleteFileAsync(user.ResumePath, _bucketName);
        }

        var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
        user.ResumePath = resumeUrl;
        await _userService.UpdateUserAsync(user);

        return Ok(new { ResumeUrl = resumeUrl });
    }

    [HttpDelete("delete-resume")]
    public async Task<IActionResult> DeleteResume()
    {
        var username = GetCurrentUserName();
        var password = GetCurrentUserPassword();
        var user = await _userService.GetUserByIdAsync(password, username);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (!string.IsNullOrEmpty(user.ResumePath))
        {
            await _s3Service.DeleteFileAsync(user.ResumePath, _bucketName);
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
        var user = await _userService.GetUserByIdAsync(password, username);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(new { Username = user.Username, ResumePath = user.ResumePath });
    }

    [HttpGet("all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    public string GetCurrentUserName()
    {
        var userNameClaim = User.FindFirst(ClaimTypes.Name);
        if (userNameClaim == null)
        {
            throw new UnauthorizedAccessException("User not authorized.");
        }
        return userNameClaim.Value;
    }

    public string GetCurrentUserPassword()
    {
        var passwordClaim = User.FindFirst("Password");
        if (passwordClaim == null)
        {
            throw new UnauthorizedAccessException("User not authorized.");
        }
        return passwordClaim.Value;
    }

    public string GetCurrentUserId()
    {
        return User.Claims
     .Where(c => c.Type == ClaimTypes.NameIdentifier)
     .Select(c => c.Value)
     .FirstOrDefault(v => int.TryParse(v, out _));
    }

}
