using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepository;

        public AuthService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> RegisterUserAsync(string username, string password, IFormFile resume)
        {
            // הלוגיקה של רישום משתמש (הוספת קובץ קורות חיים וכו')
            return "User Registered Successfully";
        }

        public async Task<string> LoginUserAsync(string username, string password)
        {
            // הלוגיקה של התחברות (אימות סיסמא)
            return "Login Successful";
        }
    }
}
