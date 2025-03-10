using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly S3Service _s3Service;

    public UserController(S3Service s3Service)
    {
        _s3Service = s3Service;
    }

    [HttpPost("upload-resume")]
    public async Task<IActionResult> UploadResume([FromForm] IFormFile resume)
    {
        if (resume == null || resume.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var filePath = Path.Combine("path-to-save", resume.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await resume.CopyToAsync(stream);
        }

        return Ok(new { filePath });

        
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var directoryPath = "path-to-save";  // הנתיב בו נשמרים הקבצים

        if (!Directory.Exists(directoryPath))
        {
            return NotFound("Directory not found.");
        }

        var files = Directory.GetFiles(directoryPath);  // מקבלים את כל הקבצים בתיקייה

        if (files.Length == 0)
        {
            return NotFound("No files found.");
        }

        // החזרת רשימת הקבצים
        return Ok(files);
    }

}
