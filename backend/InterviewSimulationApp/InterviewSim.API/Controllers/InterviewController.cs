using InterviewSim.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using InterviewSim.BLL.Implementations;
namespace InterviewSim.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewService _interviewService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;

        public InterviewController(IInterviewService interviewService,IMailService mailService, IUserService userService)
        {
            _interviewService = interviewService;
            _mailService = mailService;
            _userService = userService;

        }

        // Endpoint להתחלת ראיון
        [HttpPost("start")]
        public async Task<IActionResult> StartInterview([FromQuery] int userId)
        {
            Console.WriteLine("נכנס לפונקציה של השאלות");
            if (userId <= 0)
            {
                Console.WriteLine("Received invalid user ID: " + userId); // לוג עבור בעיית קלט
                return BadRequest("Invalid user ID.");
            }

            try
            {
                Console.WriteLine($"Starting interview for userId: {userId}");
                var result = await _interviewService.StartInterviewAsync(userId);

                // לוודא שהשירות מחזיר שאלות
                if (result == null /*|| result.Count == 0*/)
                {
                    Console.WriteLine("No questions found after starting the interview.");
                    return StatusCode(404, "No questions available.");
                }

                Console.WriteLine($"Interview started successfully. Questions available: {result.Questions}");

                return Ok(result.Questions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting the interview for userId: {userId}. Error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("submit-answers")]
        public async Task<IActionResult> SubmitAnswers([FromBody] JsonElement request)
        {
            Console.WriteLine("SubmitAnswers POST request received");

            // ווידוא שהנתונים נכונים
            if (!request.TryGetProperty("userId", out JsonElement userIdElement) ||
                !userIdElement.TryGetInt32(out int userId) ||
                !request.TryGetProperty("answers", out JsonElement answersElement) ||
                answersElement.ValueKind != JsonValueKind.Array)
            {
                Console.WriteLine("Invalid data: Missing userId or answers");
                return BadRequest("Invalid data: userId and answers are required.");
            }
            Console.WriteLine($"Received userId: {userId}, answers count: {answersElement.GetArrayLength()}");

            // ניקוי התשובות
            List<string> answers = answersElement.EnumerateArray()
                .Select(a => a.GetString())
                .Where(a => a != null)
                .ToList();
            Console.WriteLine($"Processed answers: {answers.Count} answers");

            // בדיקה אם יש תשובות ואם הנתונים תקינים
            if (userId <= 0 || answers.Count == 0)
            {
                Console.WriteLine($"Invalid data for userId: {userId}, answers count: {answers.Count}");
                return BadRequest("Invalid data: userId should be greater than 0 and answers cannot be empty.");
            }

            try
            {
                Console.WriteLine($"Starting process to submit answers for userId: {userId}");
                Console.WriteLine("-----------In controller----------");
               answers.ForEach(answer => Console.WriteLine( "answer"+answer));
                // קבלת הראיון ושמירת התשובות
                var interview = await _interviewService.SubmitAnswersAsync(userId, answers);

                if (interview == null)
                {
                    Console.WriteLine($"Unable to submit answers. Interview is null for userId: {userId}");
                    return StatusCode(404, "Unable to submit answers.");
                }

                // תוצאה מוצלחת
                // תוצאה מוצלחת
                Console.WriteLine($"Answers submitted successfully for interviewId: {interview.InterviewId}");

                return Ok(new
                {
                    InterviewId = interview.InterviewId,
                    Questions = interview.Questions,
                    Answers = interview.Answers,
                    Summary = interview.Summary
                });
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות
                Console.WriteLine($"Error submitting answers: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("download-report")]
        public async Task<IActionResult> DownloadReport([FromQuery] int interviewId)
        {
            // שליפת הראיון מהמאגר
            var interview = await _interviewService.GetInterviewByIdAsync(interviewId);
            if (interview == null)
            {
                return NotFound("Interview not found.");
            }

            // יצירת מסמך PDF
            using (var memoryStream = new MemoryStream())
            {
                // יצירת מסמך חדש
                var document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                // הוספת תוכן לדו"ח
                document.Add(new Paragraph("Interview Report"));
                document.Add(new Paragraph($"Interview Date: {interview.InterviewDate}"));
                document.Add(new Paragraph($"User ID: {interview.UserId}"));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("Questions and Answers:"));

                // סופר את כמות התוכן שנוסף לדף
                float contentHeight = 0;

                // להוסיף הפסקי דף אם התוכן ארוך מדי
                for (int i = 0; i < interview.Questions.Count; i++)
                {
                    var question = interview.Questions[i];
                    var answer = (i < interview.Answers.Count ? interview.Answers[i] : "No answer provided");

                    // הוספת שאלה
                    document.Add(new Paragraph($"Q{i + 1}: {question}"));
                    contentHeight += 20;  // מניח כל שאלה היא בגובה 20 פיקסלים

                    // הוספת תשובה
                    document.Add(new Paragraph($"A{i + 1}: {answer}"));
                    contentHeight += 20;  // מניח כל תשובה היא בגובה 20 פיקסלים
                    document.Add(new Paragraph("\n"));
                    contentHeight += 10;  // מרווח בין שאלה לתשובה

                    // אם התוכן קרוב לסוף הדף, הוסף הפסק דף
                    if (contentHeight > document.PageSize.Height - document.BottomMargin - 50)
                    {
                        document.NewPage(); // הפסק דף
                        contentHeight = 0;  // אתחול סופר התוכן
                    }
                }

                document.Add(new Paragraph("Summary:"));
                document.Add(new Paragraph(interview.Summary));

                // סגירת המסמך
                document.Close();

                // החזרת התוכן כקובץ PDF
                var fileBytes = memoryStream.ToArray();
                var fileName = $"InterviewReport_{interviewId}.pdf";

                return File(fileBytes, "application/pdf", fileName);
            }
        }
        [HttpPost("send-report")]
        public async Task<IActionResult> SendReport([FromQuery] int interviewId)
        {
            // שליפת הראיון מהמאגר
            var interview = await _interviewService.GetInterviewByIdAsync(interviewId);
            if (interview == null)
            {
                Console.WriteLine($"Interview with ID {interviewId} not found.");
                return NotFound("Interview not found.");
            }

            string userEmail = null; // אתחול המייל של המשתמש ל-null

            try
            {
                // שליפת המשתמש באמצעות ה-UserId מהראיון
                var user = await _userService.GetUserByIdAsync(interview.UserId);

                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    // אם המשתמש נמצא ויש לו מייל, נשמור את המייל
                    userEmail = user.Email;
                    Console.WriteLine($"User email found: {userEmail} for userId: {interview.UserId}");
                }
                else
                {
                    // אם המשתמש לא נמצא או שאין לו מייל תקין
                    Console.WriteLine($"User with ID {interview.UserId} not found or email is empty/null. Cannot send email.");
                    // כאן אנחנו מחזירים שגיאה במקום לשלוח למייל דיפולטיבי
                    return NotFound($"User with ID {interview.UserId} not found or email is missing/invalid.");
                }
            }
            catch (Exception ex)
            {
                // אם קרתה שגיאה כלשהי במהלך שליפת המשתמש
                Console.WriteLine($"Error retrieving user for ID {interview.UserId}: {ex.Message}. Cannot send email.");
                return StatusCode(500, $"Internal server error while retrieving user email: {ex.Message}");
            }

            // יצירת מסמך PDF
            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, memoryStream);

                document.Open();
                document.Add(new Paragraph("Interview Report"));
                document.Add(new Paragraph($"Interview Date: {interview.InterviewDate}"));
                document.Add(new Paragraph($"User ID: {interview.UserId}"));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("Questions and Answers:"));

                float contentHeight = 0;

                for (int i = 0; i < interview.Questions.Count; i++)
                {
                    var question = interview.Questions[i];
                    var answer = (i < interview.Answers.Count ? interview.Answers[i] : "No answer provided");

                    document.Add(new Paragraph($"Q{i + 1}: {question}"));
                    contentHeight += 20;

                    document.Add(new Paragraph($"A{i + 1}: {answer}"));
                    contentHeight += 20;
                    document.Add(new Paragraph("\n"));
                    contentHeight += 10;

                    if (contentHeight > document.PageSize.Height - document.BottomMargin - 50)
                    {
                        document.NewPage();
                        contentHeight = 0;
                    }
                }

                document.Add(new Paragraph("Summary:"));
                document.Add(new Paragraph(interview.Summary));

                document.Close();

                var fileBytes = memoryStream.ToArray();
                var fileName = $"InterviewReport_{interviewId}.pdf";

                // שליחת המייל עם הדוח כקובץ מצורף
                var subject = "Your Interview Report";
                var body = "Please find attached your interview report.";
                try
                {
                    // נשלח מייל רק אם userEmail אינו null (כלומר, נמצא מייל תקין)
                    if (userEmail != null) // בדיקה וודאית למרות שלוגיקת ה-try-catch כבר מטפלת בזה
                    {
                        await _mailService.SendEmailWithAttachmentAsync(userEmail, subject, body, fileBytes, fileName);
                        Console.WriteLine($"Email sent successfully to {userEmail}.");
                        return Ok($"Email sent successfully to {userEmail}.");
                    }
                    else
                    {
                        // מקרה זה לא אמור לקרות אם הלוגיקה של ה-try-catch נכונה
                        Console.WriteLine($"An unexpected error occurred: userEmail is null. Cannot send email.");
                        return StatusCode(500, "An unexpected error prevented sending the email.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email to {userEmail}: {ex}");
                    return StatusCode(500, $"Error sending email: {ex.Message}");
                }
            }
        }
        [HttpGet("get-text-report")]
        public async Task<IActionResult> GetTextReport([FromQuery] int interviewId)
        {
            var interview = await _interviewService.GetInterviewByIdAsync(interviewId);
            if (interview == null)
            {
                return NotFound("Interview not found.");
            }

            var reportText = new StringBuilder();
            reportText.AppendLine($"Interview Date: {interview.InterviewDate}");
            reportText.AppendLine($"User ID: {interview.UserId}");
            reportText.AppendLine("\nQuestions and Answers:\n");

            for (int i = 0; i < interview.Questions.Count; i++)
            {
                var question = interview.Questions[i];
                var answer = (i < interview.Answers.Count ? interview.Answers[i] : "No answer provided");
                reportText.AppendLine($"Q{i + 1}: {question}");
                reportText.AppendLine($"A{i + 1}: {answer}");
                reportText.AppendLine();
            }

            reportText.AppendLine("Summary:");
            reportText.AppendLine(interview.Summary);

            return Ok(reportText.ToString());
        }

    }
}
