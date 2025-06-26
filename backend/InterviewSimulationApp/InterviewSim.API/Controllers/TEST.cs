using Microsoft.AspNetCore.Mvc;

namespace InterviewSim.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API עובד!");
        }
    }
}


