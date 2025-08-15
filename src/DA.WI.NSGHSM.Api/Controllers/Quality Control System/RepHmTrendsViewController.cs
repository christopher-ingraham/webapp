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
    public class RepHmTrendsViewController : ControllerBase
    {
        private RepHmTrendsViewLogic repHmTrendsViewlogic = default(RepHmTrendsViewLogic);

        public RepHmTrendsViewController(RepHmTrendsViewLogic repHmTrendsViewlogic)
        {
            this.repHmTrendsViewlogic = repHmTrendsViewlogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<RepHmTrendsViewListItemDto>), StatusCodes.Status200OK)]
        public IActionResult ReadList([FromQuery] ListRequestDto<RepHmTrendsViewListFilterDto> listRequest)
        {
            ListResultDto<RepHmTrendsViewListItemDto> result = repHmTrendsViewlogic.ReadList(listRequest);

            return Ok(result);
        }
    }
}