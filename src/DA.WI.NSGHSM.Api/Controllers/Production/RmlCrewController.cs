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
    public class RmlCrewController : ControllerBase
    {
        private RmlCrewLogic rmlCrewLogic = default(RmlCrewLogic);

        public RmlCrewController(RmlCrewLogic rmlCrewLogic)
        {

            this.rmlCrewLogic = rmlCrewLogic;
        }

        [HttpGet("lookup")]
        [ProducesResponseType(typeof(ListRequestDto<RmlCrewLookupDto>), StatusCodes.Status200OK)]
        public IActionResult SelInPieceForProdCoil([FromQuery] ListRequestDto<RmlCrewLookupDto> listRequest)
        {
            ListResultDto<RmlCrewLookupDto> result = rmlCrewLogic.SelCrewForOutCoil(listRequest);

            return Ok(result);
        }

    }

}
