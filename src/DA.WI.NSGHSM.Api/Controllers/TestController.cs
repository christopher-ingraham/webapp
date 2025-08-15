using DA.WI.NSGHSM.Api._Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DA.WI.NSGHSM.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> logger;
        
        public TestController(ILogger<TestController> logger)
        {
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("ping")]
        [ProducesResponseType(typeof(PingResult), StatusCodes.Status200OK)]
        public IActionResult Ping()
        {
            logger.LogInformation("Ping Occurred");
            return Ok(new PingResult { Response = "pong" });
        }

        public class PingResult
        {
            public string Response { get; set; }
        }
    }
}