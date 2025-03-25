using InterviewSim.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewSim.DAL.Repositories
{
    public interface IQuestionRepository
    {
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<List<Question>> GetQuestionsByCategoryAsync(string category);
        Task SaveQuestionAsync(Question question);
    }
}
