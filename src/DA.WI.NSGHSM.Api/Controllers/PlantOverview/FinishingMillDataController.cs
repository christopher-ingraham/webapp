using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.PlantOverview;
using DA.WI.NSGHSM.Logic.PlantOverview;
using DA.WI.NSGHSM.Logic.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace DA.WI.NSGHSM.Api.Controllers.PlantOverview
{
    [Route("plantoverview/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class FinishingMillDataController : ControllerBase
    {
        private FinishingMillDataLogic finishingMillDataLogic = default(FinishingMillDataLogic);

        public FinishingMillDataController(FinishingMillDataLogic finishingMillDataLogic)
        {
            this.finishingMillDataLogic = finishingMillDataLogic;
            this.messagingLogic = messagingLogic;
        }

        public MessagingLogic messagingLogic { get; }

        [HttpGet("list")]
        [ProducesResponseType(typeof(FinishingMillDataDto), StatusCodes.Status200OK)]
        public IActionResult SelFinishingMillData()
        {
            FinishingMillDataDto result = finishingMillDataLogic.SelFinishingMillData();

            return Ok(result);
        }
    }

}