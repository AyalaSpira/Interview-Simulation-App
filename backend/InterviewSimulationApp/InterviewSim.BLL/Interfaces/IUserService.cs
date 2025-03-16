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
        Task<UserDTO> GetUserByIdAsync(string password, string name);
        Task UpdateUserResumeAsync(string password, string name, IFormFile resume);

        Task<string> GetResumeContentAsync(string resumePath); // הוספת השיטה הזו

        //Task<string> GetCurrentUserId();
    }
}
