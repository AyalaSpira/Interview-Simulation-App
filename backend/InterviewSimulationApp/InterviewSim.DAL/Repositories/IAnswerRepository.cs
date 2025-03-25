using InterviewSim.DAL.Entities;
using System.Threading.Tasks;

namespace InterviewSim.DAL.Repositories
{
    public interface IAnswerRepository
    {
        Task<Answer> GetAnswerByIdAsync(int answerId);
        Task SaveAnswerAsync(Answer answer);
    }
}
