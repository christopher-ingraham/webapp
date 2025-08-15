using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.PlantOverview;
using DA.WI.NSGHSM.Logic.Messaging;
using DA.WI.NSGHSM.Logic.PlantOverview;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace DA.WI.NSGHSM.Api.Controllers.PlantOverview
{
    [Route("plantoverview/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class CoolingDowncoilersDataController : ControllerBase
    {
        private CoolingDowncoilersDataLogic coolingDowncoilersDataLogic = default(CoolingDowncoilersDataLogic);

        public CoolingDowncoilersDataController(CoolingDowncoilersDataLogic coolingDowncoilersDataLogic) 
        {
            this.coolingDowncoilersDataLogic = coolingDowncoilersDataLogic;
            this.messagingLogic = messagingLogic;
        }
        public MessagingLogic messagingLogic { get; }

        [HttpGet("list")]
        [ProducesResponseType(typeof(CoolingDowncoilersDataDto), StatusCodes.Status200OK)]
        public IActionResult SelCoolingDowncoilersData()
        {
            CoolingDowncoilersDataDto result = coolingDowncoilersDataLogic.SelCoolingDowncoilersData();

            return Ok(result);
        }
    }

}
