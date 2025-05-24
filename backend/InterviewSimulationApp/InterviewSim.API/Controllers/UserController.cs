using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InterviewSim.BLL.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly S3Service _s3Service;
    private readonly IUserService _userService;
    private readonly string _bucketName = "ayala-spira-testpnoren";

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

        var userId = GetCurrentUserId();
        var email = GetCurrentUserEmail();
        var user = await _userService.GetUserByIdAsync(int.Parse(userId));

        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (!string.IsNullOrEmpty(user.ResumePath))
        {
            await _s3Service.DeleteFileByUrlAsync(user.ResumePath, _bucketName);
        }

        var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
        user.ResumePath = resumeUrl;
        await _userService.UpdateUserAsync(user);

        return Ok(new { ResumePath = resumeUrl });
    }

    [HttpDelete("delete-resume")]
    public async Task<IActionResult> DeleteResume()
    {
        var userId = GetCurrentUserId();
        var email = GetCurrentUserEmail();
        var user = await _userService.GetUserByIdAsync(int.Parse( userId));

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
        var userId = GetCurrentUserId();
        var email = GetCurrentUserEmail();
        var user = await _userService.GetUserByIdAsync(int.Parse(userId));

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(new { UserId = user.UserId, Name = user.Name, ResumePath = user.ResumePath });
    }

    [HttpGet("all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? throw new UnauthorizedAccessException("User not authorized.");
    }

    private string GetCurrentUserEmail()
    {
        return User.FindFirst(ClaimTypes.Email)?.Value ?? throw new UnauthorizedAccessException("User email not found.");
    }
}
