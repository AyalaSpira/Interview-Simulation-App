using InterviewSim.BLL.Helpers;
using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using InterviewSim.Shared.Helpers;
using Microsoft.AspNetCore.Http;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly S3Service _s3Service;  // הוספנו את ה-S3Service כאן

    // הזרקת התלות של S3Service בנוסף ל-UserRepository
    public AuthService(IUserRepository userRepository, S3Service s3Service)
    {
        _userRepository = userRepository;
        _s3Service = s3Service; 
    }

    public async Task<string> RegisterUserAsync(string username, string password, IFormFile resume)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(username,password);
        if (existingUser != null)
        {
            throw new Exception("Username already exists.");
        }

        var hashedPassword = PasswordHelper.HashPassword(password);

        // שמירת קובץ קורות חיים אם יש
        string resumePath = null;
        if (resume != null)
        {
            // העלאה ל-S3 וקבלת URL
            resumePath = await _s3Service.UploadFileAsync(resume, "ayala-spira-testpnoren");
        }

        // יצירת משתמש חדש
        var newUser = new User
        {
            Username = username,
            Password = hashedPassword,
            ResumePath = resumePath // שמירת ה-URL ב-ResumePath
        };

        // הוספת המשתמש לבסיס הנתונים
        await _userRepository.AddUserAsync(newUser);

        return "User Registered Successfully";
    }

    // התחברות
    public async Task<string> LoginUserAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByIdAsync(username, password);
        if (user == null || !PasswordHelper.VerifyPassword(password, user.Password))
        {
            throw new Exception("Invalid username or password");
        }
        Console.WriteLine("234567890876543245678908765432");
        var token = JwtHelper.GenerateJwtToken(user.UserId, user.Username);
        return token;
    }

}
