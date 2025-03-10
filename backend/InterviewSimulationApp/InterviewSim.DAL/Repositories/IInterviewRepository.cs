using InterviewSim.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewSim.DAL.Repositories
{
    public interface IInterviewRepository
    {
        Task<string> GetResumeContentAsync(int userId);
        Task<List<string>> GetInterviewQuestionsAsync(int interviewId);
        Task<Interview> StartInterviewAsync(int userId, List<string> questions);
    }
}
