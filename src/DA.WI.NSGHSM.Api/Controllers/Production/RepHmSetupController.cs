using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.Logic.Production;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace DA.WI.NSGHSM.Api.Controllers.Production
{
    [Route("production/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class RepHmSetupController : ControllerBase
    {
        private RepHmSetupLogic repHmSetuplogic = default(RepHmSetupLogic);

        public RepHmSetupController(RepHmSetupLogic repHmSetuplogic)
        {
            this.repHmSetuplogic = repHmSetuplogic;
        }

        [HttpGet("{inPieceNo}")]
        [ProducesResponseType(typeof(RepHmSetupDto), StatusCodes.Status200OK)]
        public IActionResult GetCurrentOutputCoil([FromRoute]int inPieceNo)
        {
            RepHmSetupDto result = repHmSetuplogic.GetCurrentREPSetupHeader(inPieceNo);
            return Ok(result);
        }
    }
}
