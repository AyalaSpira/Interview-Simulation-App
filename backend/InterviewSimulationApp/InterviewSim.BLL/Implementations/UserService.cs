using InterviewSim.BLL.Interfaces;
using InterviewSim.Shared.DTOs;
using InterviewSim.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> GetUserDetailsAsync(int userId)
        {
            // ������ ������ ���� �����
            var user =  _userRepository.GetUserByIdAsync(userId);
            return new UserDTO { UserId = user.UserId, Username = user.Username };
        }

        public async Task UpdateUserResumeAsync(int userId, IFormFile resume)
        {
            // ������ ������ ����� ����
            var user =  _userRepository.GetUserByIdAsync(userId);
           // user.Resume = resume.FileName; // ���� ������� �� ����
            await _userRepository.SaveAsync(user);
        }
    }
}
