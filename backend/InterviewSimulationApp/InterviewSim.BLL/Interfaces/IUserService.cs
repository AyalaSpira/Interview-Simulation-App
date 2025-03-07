using InterviewSim.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserDetailsAsync(int userId); // ���� ���� �����
        Task UpdateUserResumeAsync(int userId, IFormFile resume); // ����� ����� ����
    }
}
