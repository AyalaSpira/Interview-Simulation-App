namespace InterviewSim.DAL.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Category { get; set; }
        public string QuestionText { get; set; }
        public int TimeLimit { get; set; }
        public string Profession { get; set; }
    }
}
