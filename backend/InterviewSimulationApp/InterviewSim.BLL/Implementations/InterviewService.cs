using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using InterviewSim.Shared.DTOs;

namespace InterviewSim.BLL.Implementations
{
    public class InterviewService : IInterviewService
    {
        private readonly IAIService _aiService;
        private readonly Dictionary<int, int> _interviewAnswersCount = new Dictionary<int, int>();
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public InterviewService(IAIService aiService, IUserService userService, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _aiService = aiService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<string> GetUserResumePathAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId); // ����� ������ ������������
            return user?.ResumePath ?? string.Empty; // ����� ����� �� ����, ���� ������ ����
        }

        public async Task<Interview> StartInterviewAsync(int userId)
        {
            Task<User> a = _userRepository.GetUserByIdAsync(userId);
            var resumeContent = await GetResumeContentAsync(a.Result.ResumePath);
            //����� �_aiService
            var category = await _aiService.AnalyzeResumeAsync(resumeContent);
            //����� �_aiService
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

            Console.WriteLine($"Interview started with {questions.Count} questions for category: {category}");
            return interview;
        }

        //������ ������ �������
        public async Task<string> SubmitAnswersAsync(int interviewId, List<string> answers)
        {
            var userId = 0; // Retrieve userId based on interviewId from the data source

            var questions = new List<string> { "What is your experience?", "What are your strengths?" }; // ����� �-AI �� ��-DB

            if (_interviewAnswersCount.ContainsKey(userId))
            {
                _interviewAnswersCount[userId] += answers.Count;
            }

            if (_interviewAnswersCount[userId] >= 3)
            {
                var category = " ";//await _aiService.AnalyzeResumeAsync(await GetResumeContentAsync(userId));
                questions = await _aiService.GenerateQuestionsAsync(category);
            }

            var summary = await _aiService.AnalyzeInterviewAsync(answers, questions);
            return summary;
        }

        //���� �� ���� ������ �� ���� ����
        public async Task<string> GetResumeContentAsync(string path)
        {
            var resumePath = await _userService.GetResumeContentAsync(path);

            if (string.IsNullOrEmpty(resumePath))
            {
                return string.Empty; // �� ��� ���� �����
            }

            return await GetResumeContentFromFileAsync(path);
        }
//��� ������ ������� �� �����
private async Task<string> GetResumeContentFromFileAsync(string resumePath)
{
    if (File.Exists(resumePath))
    {
        return await File.ReadAllTextAsync(resumePath); // ����� ���� ������
    }

    return string.Empty; // ����� �� ����� �� �� ��� �����
}

    }
}
