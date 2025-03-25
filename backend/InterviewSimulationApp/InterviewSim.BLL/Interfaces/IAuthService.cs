using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewSim.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(string username, string password, string email, IFormFile resume);
        Task<string> LoginUserAsync(string email, string password);
       
        Task<IActionResult> UploadNewResumeAsync(HttpRequest request, IFormFile resume);
        int GetUserIdFromToken(HttpRequest request); // הוספת הפונקציה כאן
    }
}
