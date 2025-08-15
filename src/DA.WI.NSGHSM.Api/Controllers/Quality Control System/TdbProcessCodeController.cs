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
    public class TdbProcessCodeController : ControllerBase
    {
        private TdbProcessCodeLogic TdbProcessCodelogic = default(TdbProcessCodeLogic);

        public TdbProcessCodeController(TdbProcessCodeLogic TdbProcessCodelogic)
        {
            this.TdbProcessCodelogic = TdbProcessCodelogic;
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<TdbProcessCodeListItemDto>), StatusCodes.Status200OK)]
        public IActionResult FillProcessCodesByCodeType([FromQuery] ListRequestDto<TdbProcessCodeListFilterDto> listRequest)
        {
            ListResultDto<TdbProcessCodeListItemDto> result = TdbProcessCodelogic.FillProcessCodesByCodeType(listRequest);

            return Ok(result);
        }
    }

}
