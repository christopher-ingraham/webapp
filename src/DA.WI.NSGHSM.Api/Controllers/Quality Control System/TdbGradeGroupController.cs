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
    public class TdbGradeGroupController : ControllerBase
    {
        private TdbGradeGroupLogic TdbGradeGrouplogic = default(TdbGradeGroupLogic);

        public TdbGradeGroupController(TdbGradeGroupLogic TdbGradeGrouplogic)
        {
            this.TdbGradeGrouplogic = TdbGradeGrouplogic;
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<TdbGradeGroupListItemDto>), StatusCodes.Status200OK)]
        public IActionResult FillGradeGroup([FromQuery] ListRequestDto<TdbGradeGroupListFilterDto> listRequest)
        {
            ListResultDto<TdbGradeGroupListItemDto> result = TdbGradeGrouplogic.FillGradeGroup(listRequest);

            return Ok(result);
        }
    }

}
