using InterviewSim.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InterviewSim.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewService _interviewService;

        public InterviewController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [HttpPost("generate-questions")]
        public async Task<IActionResult> GenerateQuestions([FromBody] string category)
        {
           // var questions = await _interviewService.StartInterviewAsync(category);
            return Ok();
        }

        [HttpPost("submit-answers")]
        public async Task<IActionResult> SubmitAnswers([FromBody] List<string> answers)
        {
            //var feedback = await _interviewService.SubmitAnswersAsync(answers);
            return Ok();
        }
    }
}
