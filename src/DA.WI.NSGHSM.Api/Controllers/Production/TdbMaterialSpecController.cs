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
    public class TdbMaterialSpecController : ControllerBase
    {
        private TdbMaterialSpecLogic tdbMaterialSpecLogic = default(TdbMaterialSpecLogic);

        public TdbMaterialSpecController(TdbMaterialSpecLogic tdbMaterialSpecLogic)
        {
            this.tdbMaterialSpecLogic = tdbMaterialSpecLogic;
        }


        [HttpGet("lookup")]
        [ProducesResponseType(typeof(ListResultDto<TdbMaterialSpecLookupDto>), StatusCodes.Status200OK)]
        public IActionResult SelMaterialSpecList([FromQuery] ListRequestDto<TdbMaterialSpecListFilterDto> listRequest)
        {
            ListResultDto<TdbMaterialSpecLookupDto> result = tdbMaterialSpecLogic.SelMaterialSpecList(listRequest);

            return Ok(result);
        }
    }

}
