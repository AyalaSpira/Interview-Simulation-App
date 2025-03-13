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

    // יש להוסיף את שם ה-Bucket שברצונך לעבוד איתו.
    private readonly string _bucketName = ""; // שנה לפי הצורך

    public UserController(S3Service s3Service, IUserService userService)
    {
        _s3Service = s3Service;
        _userService = userService;
    }

    // API להעלאת רזומה חדש
    [HttpPost("upload-resume")]
    public async Task<IActionResult> UploadResume([FromForm] IFormFile resume)
    {
        if (resume == null || resume.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var username = GetCurrentUserName(); // הפונקציה שמחזירה את שם המשתמש הנוכחי
        var password = GetCurrentUserPassword(); // הפונקציה שמחזירה את הסיסמה
        var user = await _userService.GetUserByIdAsync(password, username);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // מחיקת רזומה ישן אם קיים
        if (!string.IsNullOrEmpty(user.ResumePath))
        {
            // הוספת שם ה-Bucket
            await _s3Service.DeleteFileAsync(user.ResumePath, _bucketName);
        }

        // העלאת קובץ חדש ל-S3 עם שם ה-Bucket
        var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);

        // עדכון הנתיב במשתמש
        user.ResumePath = resumeUrl;
        await _userService.UpdateUserAsync(user);

        return Ok(new { ResumeUrl = resumeUrl });
    }

    // API לעדכון רזומה קיים
    [HttpPost("update-resume")]
    public async Task<IActionResult> UpdateResume([FromForm] IFormFile resume)
    {
        if (resume == null || resume.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var username = GetCurrentUserName(); // הפונקציה שמחזירה את שם המשתמש הנוכחי
        var password = GetCurrentUserPassword(); // הפונקציה שמחזירה את הסיסמה
        var user = await _userService.GetUserByIdAsync(password, username);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // מחיקת רזומה ישן אם קיים
        if (!string.IsNullOrEmpty(user.ResumePath))
        {
            // הוספת שם ה-Bucket
            await _s3Service.DeleteFileAsync(user.ResumePath, _bucketName);
        }

        // העלאת קובץ חדש ל-S3 עם שם ה-Bucket
        var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);

        // עדכון הנתיב במשתמש
        user.ResumePath = resumeUrl;
        await _userService.UpdateUserAsync(user);

        return Ok(new { ResumeUrl = resumeUrl });
    }

    // API למחיקת רזומה
    [HttpDelete("delete-resume")]
    public async Task<IActionResult> DeleteResume()
    {
        var username = GetCurrentUserName(); // הפונקציה שמחזירה את שם המשתמש הנוכחי
        var password = GetCurrentUserPassword(); // הפונקציה שמחזירה את הסיסמה
        var user = await _userService.GetUserByIdAsync(password, username);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // מחיקת קובץ הרזומה ב-S3
        if (!string.IsNullOrEmpty(user.ResumePath))
        {
            // הוספת שם ה-Bucket
            await _s3Service.DeleteFileAsync(user.ResumePath, _bucketName);
            user.ResumePath = null; // לא לשמור על הנתיב של הרזומה
            await _userService.UpdateUserAsync(user);
        }

        return Ok(new { message = "Resume deleted successfully." });
    }

    // API לקבלת פרטי המשתמש
    [HttpGet("get")]
    public async Task<IActionResult> GetUserDetails()
    {
        var username = GetCurrentUserName(); // הפונקציה שמחזירה את שם המשתמש הנוכחי
        var password = GetCurrentUserPassword(); // הפונקציה שמחזירה את הסיסמה
        var user = await _userService.GetUserByIdAsync(password, username);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(new { Username = user.Username, ResumePath = user.ResumePath });
    }

    // API לקבלת כל המשתמשים (למנהל)
    [HttpGet("all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    public string GetCurrentUserName()
    {
        // לשלוף את שם המשתמש מתוך ה-Claims של הטוקן (אם אתה משתמש ב-JWT)
        var userNameClaim = User.FindFirst(ClaimTypes.Name);

        if (userNameClaim == null)
        {
            // אם לא מצאנו את שם המשתמש בטוקן, נחזיר שגיאה או שם ברירת מחדל
            throw new UnauthorizedAccessException("User not authorized.");
        }

        return userNameClaim.Value;
    }

    public string GetCurrentUserPassword()
    {
        // לשלוף את הסיסמה מתוך ה-Claims של הטוקן (אם יש טוקן שמחזיק את הסיסמה)
        var passwordClaim = User.FindFirst("Password");

        if (passwordClaim == null)
        {
            // אם לא מצאנו את הסיסמה בטוקן, נחזיר שגיאה או סיסמה ברירת מחדל
            throw new UnauthorizedAccessException("User not authorized.");
        }

        return passwordClaim.Value;
    }
}
