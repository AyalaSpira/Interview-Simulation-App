using InterviewSim.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InterviewSim.DAL.Repositories
{
    public class AnswerRepository : IAnswerRepository  // ���� �� ����� IAnswerRepository
    {
        private readonly InterviewSimContext _context;

        public AnswerRepository(InterviewSimContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Answer> GetAnswerByIdAsync(int answerId)
        {
            return await _context.Set<Answer>().FindAsync(answerId);  // �� ������ ������ Answers ����� �-DbContext
        }

        public async Task SaveAnswerAsync(Answer answer)
        {
            _context.Set<Answer>().Add(answer);
            await _context.SaveChangesAsync();
        }
    }
}
