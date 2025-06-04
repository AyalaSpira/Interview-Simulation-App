//using InterviewSim.Shared.DTOs;
//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace InterviewSim.BLL.Interfaces
//{
//    public interface IUserService
//    {
//        Task UpdateUserAsync(UserDTO userDto);
//        Task<List<UserDTO>> GetAllUsersAsync();
//        Task<UserDTO> GetUserByIdAsync(int userId);
//        Task<UserDTO> GetUserByEmailAsync(string email); // חיפוש לפי Email בלבד
//        Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email); // חיפוש לפי ID ו-Email
//        Task UpdateUserResumeAsync(int userId, IFormFile resume);
//        Task<bool> DeleteUserAsync(int id);
//        Task<IEnumerable<string>> GetAllResumeUrlsAsync();
//        Task<IEnumerable<string>> GetAllReportUrlsAsync();
//        Task<bool> DeleteFileAsync(string fileUrl, string fileType, int? interviewId = null);

//        Task<string> LoginAdminAsync(string email, string password);


//        Task AddUserAsync(UserDTO userDto);
//        Task UpdateUserByAdminAsync(UserDTO userDto);

//    }
//}

using InterviewSim.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task UpdateUserResumeAsync(int userId, IFormFile resume);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task UpdateUserAsync(UserDTO userDto); // לשמור אם יש קריאות לפונקציה זו
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<string>> GetAllResumeUrlsAsync();
        Task<IEnumerable<string>> GetAllReportUrlsAsync();
        Task<bool> DeleteFileAsync(string fileUrl, string fileType, int? interviewId = null);
        Task<string> LoginAdminAsync(string email, string password);

        // הוספה ועריכה למנהל מערכת
        Task AddUserWithResumeAsync(string name, string email, string password, IFormFile resumeFile);
        Task UpdateUserByAdminAsync(int userId, string name, string email, string? password = null, IFormFile? newResumeFile = null);
    }
}