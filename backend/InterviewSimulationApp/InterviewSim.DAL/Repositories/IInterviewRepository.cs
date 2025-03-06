
public interface IInterviewRepository
{
    Task StartInterviewAsync(int interviewId);
    Task<FeedbackDTO> GetFeedbackAsync(int interviewId);
}
