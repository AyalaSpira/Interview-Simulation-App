using System.Collections.Generic;
using System.Threading.Tasks;
using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;

namespace InterviewSim.BLL.Implementations
{
    public class InterviewService : IInterviewService
    {
        private readonly IAIService _aiService;

        public InterviewService(IAIService aiService)
        {
            _aiService = aiService;
        }

        public async Task<string> StartInterviewAsync(int userId)
        {
            // חילוץ תחום ההתמחות מתוך הרזומה (בינה מלאכותית)
            var resumeContent = await GetResumeContentAsync(userId); // המידע הזה יכול להגיע מהמשתמש או ממקום אחר
            var category = await _aiService.AnalyzeResumeAsync(resumeContent);

            // יצירת שאלות בהתבסס על תחום ההתמחות
            var questions = await _aiService.GenerateQuestionsAsync(category);

            // יצירת הראיון
            var interview = new Interview { InterviewId = 1, Questions = questions };

            return $"Interview started with {questions.Count} questions for category: {category}";
        }

        public async Task<List<string>> GetInterviewQuestionsAsync(int interviewId)
        {
            // החזרת השאלות
            return new List<string> { "What is your experience?", "What are your strengths?" }; // כאן, הפוך לשאילתות AI אמיתיות
        }

        public async Task<string> SubmitAnswersAsync(int interviewId, List<string> answers)
        {
            // קבלת השאלות מהראיון
            var questions = new List<string> { "What is your experience?", "What are your strengths?" }; // כאן, החזר את השאלות מ-AI

            // ניתוח התשובות והפקת סיכום
            var summary = await _aiService.AnalyzeInterviewAsync(answers, questions);
            return summary;
        }

        // פונקציה זו יכולה לכלול קריאה למאגר נתונים או לבינה מלאכותית לחילוץ הרזומה
        private async Task<string> GetResumeContentAsync(int userId)
        {
            return "Resume content"; // כאן עליך לבצע את פעולת החילוץ של הרזומה
        }
    }
}
