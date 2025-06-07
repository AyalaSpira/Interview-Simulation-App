using InterviewSim.DAL.Entities;
using InterviewSim.DAL.Repositories;
using InterviewSim.DAL;
using Microsoft.EntityFrameworkCore;
using Mysqlx;

public class InterviewRepository : IInterviewRepository
{
    private readonly InterviewSimContext _context;

    public InterviewRepository(InterviewSimContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // התחלת ראיון
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

    // קבלת שאלות ראיון לפי מזהה ראיון
    public async Task<List<string>> GetInterviewQuestionsAsync(int interviewId)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        return interview?.Questions ?? new List<string>();
    }

    // Fix for CS1997: Since 'InterviewRepository.SaveInterviewAnswersAsync(int, List<string>)' is an async method that returns 'Task', a return keyword must not be followed by an object expression  
    public async Task SaveInterviewAnswersAsync(int interviewId, List<string> answers)
    {
        Console.WriteLine("----try saved answers------");
        var interview = await _context.Interviews.FindAsync(interviewId);
        if (interview == null)
        {
            Console.WriteLine($"Interview {interviewId} not found.");
            throw new Exception("Interview not found.");
        }

        interview.Status = "Completed"; // Update status  
        interview.Answers = answers; // Save answers  
      await  UpdateInterviewAsync(interview); await _context.SaveChangesAsync();
        Console.WriteLine($"Answers saved for interview {interviewId}: {string.Join(", ", answers)}");
        Console.WriteLine(interview.Answers); // Correctly return a Task instead of an object expression  
      
    }

    // שמירת דוח ראיון
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

    // עדכון ראיון
    public async Task UpdateInterviewAsync(Interview interview)
    {
        _context.Interviews.Update(interview);
        await _context.SaveChangesAsync();
    }

    // קבלת ראיון לפי מזהה
    public async Task<Interview> GetInterviewByIdAsync(int interviewId)
    {
        return await _context.Interviews.FirstOrDefaultAsync(i => i.InterviewId == interviewId);
    }

    // שמירת ראיון חדש
    public async Task SaveInterviewAsync(Interview interview)
    {
        await _context.Interviews.AddAsync(interview);
        await _context.SaveChangesAsync();
    }

    // עדכון הסיכום ל-null
    public async Task UpdateReportToNullAsync(string fileUrl)
    {
        var interview = await _context.Interviews.FirstOrDefaultAsync(r => r.Summary == fileUrl);
        if (interview != null)
        {
            interview.Summary = null;
            await _context.SaveChangesAsync();
        }
    }

    // שליפת כל הראיונות של משתמש לפי מזהה
    public async Task<IEnumerable<Interview>> GetInterviewsByUserIdAsync(int userId)
    {
        return await _context.Interviews
            .Where(i => i.UserId == userId)
            .ToListAsync();
    }



    public async Task<Interview> GetLastInterviewByUserIdAsync(int userId)
    {
        // לפי הנתונים ששלחת, השדה הרלוונטי הוא InterviewDate
        return await _context.Interviews
                             .Where(i => i.UserId == userId)
                             .OrderByDescending(i => i.InterviewDate) // מיון יורד לפי InterviewDate כדי לקבל את האחרון
                             .FirstOrDefaultAsync(); // מחזיר את הראשון (האחרון בזמן) או null
    }
}
