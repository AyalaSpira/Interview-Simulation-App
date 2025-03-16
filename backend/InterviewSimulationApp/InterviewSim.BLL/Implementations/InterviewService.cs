using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace InterviewSim.BLL.Implementations
{
    public class InterviewService : IInterviewService
    {
        private readonly IAIService _aiService;
        private readonly Dictionary<int, int> _interviewAnswersCount = new Dictionary<int, int>();
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InterviewService(IAIService aiService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _aiService = aiService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> StartInterviewAsync(int userId)
        {
            var resumeContent = await GetResumeContentAsync(userId);
            var category = await _aiService.AnalyzeResumeAsync(resumeContent);

            var questions = await _aiService.GenerateQuestionsAsync(category);

            var interview = new Interview
            {
                InterviewId = 1,
                Questions = questions,
                UserId = userId,
                InterviewDate = DateTime.Now,
                Status = "In Progress",
                Answers = new List<string>()
            };

            _interviewAnswersCount[userId] = 0;

            return $"Interview started with {questions.Count} questions for category: {category}";
        }

        public async Task<List<string>> GetInterviewQuestionsAsync(int interviewId)
        {
            return new List<string> { "What is your experience?", "What are your strengths?" }; // תשאל את השאלות מ-AI
        }

        public async Task<string> SubmitAnswersAsync(int interviewId, List<string> answers)
        {
            var userId = 0; // Retrieve userId based on interviewId from the data source

            var questions = new List<string> { "What is your experience?", "What are your strengths?" }; // שאלות מ-AI או מה-DB

            if (_interviewAnswersCount.ContainsKey(userId))
            {
                _interviewAnswersCount[userId] += answers.Count;
            }

            if (_interviewAnswersCount[userId] >= 10)
            {
                var category = await _aiService.AnalyzeResumeAsync(await GetResumeContentAsync(userId));
                questions = await _aiService.GenerateQuestionsAsync(category);
            }

            var summary = await _aiService.AnalyzeInterviewAsync(answers, questions);
            return summary;
        }

        public async Task<string> GetResumeContentAsync(int userId)
        {
            // תחילת שליפה באמצעות HttpContext אם יש טוקן
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                return "Token is missing"; // טעות אם אין טוקן
            }

            var resumePath = await _userService.GetResumeContentAsync(userId.ToString());

            if (string.IsNullOrEmpty(resumePath))
            {
                return string.Empty; // אם אין נתיב רזומה
            }

            return await GetResumeContentFromFileAsync(resumePath);
        }

        private async Task<string> GetResumeContentFromFileAsync(string resumePath)
        {
            if (File.Exists(resumePath))
            {
                return await File.ReadAllTextAsync(resumePath); // קריאת תוכן הרזומה
            }

            return string.Empty; // במקרה של שגיאה או אם אין רזומה
        }
    }
}
