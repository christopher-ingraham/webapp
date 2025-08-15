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
    public class RepHmPieceTrendController : ControllerBase
    {
        private RepHmPieceTrendLogic repHmPieceTrendlogic = default(RepHmPieceTrendLogic);

        public RepHmPieceTrendController(RepHmPieceTrendLogic repHmPieceTrendlogic)
        {
            this.repHmPieceTrendlogic = repHmPieceTrendlogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<RepHmPieceTrendListItemDto>), StatusCodes.Status200OK)]
        public IActionResult ReadList([FromQuery] ListRequestDto<RepHmPieceTrendListFilterDto> listRequest)
        {
            ListResultDto<RepHmPieceTrendListItemDto> result = repHmPieceTrendlogic.ReadList(listRequest);

            return Ok(result);
        }
    }
}
