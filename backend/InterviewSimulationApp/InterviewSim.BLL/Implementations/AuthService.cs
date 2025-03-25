using InterviewSim.BLL.Helpers;
using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using InterviewSim.Shared.DTOs;
using InterviewSim.Shared.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly S3Service _s3Service;

    public AuthService(IUserRepository userRepository, S3Service s3Service)
    {
        _userRepository = userRepository;
        _s3Service = s3Service;
    }

    public async Task<string> RegisterUserAsync(string username, string password, string email, IFormFile resume)
    {
        var existingUserDTO = await _userRepository.GetUserByEmailAsync(email);
        if (existingUserDTO != null)
        {
            throw new Exception("Email already exists.");
        }

        var hashedPassword = PasswordHelper.HashPassword(password);
        string resumePath = null;
        if (resume != null)
        {
            resumePath = await _s3Service.UploadFileAsync(resume, "ayala-spira-testpnoren");
        }

        // המרה מ-UserDTO ל-User
        var newUser = new User
        {
            Username = username,
            Password = hashedPassword,
            Email = email,
            ResumePath = resumePath
        };

        // הוספת המשתמש החדש למאגר הנתונים
        await _userRepository.AddUserAsync(newUser);
        Console.WriteLine("User Registered Successfully");
        return "User Registered Successfully";
    }

    public async Task<string> LoginUserAsync(string email, string password)
    {
        Console.WriteLine($"Attempting to login with email: {email}, password: {password}");

        // ודא שהאימייל שנכנס לא מכיל רווחים מיותרים או טעויות כתיב
        email = email.Trim();  // חיתוך רווחים מיותרים

        // חפש את המשתמש על פי המייל
        var user = await _userRepository.GetUserEntityByEmailAsync(email);
        if (user == null)
        {
            Console.WriteLine("User not found in database");
            return "Invalid email or password";
        }

        Console.WriteLine($"User found: {user.Username}");
        Console.WriteLine(user.Password,"password in service");
        Console.WriteLine($"Password in service (hashed): {user.Password}");  // הצגת הסיסמה המוצפנת

      
        // ודא שהסיסמה תואמת את הסיסמה המוצפנת
        //if (!PasswordHelper.VerifyPassword(password, user.Password))
        //{
        //    Console.WriteLine("Password does not match");
        //    return "Invalid email or password";
        //}

        // יצירת UserDTO
        var userDto = new UserDTO
        {
            UserId = user.UserId,
            ResumePath = user.ResumePath,
            Email = user.Email,
            Name = user.Username
        };

        // הגenerate טוקן חדש
        var token = JwtHelper.GenerateJwtToken(user.UserId, user.Username, email);
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Token generation failed");
            return "Token generation failed";
        }

        Console.WriteLine($"🔑 Generated Token: {token}");
        return token;
    }

    public async Task<IActionResult> UploadNewResumeAsync(HttpRequest request, IFormFile resume)
    {
        try
        {
            int userId = GetUserIdFromToken(request);

            // מביא את המשתמש כ-UserDTO
            var userDTO = await _userRepository.GetUserByIdAsync(userId);
            if (userDTO == null)
            {
                return new NotFoundObjectResult(new { error = "User not found." });
            }

            // העלאת קובץ ל-S3
            var resumeUrl = await _s3Service.UploadFileAsync(resume, "ayala-spira-testpnoren");

            // המרת UserDTO חזרה ל-User לעדכון
            userDTO.ResumePath = resumeUrl;

            // עדכון במסד הנתונים עם ה-UserDTO
            await _userRepository.UpdateUserAsync(userDTO);

            return new OkObjectResult(new { message = "Resume uploaded successfully", resumeUrl });
        }
        catch (UnauthorizedAccessException ex)
        {
            return new UnauthorizedObjectResult(new { error = ex.Message });
        }
    }

    public int GetUserIdFromToken(HttpRequest request)
    {
        var tokenHeader = request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(tokenHeader) || !tokenHeader.StartsWith("Bearer "))
        {
            throw new UnauthorizedAccessException("Token is missing or invalid");
        }

        var token = tokenHeader.Replace("Bearer ", "").Trim();
        Console.WriteLine($"🔍 Received Token: {token}");

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ??
                          jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        return int.Parse(userIdClaim);
    }

}
