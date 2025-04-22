using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using InterviewSim.DAL.Repositories;
using InterviewSim.Shared.DTOs;
using InterviewSim.Shared.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IInterviewRepository _interviewRepository;
    private readonly S3Service _s3Service;
    private readonly string _bucketName;

    public UserService(IUserRepository userRepository, IInterviewRepository interviewRepository, S3Service s3Service, string bucketName)
    {
        _userRepository = userRepository;
        _interviewRepository = interviewRepository;
        _s3Service = s3Service;
        _bucketName = bucketName;
    }

    public async Task<UserDTO> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email); // חיפוש לפי Email

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return new UserDTO
        {
            UserId = user.UserId,
            ResumePath = user.ResumePath,
            Email = user.Email,
            Name = user.Name
        };
    }

    public async Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email)
    {
        var user = await _userRepository.GetUserByIdAndEmailAsync(userId, email); // חיפוש לפי ID ו-Email

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return new UserDTO
        {
            UserId = user.UserId,
            ResumePath = user.ResumePath,
            Email = user.Email,
            Name = user.Name
        };
    }

    public async Task<UserDTO> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId); // חיפוש לפי ID

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return new UserDTO
        {
            UserId = user.UserId,
            ResumePath = user.ResumePath,
            Email = user.Email,
            Name = user.Name
        };
    }

    public async Task UpdateUserResumeAsync(int userId, IFormFile resume)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user != null && resume != null)
        {
            if (!string.IsNullOrEmpty(user.ResumePath))
            {
                var oldFileKey = user.ResumePath.Substring(user.ResumePath.LastIndexOf("/") + 1);
                await _s3Service.DeleteFileByUrlAsync(oldFileKey, _bucketName);
            }

            var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
            user.ResumePath = resumeUrl;
            await _userRepository.UpdateUserAsync(user);
        }
    }

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var userDtos = new List<UserDTO>();

        foreach (var user in users)
        {
            userDtos.Add(new UserDTO
            {
                UserId = user.UserId,
                ResumePath = user.ResumePath,
                Email = user.Email,
                Name = user.Name
            });
        }

        return userDtos;
    }

    public async Task UpdateUserAsync(UserDTO userDto)
    {
        // חיפוש משתמש לפי ID ו-Email
        var user = await _userRepository.GetUserByIdAndEmailAsync(userDto.UserId, userDto.Email);

        if (user != null)
        {
            // המרת UserDTO ל-User
            user.UserId = userDto.UserId;
            user.ResumePath = userDto.ResumePath;
            user.Email = userDto.Email;
            user.Name = userDto.Name; // עדכון שם המשתמש ב-Username של User

            // עדכון הנתונים במסד
            await _userRepository.UpdateUserAsync(user);
        }
    }


    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
            return false;

        await _userRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<string>> GetAllResumeUrlsAsync()
    {
        return await _userRepository.GetAllResumeUrlsAsync();
    }

    public async Task<IEnumerable<string>> GetAllReportUrlsAsync()
    {
        return await _userRepository.GetAllReportUrlsAsync();
    }

    public async Task<bool> DeleteFileAsync(string fileUrl, string fileType, int? interviewId = null)
    {
        bool result = await _s3Service.DeleteFileByUrlAsync(fileUrl, _bucketName);

        if (result)
        {
            if (fileType == "resume")
            {
                await _userRepository.UpdateResumeUrlToNullAsync(fileUrl);
            }
            if (fileType == "report")
            {
                await _interviewRepository.UpdateReportToNullAsync(fileUrl);
            }
        }
        return result;
    }

    public async Task<string> LoginAdminAsync(string email, string password)
    {
        var admin = await _userRepository.GetAdminByCredentialsAsync(email, password);

        if (admin == null)
            throw new UnauthorizedAccessException("Email or password is incorrect.");

        return JwtHelper.GenerateJwtToken(admin.Id, admin.Password, admin.Email);
    }


    public async Task AddUserAsync(UserDTO userDto)
    {
        var user = new User
        {
            Username = userDto.Name,
            Email = userDto.Email,
            ResumePath = userDto.ResumePath ?? string.Empty,
            Password = "123456" // או קלט אחר או מחולל סיסמאות
        };

        await _userRepository.AddUserAsync(user);
    }

    public async Task UpdateUserByAdminAsync(UserDTO userDto)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(userDto.UserId);
        if (existingUser == null)
            throw new InvalidOperationException("User not found.");

        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;
        existingUser.ResumePath = userDto.ResumePath;

        await _userRepository.UpdateUserAsync(existingUser);
    }


}
