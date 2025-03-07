using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;  // ����� �- UserRepository �- IUserRepository

        public AuthService(IUserRepository userRepository)  // �� ���
        {
            _userRepository = userRepository;
        }
        public async Task<string> RegisterUserAsync(string username, string password, IFormFile resume)
        {
            // ������� �� ����� ����� (����� ���� ����� ���� ���')
            return "User Registered Successfully";
        }

        public async Task<string> LoginUserAsync(string username, string password)
        {
            // ������� �� ������� (����� �����)
            return "Login Successful";
        }
    }
}
