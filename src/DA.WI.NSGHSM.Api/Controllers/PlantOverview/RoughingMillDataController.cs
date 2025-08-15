using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.PlantOverview;
using DA.WI.NSGHSM.Logic.PlantOverview;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace DA.WI.NSGHSM.Api.Controllers.PlantOverview
{
    [Route("plantoverview/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class RoughingMillDataController : ControllerBase
    {
        private RoughingMillDataLogic roughingMillDataLogic = default(RoughingMillDataLogic);

        public RoughingMillDataController(
            RoughingMillDataLogic roughingMillDataLogic
        ) {
            this.roughingMillDataLogic = roughingMillDataLogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(RoughingMillDataDto), StatusCodes.Status200OK)]
        public IActionResult SelRoughingMillData()
        {
            RoughingMillDataDto result = roughingMillDataLogic.SelRoughingMillData();

            return Ok(result);
        }
    }

}