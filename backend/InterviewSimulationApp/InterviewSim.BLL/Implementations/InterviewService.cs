using InterviewSim.BLL.Interfaces;
using InterviewSim.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using InterviewSim.Shared.DTOs;
using DocumentFormat.OpenXml.Packaging;
using System.Text;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using InterviewSim.DAL.Repositories;


namespace InterviewSim.BLL.Implementations
{
    public class InterviewService : IInterviewService
    {
        private readonly IAIService _aiService;
        private readonly Dictionary<int, int> _interviewAnswersCount = new Dictionary<int, int>();
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly S3Service _s3Service;
        private readonly IInterviewRepository _interviewRepository;
        public InterviewService(IAIService aiService, IUserService userService, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, S3Service s3Service, IInterviewRepository interviewRepository)
        {
            _aiService = aiService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _s3Service = s3Service;
            _interviewRepository = interviewRepository;
        }

        public async Task<string> GetUserResumePathAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId); // שליפת המשתמש מהריפוזיטורי
            return user?.ResumePath ?? string.Empty; // החזרת הנתיב אם קיים, אחרת מחרוזת ריקה
        }
        public async Task<Interview> StartInterviewAsync(int userId)
        {
            // שים לב ששינית את השיטה ב-Repository להחזיר UserDTO ולא User
            Task<UserDTO> a = _userRepository.GetUserByIdAsync(userId);
            var resumeContent = await GetResumeContentFromFileAsync(a.Result.ResumePath);
            var category = await _aiService.AnalyzeResumeAsync(resumeContent);
            var questions = await _aiService.GenerateQuestionsAsync(category);

            var interview = new Interview
            {
                Questions = questions,
                UserId = userId,
                InterviewDate = DateTime.Now,
                Status = "In Progress",
                Answers = new List<string>(),
                Summary = ""
            };

            await _interviewRepository.SaveInterviewAsync(interview);

            Console.WriteLine($"Interview started with {questions.Count} questions for category: {category}");
            return interview;
        }

        public async Task<Interview> SubmitAnswersAsync(int interviewId, List<string> answers)
        {
            Console.WriteLine($"SubmitAnswersAsync started for interviewId: {interviewId}");

            var interview = await _interviewRepository.GetInterviewByIdAsync(interviewId);
            if (interview == null)
            {
                Console.WriteLine($"Interview not found for interviewId: {interviewId}");
                throw new Exception("Interview not found");
            }
            Console.WriteLine($"Interview found: {interview.InterviewId}");

            // שמירה של התשובות בראיון
            Console.WriteLine($"Saving answers for interviewId: {interviewId}");
            await _interviewRepository.SaveInterviewAnswersAsync(interviewId, answers);

            var questions = await _interviewRepository.GetInterviewQuestionsAsync(interviewId);
            if (questions == null || !questions.Any())
            {
                Console.WriteLine($"No questions found for interviewId: {interviewId}");
                throw new Exception("No questions found for the interview");
            }
            Console.WriteLine($"Found {questions.Count} questions for interviewId: {interviewId}");

            // ניתוח התשובות
            Console.WriteLine("Starting AI analysis on answers");
            var summary = await _aiService.AnalyzeInterviewAsync(answers, questions);
            if (summary == null)
            {
                Console.WriteLine($"Failed to analyze interview for interviewId: {interviewId}");
                throw new Exception("Failed to analyze interview");
            }
            Console.WriteLine("AI analysis complete");

            // שמירת הסיכום בראיון
            Console.WriteLine($"Saving summary for interviewId: {interviewId}");
            await _interviewRepository.SaveInterviewReportAsync(interviewId, summary);

            // עדכון הסיכום והסטטוס של הראיון
            Console.WriteLine($"Updating interview details for interviewId: {interviewId}");
            interview.Answers = answers;
            interview.Summary = summary;
            interview.Status = "Completed";

            await _interviewRepository.UpdateInterviewAsync(interview);
            Console.WriteLine($"Interview updated and status set to 'Completed' for interviewId: {interviewId}");

            // החזרת כל פרטי הראיון
            Console.WriteLine($"SubmitAnswersAsync completed for interviewId: {interviewId}");
            return interview;
        }

        //פונ פנימית שמחזירה את התכון
        public async  Task<Interview> GetInterviewByIdAsync(int id)
        {
            var interview = await _interviewRepository.GetInterviewByIdAsync(id);
            return interview;
        }


        #region מחזירות תוכן רזומה
        public async Task<string> GetResumeContentFromFileAsync(string resumePath)
        {
            if (string.IsNullOrEmpty(resumePath))
                return string.Empty;

            byte[] fileBytes;

            if (resumePath.StartsWith("https://") && resumePath.Contains("amazonaws.com"))
            {
                // פירוק ה-URL כדי לחלץ את ה-Bucket Name וה-Key
                var uri = new Uri(resumePath);
                var segments = uri.AbsolutePath.TrimStart('/').Split('/');

                if (segments.Length < 2)
                    return "Invalid S3 URL format.";

                string bucketName = segments[0]; // שם ה-Bucket
                string key = segments[1]; // המפתח (Key) של הקובץ

                fileBytes = await _s3Service.DownloadFileAsByteArrayAsync(bucketName, key);
            }
            else if (File.Exists(resumePath))
            {
                fileBytes = await File.ReadAllBytesAsync(resumePath);
            }
            else
            {
                return "Invalid file path.";
            }

            // קביעת סוג הקובץ לפי הסיומת
            string fileExtension = System.IO.Path.GetExtension(resumePath).ToLower();

            return fileExtension switch
            {
                ".docx" => ExtractTextFromDocx(fileBytes),
                ".pdf" => ExtractTextFromPdf(fileBytes),
                _ => "Unsupported file format."
            };
        }

        private string ExtractTextFromDocx(byte[] fileBytes)
        {
            using (MemoryStream stream = new MemoryStream(fileBytes))
            using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, false))
            {
                return doc.MainDocumentPart.Document.Body.InnerText;
            }
        }
        public string ExtractTextFromPdf(byte[] pdfBytes)
        {
            using (MemoryStream stream = new MemoryStream(pdfBytes))
            using (PdfReader reader = new PdfReader(stream))
            {
                string text = "";
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text += PdfTextExtractor.GetTextFromPage(reader, i);
                }
                return text;
            }

        }

        #endregion



    }
}
