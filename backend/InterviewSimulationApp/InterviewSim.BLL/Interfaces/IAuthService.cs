using Microsoft.AspNetCore.Http;

namespace InterviewSim.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(string username, string password, IFormFile resume);
        Task<string> LoginUserAsync(string username, string password);
    }
}
