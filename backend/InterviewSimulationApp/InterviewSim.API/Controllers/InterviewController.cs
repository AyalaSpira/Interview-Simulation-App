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
        public async Task<IActionResult> StartInterview(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var result = await _interviewService.StartInterviewAsync(userId);
                return Ok(new { Message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint לשליחת תשובות לראיון
        [HttpPost("submit-answers")]
        public async Task<IActionResult> SubmitAnswers(int interviewId, int userId, [FromBody] List<string> answers)
        {
            if (interviewId <= 0 || userId <= 0 || answers == null || answers.Count == 0)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var result = await _interviewService.SubmitAnswersAsync(interviewId, answers);
                return Ok(new { Summary = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}