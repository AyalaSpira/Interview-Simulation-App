using InterviewSim.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewSim.DAL.Repositories
{
    public class QuestionRepository : IQuestionRepository  // מממש את הממשק IQuestionRepository
    {
        private readonly InterviewSimContext _context;

        public QuestionRepository(InterviewSimContext context)
        {
            _context = context;
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _context.Set<Question>().FindAsync(questionId);  // יש להבטיח שהטבלה Questions קיימת ב-DbContext
        }

        public async Task<List<Question>> GetQuestionsByCategoryAsync(string category)
        {
            return await _context.Set<Question>().Where(q => q.Category == category).ToListAsync(); // בדיקה על פי קטגוריה
        }

        public async Task SaveQuestionAsync(Question question)
        {
            _context.Set<Question>().Add(question);
            await _context.SaveChangesAsync();
        }
    }
}
