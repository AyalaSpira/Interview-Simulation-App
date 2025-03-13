using Microsoft.AspNetCore.Http;

namespace InterviewSim.Shared.DTOs
{
    public class UserDTO
    {

        public string Username { get; set; }
        public string Password { get; set; }  // מוסיפים את ה-UserId
        public string? ResumePath { get; set; }

    }
}
