using InterviewSim.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Interfaces
{
    public interface IInterviewService
    {
        Task<Interview> StartInterviewAsync(int userId);
        //Task<List<string>> GetInterviewQuestionsAsync(int interviewId);
        Task<string> SubmitAnswersAsync(int interviewId, List<string> answers);
    }
}
