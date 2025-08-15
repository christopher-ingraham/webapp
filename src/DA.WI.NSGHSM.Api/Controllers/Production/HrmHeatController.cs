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
    public class HrmHeatController : ControllerBase
    {
        private HrmHeatLogic hrmHeatlogic = default(HrmHeatLogic);

        public HrmHeatController(HrmHeatLogic hrmHeatlogic)
        {
            this.hrmHeatlogic = hrmHeatlogic;
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<HrmHeatListItemDto>), StatusCodes.Status200OK)]
        public IActionResult FillAllHeatIdList([FromQuery] ListRequestDto<HrmHeatListFilterDto> listRequest)
        {
            ListResultDto<HrmHeatListItemDto> result = hrmHeatlogic.FillAllHeatIdList(listRequest);

            return Ok(result);
        }
    }

}
