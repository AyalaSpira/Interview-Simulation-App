using InterviewSim.DAL.Entities;
using InterviewSim.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task<User> GetUserEntityByEmailAsync(string email);

    Task<UserDTO> GetUserByIdAsync(int userId);
    Task<UserDTO> GetUserByEmailAsync(string email);
    Task<UserDTO> GetUserByIdAndEmailAsync(int userId, string email);
    Task AddUserAsync(User newUser);  // שמירה ב-User
    Task SaveAsync(User user);        // עדכון ב-User
    Task UpdateUserAsync(UserDTO userDTO);  // עדכון ב-User
    Task DeleteAsync(int userId);     // מחיקה של User
    Task<List<UserDTO>> GetAllUsersAsync();  // מחזיר UserDTO ולא User
    Task<IEnumerable<string>> GetAllResumeUrlsAsync();
    Task<IEnumerable<string>> GetAllReportUrlsAsync();
    Task UpdateResumeUrlToNullAsync(string fileUrl);

    Task<User> GetAdminByCredentialsAsync(string email, string password);

}
