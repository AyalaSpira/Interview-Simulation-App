using InterviewSim.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewSim.DAL.Repositories
{
    public interface IInterviewRepository
    {
        // התחלת ראיון
        Task<Interview> StartInterviewAsync(int userId, List<string> questions);

        // קבלת שאלות ראיון לפי מזהה ראיון
        Task<List<string>> GetInterviewQuestionsAsync(int interviewId);

        // שמירת תשובות
        Task SaveInterviewAnswersAsync(int interviewId, List<string> answers);

        // שמירת דוח ראיון
        Task SaveInterviewReportAsync(int interviewId, string report);

        // עדכון ראיון
        Task UpdateInterviewAsync(Interview interview);

        // קבלת ראיון לפי מזהה
        Task<Interview> GetInterviewByIdAsync(int interviewId);

        // שמירת ראיון חדש
        Task SaveInterviewAsync(Interview interview);

        // עדכון הסיכום ל-null
        Task UpdateReportToNullAsync(string fileUrl);

        Task<IEnumerable<Interview>> GetInterviewsByUserIdAsync(int userId);


        Task<Interview> GetLastInterviewByUserIdAsync(int userId);

    }
}
