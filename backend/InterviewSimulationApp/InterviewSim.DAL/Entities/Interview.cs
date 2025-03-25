using System.Text.Json.Serialization;

namespace InterviewSim.DAL.Entities
{
    public class Interview
    {
        public int InterviewId { get; set; }  // מזהה הראיון
        public int UserId { get; set; }  // מזהה המשתמש שביצע את הראיון
        public DateTime InterviewDate { get; set; }  // תאריך הראיון
        public string Status { get; set; }  // סטטוס הראיון (Scheduled, Completed וכו')
        public string Summary { get; set; }  // סיכום הראיון

        public List<string> Questions { get; set; }  // רשימת השאלות
        public List<string> Answers { get; set; }  // רשימת השאלות

        
    }
    public class OpenAIResponse
    {
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
