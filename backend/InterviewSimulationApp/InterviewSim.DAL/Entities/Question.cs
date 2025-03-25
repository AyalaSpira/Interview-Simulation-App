namespace InterviewSim.DAL.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }  // מזהה ייחודי של השאלה
        public string Category { get; set; }  // קטגוריית השאלה (למשל: "טכני", "התנהגותי")
        public string QuestionText { get; set; }  // נוסח השאלה
        public int TimeLimit { get; set; }  // מגבלת זמן למענה (בשניות)
        public string Profession { get; set; }
    }
}
