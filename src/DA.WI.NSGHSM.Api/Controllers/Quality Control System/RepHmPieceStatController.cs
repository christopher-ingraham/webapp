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
    public class RepHmPieceStatController : ControllerBase
    {
        private RepHmPieceStatLogic repHmPieceStatlogic = default(RepHmPieceStatLogic);

        public RepHmPieceStatController(RepHmPieceStatLogic repHmPieceStatlogic)
        {
            this.repHmPieceStatlogic = repHmPieceStatlogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<RepHmPieceStatListItemDto>), StatusCodes.Status200OK)]
        public IActionResult ReadList([FromQuery] ListRequestDto<RepHmPieceStatListFilterDto> listRequest)
        {
            ListResultDto<RepHmPieceStatListItemDto> result = repHmPieceStatlogic.ReadList(listRequest);

            return Ok(result);
        }
    }
}
