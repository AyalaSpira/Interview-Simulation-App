namespace InterviewSim.DAL.Entities
{
    public class Interview
    {
        public int InterviewId { get; set; }
        public int UserId { get; set; }
        public DateTime InterviewDate { get; set; }
        public string Status { get; set; }
    }
}
