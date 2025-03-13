using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Repositories;
using InterviewSim.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly S3Service _s3Service;
    private readonly string _bucketName; // שם ה-bucket

    public UserService(IUserRepository userRepository, S3Service s3Service, string bucketName)
    {
        _userRepository = userRepository;
        _s3Service = s3Service;
        _bucketName = bucketName; // העברת שם ה-bucket
    }

    // עדכון קורות חיים
    public async Task UpdateUserResumeAsync(string password, string username, IFormFile resume)
    {
        // שיניתי את הקריאה ל-GetUserByUsernameAndPassword כך שהיא תעשה חיפוש נכון
        var user = await _userRepository.GetUserByIdAsync(username, password);

        if (user != null && resume != null)
        {
            // מחיקה של קובץ קורות חיים ישן אם קיים
            if (!string.IsNullOrEmpty(user.ResumePath))
            {
                var oldFileKey = user.ResumePath.Substring(user.ResumePath.LastIndexOf("/") + 1); // שולף את שם הקובץ מתוך ה-URL
                await _s3Service.DeleteFileAsync(oldFileKey, _bucketName);
            }

            // העלאת קובץ קורות חיים חדש
            var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
            user.ResumePath = resumeUrl;
            await _userRepository.SaveAsync(user);
        }
    }

    // קבלת משתמש לפי שם וסיסמה
    public async Task<UserDTO> GetUserByIdAsync(string password, string username)
    {
        var user = await _userRepository.GetUserByIdAsync(username, password);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return new UserDTO { Username = user.Username, Password = password, ResumePath = user.ResumePath };
    }

    // קבלת כל המשתמשים
    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var userDtos = new List<UserDTO>();

        foreach (var user in users)
        {
            userDtos.Add(new UserDTO { Username = user.Username, Password = user.Password, ResumePath = user.ResumePath });
        }

        return userDtos;
    }

    // עדכון פרטי משתמש
    public async Task UpdateUserAsync(UserDTO userDto)
    {
        var user = await _userRepository.GetUserByIdAsync(userDto.Username, userDto.Password);

        if (user != null)
        {
            user.Username = userDto.Username;
            user.ResumePath = userDto.ResumePath;
            await _userRepository.SaveAsync(user);
        }
    }
}
