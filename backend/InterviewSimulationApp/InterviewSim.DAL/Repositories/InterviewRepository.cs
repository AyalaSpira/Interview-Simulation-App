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
        // ������ ������ �����
    }

    public async Task<FeedbackDTO> GetFeedbackAsync(int interviewId)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        // ������ ����� �����
        return new FeedbackDTO();
    }
}
