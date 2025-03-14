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
            // ����� ���� ������� ���� ������ (���� ��������)
            var resumeContent = await GetResumeContentAsync(userId); // ����� ��� ���� ����� ������� �� ����� ���
            var category = await _aiService.AnalyzeResumeAsync(resumeContent);

            // ����� ����� ������ �� ���� �������
            var questions = await _aiService.GenerateQuestionsAsync(category);

            // ����� ������
            var interview = new Interview { InterviewId = 1, Questions = questions };

            return $"Interview started with {questions.Count} questions for category: {category}";
        }

        public async Task<List<string>> GetInterviewQuestionsAsync(int interviewId)
        {
            // ����� ������
            return new List<string> { "What is your experience?", "What are your strengths?" }; // ���, ���� �������� AI �������
        }

        public async Task<string> SubmitAnswersAsync(int interviewId, List<string> answers)
        {
            // ���� ������ �������
            var questions = new List<string> { "What is your experience?", "What are your strengths?" }; // ���, ���� �� ������ �-AI

            // ����� ������� ����� �����
            var summary = await _aiService.AnalyzeInterviewAsync(answers, questions);
            return summary;
        }

        // ������� �� ����� ����� ����� ����� ������ �� ����� �������� ������ ������
        private async Task<string> GetResumeContentAsync(int userId)
        {
            return "Resume content"; // ��� ���� ���� �� ����� ������ �� ������
        }
    }
}
