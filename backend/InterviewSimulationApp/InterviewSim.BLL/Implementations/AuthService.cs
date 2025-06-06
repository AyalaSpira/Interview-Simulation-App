//using InterviewSim.BLL.Helpers;
//using InterviewSim.BLL.Interfaces;
//using InterviewSim.DAL.Entities;
//using InterviewSim.Shared.DTOs;
//using InterviewSim.Shared.Helpers;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;

//public class AuthService : IAuthService
//{
//    private readonly IUserRepository _userRepository;
//    private readonly S3Service _s3Service;

//    public AuthService(IUserRepository userRepository, S3Service s3Service)
//    {
//        _userRepository = userRepository;
//        _s3Service = s3Service;
//    }

//    public async Task<string> RegisterUserAsync(string username, string password, string email, IFormFile resume)
//    {
//        var existingUserDTO = await _userRepository.GetUserByEmailAsync(email);
//        if (existingUserDTO != null)
//        {
//            throw new Exception("Email already exists.");
//        }

//        var hashedPassword = PasswordHelper.HashPassword(password);
//        string resumePath = null;
//        if (resume != null)
//        {
//            resumePath = await _s3Service.UploadFileAsync(resume, "ayala-spira-testpnoren");
//        }

//        // המרה מ-UserDTO ל-User
//        var newUser = new User
//        {
//            Username = username,
//            Password = hashedPassword,
//            Email = email,
//            ResumePath = resumePath
//        };

//        // הוספת המשתמש החדש למאגר הנתונים
//        await _userRepository.AddUserAsync(newUser);
//        Console.WriteLine("User Registered Successfully");
//        return "User Registered Successfully";
//    }

//    public async Task<string> LoginUserAsync(string email, string password)
//    {
//        Console.WriteLine($"Attempting to login with email: {email}, password: {password}");

//        // ודא שהאימייל שנכנס לא מכיל רווחים מיותרים או טעויות כתיב
//        email = email.Trim();  // חיתוך רווחים מיותרים

//        // חפש את המשתמש על פי המייל
//        var user = await _userRepository.GetUserEntityByEmailAsync(email);
//        if (user == null)
//        {
//            Console.WriteLine("User not found in database");
//        }

//        Console.WriteLine($"User found: {user.Username}");
//        Console.WriteLine(user.Password,"password in service");
//        Console.WriteLine($"Password in service (hashed): {user.Password}");  // הצגת הסיסמה המוצפנת


//        // ודא שהסיסמה תואמת את הסיסמה המוצפנת
//        //if (!PasswordHelper.VerifyPassword(password, user.Password))
//        //{
//        //    Console.WriteLine("Password does not match");
//        //    return "Invalid email or password";
//        //}

//        // יצירת UserDTO
//        var userDto = new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Username
//        };

//        // הגenerate טוקן חדש
//        var token = JwtHelper.GenerateJwtToken(user.UserId, user.Username, email);
//        if (string.IsNullOrEmpty(token))
//        {
//            Console.WriteLine("Token generation failed");
//            return "Token generation failed";
//        }

//        Console.WriteLine($"🔑 Generated Token: {token}");
//        return token;
//    }

//    public async Task<IActionResult> UploadNewResumeAsync(HttpRequest request, IFormFile resume)
//    {
//        try
//        {
//            int userId = GetUserIdFromToken(request);

//            // מביא את המשתמש כ-UserDTO
//            var userDTO = await _userRepository.GetUserByIdAsync(userId);
//            if (userDTO == null)
//            {
//                return new NotFoundObjectResult(new { error = "User not found." });
//            }

//            // העלאת קובץ ל-S3
//            var resumeUrl = await _s3Service.UploadFileAsync(resume, "ayala-spira-testpnoren");

//            // המרת UserDTO חזרה ל-User לעדכון
//            userDTO.ResumePath = resumeUrl;

//            // עדכון במסד הנתונים עם ה-UserDTO
//            await _userRepository.UpdateUserAsync(userDTO);

//            return new OkObjectResult(new { message = "Resume uploaded successfully", resumeUrl });
//        }
//        catch (UnauthorizedAccessException ex)
//        {
//            return new UnauthorizedObjectResult(new { error = ex.Message });
//        }
//    }

//    public int GetUserIdFromToken(HttpRequest request)
//    {
//        var tokenHeader = request.Headers["Authorization"].ToString();
//        if (string.IsNullOrEmpty(tokenHeader) || !tokenHeader.StartsWith("Bearer "))
//        {
//            throw new UnauthorizedAccessException("Token is missing or invalid");
//        }

//        var token = tokenHeader.Replace("Bearer ", "").Trim();
//        Console.WriteLine($"🔍 Received Token: {token}");

//        var handler = new JwtSecurityTokenHandler();
//        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
//        var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;





//        if (string.IsNullOrEmpty(userIdClaim))
//        {
//            throw new UnauthorizedAccessException("User ID not found in token");
//        }

//        return int.Parse(userIdClaim);
//    }

//}
using InterviewSim.BLL.Helpers;
using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using InterviewSim.Shared.DTOs;
using InterviewSim.Shared.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // Required for IActionResult
using System; // For Exception
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks; // Explicitly add for Task<string>

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly S3Service _s3Service;

    public AuthService(IUserRepository userRepository, S3Service s3Service)
    {
        _userRepository = userRepository;
        _s3Service = s3Service;
    }

    public async Task<string> RegisterUserAsync(string username, string email, string password, IFormFile resume)
    {
        Console.WriteLine($"RegisterUserAsync: Attempting to register user with email: {email}");
        var existingUserDTO = await _userRepository.GetUserByEmailAsync(email);
        if (existingUserDTO != null)
        {
            Console.WriteLine($"RegisterUserAsync: Email {email} already exists. Registration failed.");
            throw new Exception("Email already exists.");
        }

        var hashedPassword = PasswordHelper.HashPassword(password);
        string resumePath = null;
        if (resume != null)
        {
            try
            {
                resumePath = await _s3Service.UploadFileAsync(resume, "ayala-spira-testpnoren");
                Console.WriteLine($"RegisterUserAsync: Resume uploaded to S3: {resumePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RegisterUserAsync: Error uploading resume to S3: {ex.Message}");
                // Optionally rethrow or handle more gracefully
                throw new Exception("Error uploading resume. " + ex.Message);
            }
        }

        var newUser = new User
        {
            Username = username,
            Password = hashedPassword,
            Email = email,
            ResumePath = resumePath
        };

        await _userRepository.AddUserAsync(newUser);
        Console.WriteLine("User Registered Successfully");
        return "User Registered Successfully";
    }

    public async Task<string> LoginUserAsync(string email, string password)
    {
        Console.WriteLine($"LoginUserAsync: Attempting to login with email: {email}, password: [HIDDEN]");
        email = email.Trim();

        var user = await _userRepository.GetUserEntityByEmailAsync(email);
        if (user == null)
        {
            Console.WriteLine("LoginUserAsync: User not found in database.");
            Console.WriteLine("LoginUserAsync: Returning null token due to user not found."); // Debug log
            return null; // Should exit here
        }

        Console.WriteLine($"LoginUserAsync: User found: {user.Username}");
        Console.WriteLine($"LoginUserAsync: Password in service (hashed): {user.Password}");
        Console.WriteLine($"LoginUserAsync: Password received from user: {password}"); // !!! REMOVE IN PRODUCTION !!!

        bool passwordMatches = PasswordHelper.VerifyPassword(password, user.Password);
        Console.WriteLine($"LoginUserAsync: Password verification result: {passwordMatches}");

        if (!passwordMatches)
        {
            Console.WriteLine("LoginUserAsync: Password does not match.");
            Console.WriteLine("LoginUserAsync: Returning null token due to password mismatch."); // Debug log
            return null; // Should exit here
        }

        Console.WriteLine("LoginUserAsync: Password matches, proceeding to token generation.");

        var userDto = new UserDTO
        {
            UserId = user.UserId,
            ResumePath = user.ResumePath,
            Email = user.Email,
            Name = user.Username
        };

        var token = JwtHelper.GenerateJwtToken(user.UserId, user.Username, email);
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("LoginUserAsync: Token generation failed.");
            Console.WriteLine("LoginUserAsync: Returning null token due to generation failure."); // Debug log
            return null; // Should exit here
        }

        Console.WriteLine($"LoginUserAsync: 🔑 Generated Token: {token}");
        Console.WriteLine($"LoginUserAsync: Login success for user: {email}");
        return token;
    }

    // This method return type was changed to IActionResult to match common Web API patterns
    // and your usage in previous examples. Ensure your controller expects this.
    public async Task<IActionResult> UploadNewResumeAsync(HttpRequest request, IFormFile resume)
    {
        try
        {
            int userId = GetUserIdFromToken(request);

            var userDTO = await _userRepository.GetUserByIdAsync(userId);
            if (userDTO == null)
            {
                Console.WriteLine($"UploadNewResumeAsync: User with ID {userId} not found for resume upload.");
                return new NotFoundObjectResult(new { error = "User not found." });
            }

            var resumeUrl = await _s3Service.UploadFileAsync(resume, "ayala-spira-testpnoren");
            Console.WriteLine($"UploadNewResumeAsync: Resume uploaded successfully for user {userId}: {resumeUrl}");

            userDTO.ResumePath = resumeUrl;

            await _userRepository.UpdateUserAsync(userDTO);
            Console.WriteLine($"UploadNewResumeAsync: User {userId} resume path updated in DB.");

            return new OkObjectResult(new { message = "Resume uploaded successfully", resumeUrl });
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"UploadNewResumeAsync: Unauthorized access attempt: {ex.Message}");
            return new UnauthorizedObjectResult(new { error = ex.Message });
        }
        catch (Exception ex) // General exception catch for robustness
        {
            Console.WriteLine($"UploadNewResumeAsync: An unexpected error occurred: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    public int GetUserIdFromToken(HttpRequest request)
    {
        var tokenHeader = request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(tokenHeader) || !tokenHeader.StartsWith("Bearer "))
        {
            Console.WriteLine("GetUserIdFromToken: Token is missing or invalid in header.");
            throw new UnauthorizedAccessException("Token is missing or invalid");
        }

        var token = tokenHeader.Replace("Bearer ", "").Trim();
        Console.WriteLine($"GetUserIdFromToken: 🔍 Received Token (truncated for log): {token.Substring(0, Math.Min(token.Length, 30))}..."); // Truncate for security

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jsonToken == null)
        {
            Console.WriteLine("GetUserIdFromToken: JWT token could not be parsed.");
            throw new UnauthorizedAccessException("Invalid JWT token format.");
        }

        var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            Console.WriteLine("GetUserIdFromToken: User ID claim 'nameid' not found in token.");
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        if (!int.TryParse(userIdClaim, out int userId))
        {
            Console.WriteLine($"GetUserIdFromToken: User ID claim '{userIdClaim}' is not a valid integer.");
            throw new UnauthorizedAccessException("Invalid user ID in token format.");
        }

        Console.WriteLine($"GetUserIdFromToken: User ID extracted: {userId}");
        return userId;
    }
}