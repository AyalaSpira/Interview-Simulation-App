using InterviewSim.DAL.Entities;
using InterviewSim.DAL.Repositories;
using InterviewSim.DAL;
using Microsoft.EntityFrameworkCore;

public class InterviewRepository : IInterviewRepository
{
    private readonly InterviewSimContext _context;

    public InterviewRepository(InterviewSimContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // ����� �����
    public async Task<Interview> StartInterviewAsync(int userId, List<string> questions)
    {
        var interview = new Interview
        {
            UserId = userId,
            Status = "In Progress",
            Questions = questions,
            Answers = new List<string>(),
            InterviewDate = DateTime.Now
        };

        await _context.Interviews.AddAsync(interview);
        await _context.SaveChangesAsync();

        return interview;
    }

    // ���� ����� ����� ��� ���� �����
    public async Task<List<string>> GetInterviewQuestionsAsync(int interviewId)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        return interview?.Questions ?? new List<string>();
    }

    // ����� ������
    public async Task SaveInterviewAnswersAsync(int interviewId, List<string> answers)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        if (interview == null)
        {
            Console.WriteLine($"Interview {interviewId} not found.");
            throw new Exception("Interview not found.");
        }

        interview.Status = "Completed"; // ����� �����
        interview.Answers = answers; // ����� �� �������
        await _context.SaveChangesAsync();
        Console.WriteLine($"Answers saved for interview {interviewId}: {string.Join(", ", answers)}");
    }

    // ����� ��� �����
    public async Task SaveInterviewReportAsync(int interviewId, string report)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        if (interview != null)
        {
            interview.Status = "Completed";
            interview.Summary = $"AI Report: {report}";
            await _context.SaveChangesAsync();
        }
    }

    // ����� �����
    public async Task UpdateInterviewAsync(Interview interview)
    {
        _context.Interviews.Update(interview);
        await _context.SaveChangesAsync();
    }

    // ���� ����� ��� ����
    public async Task<Interview> GetInterviewByIdAsync(int interviewId)
    {
        return await _context.Interviews.FirstOrDefaultAsync(i => i.InterviewId == interviewId);
    }

    // ����� ����� ���
    public async Task SaveInterviewAsync(Interview interview)
    {
        await _context.Interviews.AddAsync(interview);
        await _context.SaveChangesAsync();
    }

    // ����� ������ �-null
    public async Task UpdateReportToNullAsync(string fileUrl)
    {
        var interview = await _context.Interviews.FirstOrDefaultAsync(r => r.Summary == fileUrl);
        if (interview != null)
        {
            interview.Summary = null;
            await _context.SaveChangesAsync();
        }
    }

    // ����� �� �������� �� ����� ��� ����
    public async Task<IEnumerable<Interview>> GetInterviewsByUserIdAsync(int userId)
    {
        return await _context.Interviews
            .Where(i => i.UserId == userId)
            .ToListAsync();
    }
}
