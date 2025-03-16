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
    private readonly string _bucketName;

    public UserService(IUserRepository userRepository, S3Service s3Service, string bucketName)
    {
        _userRepository = userRepository;
        _s3Service = s3Service;
        _bucketName = bucketName;
    }

    public async Task UpdateUserResumeAsync(string password, string username, IFormFile resume)
    {
        var user = await _userRepository.GetUserByIdAsync(username, password);

        if (user != null && resume != null)
        {
            if (!string.IsNullOrEmpty(user.ResumePath))
            {
                var oldFileKey = user.ResumePath.Substring(user.ResumePath.LastIndexOf("/") + 1);
                await _s3Service.DeleteFileAsync(oldFileKey, _bucketName);
            }

            var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
            user.ResumePath = resumeUrl;
            await _userRepository.SaveAsync(user);
        }
    }

    public async Task<UserDTO> GetUserByIdAsync(string password, string username)
    {
        var user = await _userRepository.GetUserByIdAsync(username, password);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return new UserDTO { Username = user.Username, Password = password, ResumePath = user.ResumePath };
    }

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


    public async Task<string> GetResumeContentAsync(string resumePath)
    {
        if (File.Exists(resumePath)) 
        {
            return await File.ReadAllTextAsync(resumePath); // קריאת תוכן הרזומה
        }

        return string.Empty; // במקרה של שגיאה או אם אין רזומה
    }
}
