using InterviewSim.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewSim.DAL.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly List<Interview> _interviews; // ������, ����� ������ ����� (���� ����� �-DAL)

        public InterviewRepository()
        {
            _interviews = new List<Interview>();
        }

        // ����� �����
        public async Task<Interview> StartInterviewAsync(int userId, List<string> questions)
        {
            var interview = new Interview
            {
                InterviewId = _interviews.Count + 1, // ���� ������ ��� �����
                UserId = userId,
                Status = "In Progress",
                Questions = questions,
                Answers = new List<string>(), // ������� ������� ������ ����
                InterviewDate = System.DateTime.Now
            };

            _interviews.Add(interview);

            return await Task.FromResult(interview);
        }

        // ���� ���� ������� ��� ���� �����
        public async Task<string> GetCategoryByUserIdAsync(int userId)
        {
            // ����� ������ �-DAL ���� �� ���� ������� �� ������ ��� ����
            return await Task.FromResult("Software Engineering"); // �����
        }

        // ���� ���� ����� ���� ��� ���� �����
        public async Task<string> GetResumeContentAsync(int userId)
        {
            // ��� ����� ������ ������� ���� ������ ����
            var userInterviews = _interviews.Where(i => i.UserId == userId).ToList();

            // �� �� ����� ������� ������, ����� ������ ����
            if (!userInterviews.Any())
                return string.Empty;

            // ����, ����� �� ���� ����� ����� - ���� �����, ���� ��� ����.
            // ��� ���� ������ �� ������� ���� ���� ��� ����� �� ����� �����.
            return "Resume content for user " + userId;
        }

        // ���� ����� ����� ��� ���� �����
        public async Task<List<string>> GetInterviewQuestionsAsync(int interviewId)
        {
            var interview = _interviews.FirstOrDefault(i => i.InterviewId == interviewId);
            return await Task.FromResult(interview?.Questions ?? new List<string>());
        }

        // ���� ����� ��� ����
        public async Task<Interview> GetInterviewByIdAsync(int interviewId)
        {
            var interview = _interviews.FirstOrDefault(i => i.InterviewId == interviewId);
            return await Task.FromResult(interview);
        }

        // ����� ����� (�� ������ �������)
        public async Task SaveAsync(Interview interview)
        {
            var existingInterview = _interviews.FirstOrDefault(i => i.InterviewId == interview.InterviewId);
            if (existingInterview != null)
            {
                existingInterview.Status = interview.Status;
                existingInterview.Answers = interview.Answers;  // ����� �������
            }
            await Task.CompletedTask;
        }
        public async Task SaveInterviewReportAsync(int interviewId, string report)
        {
            var interview = _interviews.FirstOrDefault(i => i.InterviewId == interviewId);
            if (interview != null)
            {
                interview.Status = "Completed";
                interview.Answers.Add($"AI Report: {report}");  // ������ �� ���� ������ ������
            }
            await Task.CompletedTask;
        }

    }
}
