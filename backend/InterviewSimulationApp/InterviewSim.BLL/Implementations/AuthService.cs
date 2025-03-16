using InterviewSim.BLL.Helpers;
using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using InterviewSim.Shared.Helpers;
using Microsoft.AspNetCore.Http;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly S3Service _s3Service;

    public AuthService(IUserRepository userRepository, S3Service s3Service)
    {
        _userRepository = userRepository;
        _s3Service = s3Service;
    }

    public async Task<string> RegisterUserAsync(string username, string password, IFormFile resume)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(username, password);
        if (existingUser != null)
        {
            throw new Exception("Username already exists.");
        }

        var hashedPassword = PasswordHelper.HashPassword(password);

        string resumePath = null;
        if (resume != null)
        {
            resumePath = await _s3Service.UploadFileAsync(resume, "ayala-spira-testpnoren");
        }

        var newUser = new User
        {
            Username = username,
            Password = hashedPassword,
            ResumePath = resumePath
        };

        await _userRepository.AddUserAsync(newUser);

        return "User Registered Successfully";
    }

    public async Task<string> LoginUserAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByIdAsync(username, password);
        if (user == null || !PasswordHelper.VerifyPassword(password, user.Password))
        {
            throw new Exception("Invalid username or password");
        }

        var token = JwtHelper.GenerateJwtToken(user.UserId, user.Username);
        return token;
    }
}
