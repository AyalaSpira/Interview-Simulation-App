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
        Console.WriteLine("---- Saving interview answers ------");

        var interview = await _context.Interviews.FindAsync(interviewId);
        if (interview == null)
        {
            Console.WriteLine($"Interview {interviewId} not found.");
            throw new Exception("Interview not found.");
        }

        if (answers == null || answers.Count == 0)
        {
            Console.WriteLine("No answers submitted.");
            throw new Exception("Answers list is empty.");
        }

        // הצגת תשובות ישנות וחדשות
        Console.WriteLine("OLD Answers: " + string.Join(" || ", interview.Answers ?? new List<string>()));
        Console.WriteLine("NEW Answers: " + string.Join(" || ", answers));

        // עדכון תשובות וסטטוס
        interview.Answers = new List<string>(answers);
        interview.Status = "Completed";

        // שמירה
        await UpdateInterviewAsync(interview);
        await _context.SaveChangesAsync();

        Console.WriteLine($"✅ Answers saved for interview {interviewId} and status set to 'Completed'");
    }

    // שמירת דוח ראיון
    public async Task SaveInterviewReportAsync(int interviewId, string report)
    {
        var interview = await _context.Interviews.FindAsync(interviewId);
        if (interview != null)
        {
            interview.Status = "Completed";
            interview.Summary = $"AI Report: {report}";
            await UpdateInterviewAsync(interview);
            await _context.SaveChangesAsync();
        }
    }

    // עדכון ראיון
    public async Task UpdateInterviewAsync(Interview interview)
    {
        var existingInterview = await _context.Interviews.FindAsync(interview.InterviewId);
        if (existingInterview == null)
            throw new Exception($"Interview with ID {interview.InterviewId} not found.");

        Console.WriteLine("----- BEFORE UPDATE -----");
        Console.WriteLine("OLD Answers: " + string.Join(" || ", existingInterview.Answers));
        Console.WriteLine("NEW Answers: " + string.Join(" || ", interview.Answers));
        Console.WriteLine("OLD Summary: " + existingInterview.Summary);
        Console.WriteLine("NEW Summary: " + interview.Summary);
        Console.WriteLine("OLD Status: " + existingInterview.Status);
        Console.WriteLine("NEW Status: " + interview.Status);

        // השוואה - האם באמת יש שינוי
        bool isAnswersChanged = string.Join("||", existingInterview.Answers) != string.Join("||", interview.Answers);
        bool isSummaryChanged = existingInterview.Summary != interview.Summary;
        bool isStatusChanged = existingInterview.Status != interview.Status;

        if (!isAnswersChanged && !isSummaryChanged && !isStatusChanged)
        {
            Console.WriteLine("❗ No changes detected. Skipping update.");
            return;
        }

        // FORCE UPDATE - לרוקן קודם כדי לוודא שינוי
        if (isAnswersChanged)
        {
            _context.Entry(existingInterview).Property(e => e.Answers).CurrentValue = null;
            await _context.SaveChangesAsync(); // שלב ביניים כדי לאפס
            existingInterview.Answers = new List<string>(interview.Answers);
            _context.Entry(existingInterview).Property(e => e.Answers).IsModified = true;
        }

        if (isSummaryChanged)
        {
            existingInterview.Summary = interview.Summary;
            _context.Entry(existingInterview).Property(e => e.Summary).IsModified = true;
        }

        if (isStatusChanged)
        {
            existingInterview.Status = interview.Status;
            _context.Entry(existingInterview).Property(e => e.Status).IsModified = true;
        }

        Console.WriteLine("Saving changes...");
        int affected = await _context.SaveChangesAsync();
        Console.WriteLine($"✅ Rows affected: {affected}");

        // בדיקת טעינה מחדש
        var reloaded = await _context.Interviews
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.InterviewId == interview.InterviewId);

        Console.WriteLine("----- AFTER UPDATE (Reloaded) -----");
        Console.WriteLine("Answers (from DB): " + string.Join(" || ", reloaded.Answers));
        Console.WriteLine("Summary (from DB): " + reloaded.Summary);
        Console.WriteLine("Status (from DB): " + reloaded.Status);
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
