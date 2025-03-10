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
        var existingUser = await _userRepository.GetUserByUsernameAsync(username);
        if (existingUser != null)
        {
            throw new Exception("Username already exists.");
        }

        var hashedPassword = PasswordHelper.HashPassword(password);

        // שמירת קובץ קורות חיים אם יש
        string resumePath = null;
        if (resume != null)
        {
            resumePath = await _s3Service.UploadFileAsync(resume);  // השתמשנו ב-_s3Service
        }

        var newUser = new User
        {
            Username = username,
            Password = hashedPassword,
            Resume = resumePath // אם יש קובץ קורות חיים
        };

        await _userRepository.AddUserAsync(newUser);

        return "User Registered Successfully";
    }

    // התחברות
    public async Task<string> LoginUserAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null || !PasswordHelper.VerifyPassword(password, user.Password))
        {
            throw new Exception("Invalid username or password");
        }

        var token = JwtHelper.GenerateJwtToken(user.UserId, user.Username);
        return token;
    }

}
