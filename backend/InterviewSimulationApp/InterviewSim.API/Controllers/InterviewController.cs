using InterviewSim.BLL.Interfaces;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace InterviewSim.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAIService _aiService;
        private readonly S3Service _s3Service;

        public InterviewController(IUserService userService, IAIService aiService, S3Service s3Service)
        {
            _userService = userService;
            _aiService = aiService;
            _s3Service = s3Service;
        }

        [HttpPost("upload-resume")]
        public async Task<IActionResult> UploadResume([FromForm] IFormFile resume)
        {
            // 1. העלה את קובץ ה-PDF ל-S3
            var resumeUrl = await _s3Service.UploadFileAsync(resume, "ayala-spira-testpnoren");

            // 2. קריאת תוכן הרזומה
            var resumeContent = await ReadResumeContentAsync(resume);

            // 3. שליחה לבינה מלאכותית לפענוח תחום ההתמחות
            var category = await _aiService.AnalyzeResumeAsync(resumeContent);

            // 4. החזרת ה-URL והקטגוריה שנמצא
            return Ok(new { ResumeUrl = resumeUrl, Category = category });
        }

        private async Task<string> ReadResumeContentAsync(IFormFile resume)
        {
            try
            {
                using (var stream = resume.OpenReadStream())
                using (var reader = new PdfReader(stream))
                {
                    var content = new StringBuilder();
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        content.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    }
                    return content.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading PDF: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
