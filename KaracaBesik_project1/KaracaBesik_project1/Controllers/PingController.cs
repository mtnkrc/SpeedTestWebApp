using Microsoft.AspNetCore.Mvc;

namespace KaracaBesik_project1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        private readonly ILogger<PingController> _logger;

        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Ping() 
        {
            try 
            {
                _logger.LogInformation("Request received for ping aat {Time}", DateTime.UtcNow);

                var response = new
                {
                    Message = "PingPong",
                    Timestamp = DateTime.UtcNow
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while ping request.");
                return StatusCode(500);
            }
        
        }
    }
}
