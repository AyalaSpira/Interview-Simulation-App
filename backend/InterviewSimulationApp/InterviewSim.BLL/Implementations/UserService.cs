//using InterviewSim.BLL.Interfaces;
//using InterviewSim.DAL.Entities;
//using InterviewSim.DAL.Repositories;
//using InterviewSim.Shared.DTOs;
//using InterviewSim.Shared.Helpers;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//public class UserService : IUserService
//{
//    private readonly IUserRepository _userRepository;
//    private readonly IInterviewRepository _interviewRepository;
//    private readonly S3Service _s3Service;
//    private readonly string _bucketName;

//    public UserService(IUserRepository userRepository, IInterviewRepository interviewRepository, S3Service s3Service, string bucketName)
//    {
//        _userRepository = userRepository;
//        _interviewRepository = interviewRepository;
//        _s3Service = s3Service;
//        _bucketName = bucketName;
//    }

//    public async Task<UserDTO> GetUserByEmailAsync(string email)
//    {
//        var user = await _userRepository.GetUserByEmailAsync(email); // ����� ��� Email

//        if (user == null)
//        {
//            throw new UnauthorizedAccessException("User not found.");
//        }

//        return new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Name
//        };
//    }

//    public async Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email)
//    {
//        var user = await _userRepository.GetUserByIdAndEmailAsync(userId, email); // ����� ��� ID �-Email

//        if (user == null)
//        {
//            throw new UnauthorizedAccessException("User not found.");
//        }

//        return new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Name
//        };
//    }

//    public async Task<UserDTO> GetUserByIdAsync(int userId)
//    {
//        var user = await _userRepository.GetUserByIdAsync(userId); // ����� ��� ID

//        if (user == null)
//        {
//            throw new UnauthorizedAccessException("User not found.");
//        }

//        return new UserDTO
//        {
//            UserId = user.UserId,
//            ResumePath = user.ResumePath,
//            Email = user.Email,
//            Name = user.Name
//        };
//    }

//    public async Task UpdateUserResumeAsync(int userId, IFormFile resume)
//    {
//        var user = await _userRepository.GetUserByIdAsync(userId);

//        if (user != null && resume != null)
//        {
//            if (!string.IsNullOrEmpty(user.ResumePath))
//            {
//                var oldFileKey = user.ResumePath.Substring(user.ResumePath.LastIndexOf("/") + 1);
//                await _s3Service.DeleteFileByUrlAsync(oldFileKey, _bucketName);
//            }

//            var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
//            user.ResumePath = resumeUrl;
//            await _userRepository.UpdateUserAsync(user);
//        }
//    }

//    public async Task<List<UserDTO>> GetAllUsersAsync()
//    {
//        var users = await _userRepository.GetAllUsersAsync();
//        var userDtos = new List<UserDTO>();

//        foreach (var user in users)
//        {
//            userDtos.Add(new UserDTO
//            {
//                UserId = user.UserId,
//                ResumePath = user.ResumePath,
//                Email = user.Email,
//                Name = user.Name
//            });
//        }

//        return userDtos;
//    }

//    public async Task UpdateUserAsync(UserDTO userDto)
//    {
//        // ����� ����� ��� ID �-Email
//        var user = await _userRepository.GetUserByIdAndEmailAsync(userDto.UserId, userDto.Email);

//        if (user != null)
//        {
//            // ���� UserDTO �-User
//            user.UserId = userDto.UserId;
//            user.ResumePath = userDto.ResumePath;
//            user.Email = userDto.Email;
//            user.Name = userDto.Name; // ����� �� ������ �-Username �� User

//            // ����� ������� ����
//            await _userRepository.UpdateUserAsync(user);
//        }
//    }


//    public async Task<bool> DeleteUserAsync(int id)
//    {
//        var user = await _userRepository.GetUserByIdAsync(id);
//        if (user == null)
//            return false;

//        await _userRepository.DeleteAsync(id);
//        return true;
//    }

//    public async Task<IEnumerable<string>> GetAllResumeUrlsAsync()
//    {
//        return await _userRepository.GetAllResumeUrlsAsync();
//    }

//    public async Task<IEnumerable<string>> GetAllReportUrlsAsync()
//    {
//        return await _userRepository.GetAllReportUrlsAsync();
//    }

//    public async Task<bool> DeleteFileAsync(string fileUrl, string fileType, int? interviewId = null)
//    {
//        bool result = await _s3Service.DeleteFileByUrlAsync(fileUrl, _bucketName);

//        if (result)
//        {
//            if (fileType == "resume")
//            {
//                await _userRepository.UpdateResumeUrlToNullAsync(fileUrl);
//            }
//            if (fileType == "report")
//            {
//                await _interviewRepository.UpdateReportToNullAsync(fileUrl);
//            }
//        }
//        return result;
//    }

//    public async Task<string> LoginAdminAsync(string email, string password)
//    {
//        var admin = await _userRepository.GetAdminByCredentialsAsync(email, password);

//        if (admin == null)
//            throw new UnauthorizedAccessException("Email or password is incorrect.");

//        return JwtHelper.GenerateJwtToken(admin.Id, admin.Password, admin.Email);
//    }


//    public async Task AddUserAsync(UserDTO userDto)
//    {
//        var user = new User
//        {
//            Username = userDto.Name,
//            Email = userDto.Email,
//            ResumePath = userDto.ResumePath ?? string.Empty,
//            Password = "123456" // �� ��� ��� �� ����� �������
//        };

//        await _userRepository.AddUserAsync(user);
//    }

//    public async Task UpdateUserByAdminAsync(UserDTO userDto)
//    {
//        var existingUser = await _userRepository.GetUserByIdAsync(userDto.UserId);
//        if (existingUser == null)
//            throw new InvalidOperationException("User not found.");

//        existingUser.Name = userDto.Name;
//        existingUser.Email = userDto.Email;
//        existingUser.ResumePath = userDto.ResumePath;

//        await _userRepository.UpdateUserAsync(existingUser);
//    }


//}

using InterviewSim.BLL.Helpers;
using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using InterviewSim.DAL.Repositories;
using InterviewSim.Shared.DTOs;
using InterviewSim.Shared.Helpers; // ���� PasswordHelper
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
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return user; // GetUserByEmailAsync ��� ����� DTO
    }

    public async Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email)
    {
        var user = await _userRepository.GetUserByIdAndEmailAsync(userId, email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return user; // GetUserByIdAndEmailAsync ��� ����� DTO
    }

    public async Task<UserDTO> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        return user; // GetUserByIdAsync ��� ����� DTO
    }

    public async Task UpdateUserResumeAsync(int userId, IFormFile resume)
    {
        // �������� ��� ����� ������ ����� ���� �� ������ ����
        // ��� ��� ���� ������. ��� ����� �����.
        var userDto = await _userRepository.GetUserByIdAsync(userId); // ���� DTO
        if (userDto == null) throw new InvalidOperationException("User not found.");

        var userEntity = new User
        {
            UserId = userDto.UserId,
            Username = userDto.Name,
            Email = userDto.Email,
            ResumePath = userDto.ResumePath
        };

        if (resume != null)
        {
            if (!string.IsNullOrEmpty(userEntity.ResumePath))
            {
                var oldFileKey = userEntity.ResumePath.Substring(userEntity.ResumePath.LastIndexOf("/") + 1);
                await _s3Service.DeleteFileByUrlAsync(oldFileKey, _bucketName);
            }

            var resumeUrl = await _s3Service.UploadFileAsync(resume, _bucketName);
            userEntity.ResumePath = resumeUrl;
        }
        await _userRepository.UpdateUserAsync(userEntity); // ����� �� �-Entity
    }

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync(); // Repository ��� ����� List<UserDTO>
    }

    // ������� �� ��� ����� �-UserRepository.
    // �� �� ������ ���� ���� ������ �-AdminPanel, �� ���� ����� ���� �����
    // �� �� ������� ������� (���� ����� �����).
    // ����� �������� ����� UpdateUserByAdminAsync ����� �� ������ �����.
    // �� �� ����� �������� ������� �� ����.
    public async Task UpdateUserAsync(UserDTO userDto)
    {
        // ��� �� ���� ����� DTO ���� ��� ���� ������ �� ���� ����� ����.
        // ���� ����� �����, ����� �-UpdateUserByAdminAsync.
        // �� �� ������ ����� ������� ���, �� ����.
        var userEntity = new User
        {
            UserId = userDto.UserId,
            Username = userDto.Name,
            Email = userDto.Email,
            ResumePath = userDto.ResumePath
        };
        await _userRepository.UpdateUserAsync(userEntity);
    }


    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
            return false;

        // ����� ����� ���� �-S3 ���� ����� ������
        if (!string.IsNullOrEmpty(user.ResumePath))
        {
            var fileKey = user.ResumePath.Substring(user.ResumePath.LastIndexOf("/") + 1);
            await _s3Service.DeleteFileByUrlAsync(fileKey, _bucketName);
        }

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

    // �����: ������� ������ ����� ��� ������ �� ����� ����� ����� ����
    // �� �������� ������ ������� �-AddUserWithResumeAsync
    //public async Task AddUserWithResumeAsync(string name, string email, string password, IFormFile resumeFile)
    //{
    //    var existingUser = await _userRepository.GetUserEntityByEmailAsync(email); // ���� ����, �� DTO
    //    if (existingUser != null)
    //    {
    //        throw new InvalidOperationException("User with this email already exists.");
    //    }

    //    string resumePath = string.Empty;
    //    if (resumeFile != null)
    //    {
    //        resumePath = await _s3Service.UploadFileAsync(resumeFile, _bucketName);
    //    }

    //    var newUser = new User
    //    {
    //        Username = name,
    //        Email = email,
    //        Password = PasswordHelper.HashPassword(password), // ����� ������
    //        ResumePath = resumePath
    //    };

    //    await _userRepository.AddUserAsync(newUser);
    //}

    //// �����: ������� ������ ����� ��� ������ (��, ������, ����� ����������, ����� ���� ����� �����������)
    //// �� �������� ������ ������� �-UpdateUserByAdminAsync
    //public async Task UpdateUserByAdminAsync(int userId, string name, string email, string? password = null, IFormFile? newResumeFile = null)
    //{
    //    var existingUser = await _userRepository.GetUserByIdAsync(userId); // ���� DTO
    //    if (existingUser == null)
    //        throw new InvalidOperationException("User not found.");

    //    var userEntity = new User
    //    {
    //        UserId = existingUser.UserId,
    //        Username = name, // �� ������
    //        Email = email,
    //        ResumePath = existingUser.ResumePath // ����� �� ����� ����� ������ ����
    //    };

    //    // ����� ����� �� �����
    //    if (!string.IsNullOrEmpty(password))
    //    {
    //        userEntity.Password = PasswordHelper.HashPassword(password); // ����� ������
    //    }
    //    else
    //    {
    //        // �� �� ����� ����� ����, ����� �� ������ ������ �����
    //        var userFromDb = await _userRepository.GetUserEntityByEmailAsync(existingUser.Email);
    //        if (userFromDb != null)
    //        {
    //            userEntity.Password = userFromDb.Password;
    //        }
    //    }


    //    // ����� ������ ���� �����
    //    if (newResumeFile != null)
    //    {
    //        // ����� ���� ����� ���� ��� �� ����
    //        if (!string.IsNullOrEmpty(existingUser.ResumePath))
    //        {
    //            var oldFileKey = existingUser.ResumePath.Substring(existingUser.ResumePath.LastIndexOf("/") + 1);
    //            await _s3Service.DeleteFileByUrlAsync(oldFileKey, _bucketName);
    //        }
    //        // ����� ���� ����� ���� ���
    //        userEntity.ResumePath = await _s3Service.UploadFileAsync(newResumeFile, _bucketName);
    //    }
    //    // �� �� ���� ���� ���, ��� ����� ����� �� ����� (���� ����� ������)
    //    // ������, �� ����� ����� "�����" ����� ����:
    //    // else if (shouldRemoveExistingResume) { userEntity.ResumePath = null; }


    //    await _userRepository.UpdateUserAsync(userEntity); // ����� �� �-Entity
    //}


    public async Task AddUserWithResumeAsync(string name, string email, string password, IFormFile? resumeFile)
    {
        // **����� �������� ������� ����**
        var existingUserByEmail = await _userRepository.GetUserEntityByEmailAsync(email);
        if (existingUserByEmail != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var newUser = new User
        {
            Username = name, // �� ������ �� ���� ����� ������
            Email = email,
            Password = PasswordHelper.HashPassword(password),
            ResumePath = string.Empty
        };

        if (resumeFile != null)
        {
            var resumeUrl = await _s3Service.UploadFileAsync(resumeFile, _bucketName);
            newUser.ResumePath = resumeUrl;
        }

        await _userRepository.AddUserAsync(newUser);
    }

    public async Task UpdateUserByAdminAsync(int userId, string name, string email, string? password, IFormFile? newResumeFile)
    {
        var existingUserDTO = await _userRepository.GetUserByIdAsync(userId);
        if (existingUserDTO == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        // **����� �������� ������� ���� ���� (�� �����)**
        if (existingUserDTO.Email != email)
        {
            var userWithNewEmail = await _userRepository.GetUserEntityByEmailAsync(email);
            if (userWithNewEmail != null && userWithNewEmail.UserId != userId)
            {
                throw new InvalidOperationException("Another user with this email already exists.");
            }
        }

        // ��� �� �-Entity ������ ������
        // ��� ��: GetUserEntityByEmailAsync ���� �� ����� ��� �� ������� ����� ���� ���� ����� �� ������� ����.
        // ���� ����� ��� �-UserId ��� �����.
        var userToUpdate = await _userRepository.GetUserByIdAsync(userId); // ��� �� �-UserDTO
        if (userToUpdate == null) // ���� ���� ��� ����
        {
            throw new InvalidOperationException("Error retrieving user entity for update.");
        }

        // ���� �-User Entity ��� ��� ����� ��� �����������
        var userEntity = await _userRepository.GetUserEntityByEmailAsync(existingUserDTO.Email) // �� GetUserEntityByIdAsync �� ����� ���
                          ?? new User { UserId = existingUserDTO.UserId }; // �� �� ����� ����, ��� ���� �� �-ID

        // ���� �� ����� �-Entity
        userEntity.Username = name;
        userEntity.Email = email;

        if (!string.IsNullOrEmpty(password))
        {
            userEntity.Password = PasswordHelper.HashPassword(password);
        }

        if (newResumeFile != null)
        {
            if (!string.IsNullOrEmpty(userEntity.ResumePath))
            {
                var oldFileKey = userEntity.ResumePath.Substring(userEntity.ResumePath.LastIndexOf("/") + 1);
                await _s3Service.DeleteFileByUrlAsync(oldFileKey, _bucketName);
            }
            var newResumeUrl = await _s3Service.UploadFileAsync(newResumeFile, _bucketName);
            userEntity.ResumePath = newResumeUrl;
        }

        await _userRepository.UpdateUserAsync(userEntity);
    }
}
