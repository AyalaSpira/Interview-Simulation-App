namespace InterviewSim.DAL.Entities
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int InterviewId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public int ResponseTime { get; set; }
        public bool IsOnTime { get; set; }
    }
}
