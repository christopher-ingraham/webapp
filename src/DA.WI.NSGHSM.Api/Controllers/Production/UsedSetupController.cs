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
    public class UsedSetupController : ControllerBase
    {
        private UsedSetupLogic usedSetuplogic = default(UsedSetupLogic);

        public UsedSetupController(UsedSetupLogic usedSetuplogic)
        {
            this.usedSetuplogic = usedSetuplogic;
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<UsedSetupListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelUsedSetup([FromQuery] ListRequestDto<UsedSetupListFilterDto> listRequest)
        {
            ListResultDto<UsedSetupListItemDto> result = usedSetuplogic.SelUsedSetup(listRequest);

            return Ok(result);
        }
    }


}
