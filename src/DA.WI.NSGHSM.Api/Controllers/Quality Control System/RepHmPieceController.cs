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
    public class RepHmPieceController : ControllerBase
    {
        private RepHmPieceLogic repHmPiecelogic = default(RepHmPieceLogic);

        public RepHmPieceController(RepHmPieceLogic repHmPiecelogic)
        {
            
            this.repHmPiecelogic = repHmPiecelogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<RepHmPieceListItemDto>), StatusCodes.Status200OK)]
        public IActionResult ReadList([FromQuery] ListRequestDto<RepHmPieceListFilterDto> listRequest)
        {
            ListResultDto<RepHmPieceListItemDto> result = repHmPiecelogic.ReadList(listRequest);

            return Ok(result);
        }
    }
}