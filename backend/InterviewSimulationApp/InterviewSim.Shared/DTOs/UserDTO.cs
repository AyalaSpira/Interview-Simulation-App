using Microsoft.AspNetCore.Http;

namespace InterviewSim.Shared.DTOs
{
    public class UserDTO
    {

        public int UserId { get; set; }
        public string Name { get; set; }
        public string? ResumePath { get; set; }
        public string Email { get; set; } 

    }
}
