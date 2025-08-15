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
    public class TdbAlloySpecController : ControllerBase
    {
        private TdbAlloySpecLogic TdbAlloySpeclogic = default(TdbAlloySpecLogic);

        public TdbAlloySpecController(TdbAlloySpecLogic TdbAlloySpeclogic)
        {
            this.TdbAlloySpeclogic = TdbAlloySpeclogic;
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<TdbAlloySpecListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelTdbAlloySpec([FromQuery] ListRequestDto<TdbAlloySpecListFilterDto> listRequest)
        {
            ListResultDto<TdbAlloySpecListItemDto> result = TdbAlloySpeclogic.SelTdbAlloySpec(listRequest);

            return Ok(result);
        }

        [HttpGet("{AlloySpecCnt}")]
        [ProducesResponseType(typeof(TdbAlloySpecDetailDto), StatusCodes.Status200OK)]
        public IActionResult GetCurrentTdbAlloy([FromRoute] int AlloySpecCnt)
        {
            TdbAlloySpecDetailDto result = TdbAlloySpeclogic.GetTdbAlloySpec(AlloySpecCnt);

            return Ok(result);
        }
    }

}
