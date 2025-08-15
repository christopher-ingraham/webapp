using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.Logic.QualityControlSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace DA.WI.NSGHSM.Api.Controllers.QualityControlSystem
{
    [Route("qcs/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class TdbAlloyController : ControllerBase
    {
        private TdbAlloyLogic TdbAlloylogic = default(TdbAlloyLogic);

        public TdbAlloyController(TdbAlloyLogic TdbAlloylogic)
        {
            this.TdbAlloylogic = TdbAlloylogic;
        }


        [HttpGet("{AlloyCode}")]
        [ProducesResponseType(typeof(TdbAlloyDto), StatusCodes.Status200OK)]
        public IActionResult GetCurrentTdbAlloy([FromRoute] int AlloyCode)
        {
            TdbAlloyDto result = TdbAlloylogic.GetCurrentTdbAlloy(AlloyCode);

            return Ok(result);
        }
    }

}
