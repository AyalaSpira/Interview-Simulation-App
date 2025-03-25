using System.Text.Json.Serialization;

namespace InterviewSim.DAL.Entities
{
    public class Interview
    {
        public int InterviewId { get; set; }  // ���� ������
        public int UserId { get; set; }  // ���� ������ ����� �� ������
        public DateTime InterviewDate { get; set; }  // ����� ������
        public string Status { get; set; }  // ����� ������ (Scheduled, Completed ���')
        public string Summary { get; set; }  // ����� ������

        public List<string> Questions { get; set; }  // ����� ������
        public List<string> Answers { get; set; }  // ����� ������

        
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
