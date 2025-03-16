using InterviewSim.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

                Console.WriteLine($"Interview started successfully. Questions available: {result}");

                return Ok(new { questions = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting the interview for userId: {userId}. Error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint לשליחת תשובות לראיון
        [HttpPost("submit-answers")]
        public async Task<IActionResult> SubmitAnswers([FromQuery] int interviewId, [FromQuery] int userId, [FromBody] List<string> answers)
        {
            if (interviewId <= 0 || userId <= 0 || answers == null || answers.Count == 0)
            {
                Console.WriteLine($"Received invalid data for interviewId: {interviewId}, userId: {userId}, answers count: {answers?.Count}");
                return BadRequest("Invalid data.");
            }

            try
            {
                Console.WriteLine($"Submitting answers for interviewId: {interviewId}, userId: {userId}");
                Console.WriteLine($"Answers: {string.Join(", ", answers)}");

                var result = await _interviewService.SubmitAnswersAsync(interviewId, answers);

                if (result == null)
                {
                    Console.WriteLine($"Unable to submit answers for interviewId: {interviewId}, userId: {userId}. Result is null.");
                    return StatusCode(404, "Unable to submit answers.");
                }

                Console.WriteLine($"Answers submitted successfully for interviewId: {interviewId}, userId: {userId}. Summary: {result}");

                return Ok(new { Summary = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error submitting answers for interviewId: {interviewId}, userId: {userId}. Error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
