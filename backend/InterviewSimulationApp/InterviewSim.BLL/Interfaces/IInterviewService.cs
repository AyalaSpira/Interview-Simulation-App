using InterviewSim.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewSim.BLL.Interfaces
{
    public interface IInterviewService
    {
        Task<Interview> StartInterviewAsync(int userId);
        //Task<List<string>> GetInterviewQuestionsAsync(int interviewId);
        Task<Interview> SubmitAnswersAsync(int interviewId, List<string> answers);
        Task<Interview> GetInterviewByIdAsync(int id);

        Task<int?> GetLastInterviewScoreAsync(int userId);

    }
}
