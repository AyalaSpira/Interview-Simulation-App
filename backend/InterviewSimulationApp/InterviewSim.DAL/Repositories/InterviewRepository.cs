using InterviewSim.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewSim.DAL.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly List<Interview> _interviews; // לדוגמה, רשימה במיקום מקומי (צריך להיות ב-DAL)

        public InterviewRepository()
        {
            _interviews = new List<Interview>();
        }

        // התחלת ראיון
        public async Task<Interview> StartInterviewAsync(int userId, List<string> questions)
        {
            var interview = new Interview
            {
                InterviewId = _interviews.Count + 1, // מספר ייחודי לכל ראיון
                UserId = userId,
                Status = "In Progress",
                Questions = questions,
                Answers = new List<string>(), // התשובות מתחילות כרשימה ריקה
                InterviewDate = System.DateTime.Now
            };

            _interviews.Add(interview);

            return await Task.FromResult(interview);
        }

        // קבלת תחום ההתמחות לפי מזהה משתמש
        public async Task<string> GetCategoryByUserIdAsync(int userId)
        {
            // להניח שמישהו ב-DAL מחפש את תחום ההתמחות של המשתמש לפי מזהה
            return await Task.FromResult("Software Engineering"); // דוגמה
        }

        // קבלת תוכן קורות חיים לפי מזהה משתמש
        public async Task<string> GetResumeContentAsync(int userId)
        {
            // כאן אנחנו מניחים שהמשתמש נמצא ברשימה שלנו
            var userInterviews = _interviews.Where(i => i.UserId == userId).ToList();

            // אם לא נמצאו ראיונות למשתמש, נחזיר מחרוזת ריקה
            if (!userInterviews.Any())
                return string.Empty;

            // אחרת, נחזיר את תוכן קורות החיים - עבור דוגמה, נניח שזה פשוט.
            // כאן תוכל להוסיף את הלוגיקה שאתה צריך כדי לשלוף את קורות החיים.
            return "Resume content for user " + userId;
        }

        // קבלת שאלות ראיון לפי מזהה ראיון
        public async Task<List<string>> GetInterviewQuestionsAsync(int interviewId)
        {
            var interview = _interviews.FirstOrDefault(i => i.InterviewId == interviewId);
            return await Task.FromResult(interview?.Questions ?? new List<string>());
        }

        // קבלת ראיון לפי מזהה
        public async Task<Interview> GetInterviewByIdAsync(int interviewId)
        {
            var interview = _interviews.FirstOrDefault(i => i.InterviewId == interviewId);
            return await Task.FromResult(interview);
        }

        // שמירת ראיון (אם נשמרים שינויים)
        public async Task SaveAsync(Interview interview)
        {
            var existingInterview = _interviews.FirstOrDefault(i => i.InterviewId == interview.InterviewId);
            if (existingInterview != null)
            {
                existingInterview = interview; // עדכון הראיון
            }
            await Task.CompletedTask;
        }
    }
}
