using InterviewSim.BLL.Interfaces;
using InterviewSim.Shared.DTOs;
using InterviewSim.DAL.Repositories;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Implementations
{
    public class InterviewService(IInterviewRepository interviewRepository) : IInterviewService
    {
        public async Task<InterviewDTO> StartInterviewAsync(int userId)
        {
            // לוגיקה להפעיל ראיון
            //var interview = await interviewRepository.StartInterviewAsync(userId);
            return new InterviewDTO(); //{ InterviewId = interview.InterviewId, Status = "In Progress" };
        }

        public async Task<FeedbackDTO> GetFeedbackAsync(int interviewId)
        {
            // לוגיקה להחזיר פידבק
            var feedback = await interviewRepository.GetFeedbackAsync(interviewId);
            return new FeedbackDTO { InterviewId = interviewId, FeedbackText = feedback.FeedbackText };
        }
    }
}
