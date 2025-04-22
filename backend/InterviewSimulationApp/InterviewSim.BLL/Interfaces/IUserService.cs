using InterviewSim.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Interfaces
{
    public interface IUserService
    {
        Task UpdateUserAsync(UserDTO userDto);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<UserDTO> GetUserByEmailAsync(string email); // חיפוש לפי Email בלבד
        Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email); // חיפוש לפי ID ו-Email
        Task UpdateUserResumeAsync(int userId, IFormFile resume);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<string>> GetAllResumeUrlsAsync();
        Task<IEnumerable<string>> GetAllReportUrlsAsync();
        Task<bool> DeleteFileAsync(string fileUrl, string fileType, int? interviewId = null);

        Task<string> LoginAdminAsync(string email, string password);


        Task AddUserAsync(UserDTO userDto);
        Task UpdateUserByAdminAsync(UserDTO userDto);

    }
}
