using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Logic.QualityControlSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DA.WI.NSGHSM.Api.Controllers.QualityControlSystem
{
    [Route("qcs/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class RepHmMapsViewController : ControllerBase
    {
        private RepHmMapsViewLogic RepHmMapsViewlogic = default(RepHmMapsViewLogic);

        public RepHmMapsViewController(RepHmMapsViewLogic RepHmMapsViewlogic)
        {
            this.RepHmMapsViewlogic = RepHmMapsViewlogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<RepHmMapsViewListItemDto>), StatusCodes.Status200OK)]
        public IActionResult ReadList([FromQuery] ListRequestDto<RepHmMapsViewListFilterDto> listRequest)
        {
            ListResultDto<RepHmMapsViewListItemDto> result = RepHmMapsViewlogic.ReadList(listRequest);

            return Ok(result);
        }
    }
}