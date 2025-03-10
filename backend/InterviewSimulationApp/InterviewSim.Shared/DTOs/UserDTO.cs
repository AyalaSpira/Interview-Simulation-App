using Microsoft.AspNetCore.Http;

namespace InterviewSim.Shared.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public IFormFile? Resume { get; set; } // יהיה רק ברישום
    }
}
