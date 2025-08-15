using DA.WI.NSGHSM.Dto._Core.Configuration;
using DA.WI.NSGHSM.Logic._Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DA.WI.NSGHSM.Api._Core.Configuration.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> logger;

        private UserLogic logic;

        public UserController(UserLogic logic, ILogger<UserController> logger)
        {
            this.logic = logic;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public IActionResult Get([FromRoute]long id)
        {
            var result = this.logic.Read(id);

            return Ok(result);
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var result = logic.Read();

            return Ok(result);
        }

        [HttpPost]
        [AuthorizeRoleAll]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        public IActionResult Post([FromBody]UserCreateDto dto)
        {
            var result = logic.Create(dto);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
    }
}