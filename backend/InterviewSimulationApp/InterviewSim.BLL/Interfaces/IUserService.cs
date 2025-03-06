using InterviewSim.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserDetailsAsync(int userId); // קבלת פרטי משתמש
        Task UpdateUserResumeAsync(int userId, IFormFile resume); // עדכון קורות חיים
    }
}
