using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Logic.Production;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace DA.WI.NSGHSM.Api.Controllers.Production
{
    [Route("production/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class ExitSaddlesController : ControllerBase
    {
        private ExitSaddlesLogic exitSaddleslogic = default(ExitSaddlesLogic);

        public ExitSaddlesController(ExitSaddlesLogic exitSaddleslogic)
        {
            this.exitSaddleslogic = exitSaddleslogic;
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<ExitSaddlesListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelExitSaddlesMap([FromQuery] ListRequestDto<ExitSaddlesListFilterDto> listRequest)
        {
            ListResultDto<ExitSaddlesListItemDto> result = exitSaddleslogic.SelExitSaddlesMap(listRequest);

            return Ok(result);
        }
    }

}
