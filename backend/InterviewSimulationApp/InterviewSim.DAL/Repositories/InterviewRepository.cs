using InterviewSim.DAL;


public class InterviewRepository : IInterviewRepository
{
    private readonly InterviewSimContext _context;

    public InterviewRepository(InterviewSimContext context)
    {
        _context = context;
    }

    public async Task StartInterviewAsync(int interviewId)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        // לוגיקה להפעלת ראיון
    }

    public async Task<FeedbackDTO> GetFeedbackAsync(int interviewId)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        // לוגיקה לקבלת פידבק
        return new FeedbackDTO();
    }
}
