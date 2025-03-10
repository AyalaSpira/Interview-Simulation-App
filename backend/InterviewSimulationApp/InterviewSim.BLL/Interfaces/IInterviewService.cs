using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Interfaces
{
    public interface IInterviewService
    {
        Task<string> StartInterviewAsync(int userId);
        Task<List<string>> GetInterviewQuestionsAsync(int interviewId);
        Task<string> SubmitAnswersAsync(int interviewId, List<string> answers);
    }
}
