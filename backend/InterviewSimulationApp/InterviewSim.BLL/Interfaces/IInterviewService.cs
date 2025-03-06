using InterviewSim.Shared.DTOs;

namespace InterviewSim.BLL.Interfaces
{
    public interface IInterviewService
    {
        Task<InterviewDTO> StartInterviewAsync(int userId);
        Task<FeedbackDTO> GetFeedbackAsync(int interviewId);
    }
}
