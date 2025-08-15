using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.AuxValue;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Logic.AuxValue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DA.WI.NSGHSM.Api.Controllers.AuxValue
{
    [Route("auxvalue/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class AuxValueController : ControllerBase
    {
        private AuxValueLogic auxValuelogic = default(AuxValueLogic);

        public AuxValueController(AuxValueLogic auxValuelogic)
        {
            this.auxValuelogic = auxValuelogic;
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<AuxValueListItemDto>), StatusCodes.Status200OK)]
        public IActionResult ReadList([FromQuery] ListRequestDto<AuxValueListFilterDto> listRequest)
        {
            ListResultDto<AuxValueListItemDto> result = auxValuelogic.SelAuxIntegerValue(listRequest);

            return Ok(result);
        }
    }
}
