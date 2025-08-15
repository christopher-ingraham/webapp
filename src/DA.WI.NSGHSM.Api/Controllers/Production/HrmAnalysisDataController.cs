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
    public class HrmAnalysisDataController : ControllerBase
    {
        private HrmAnalysisDataLogic hrmAnalysisDatalogic = default(HrmAnalysisDataLogic);

        public HrmAnalysisDataController(HrmAnalysisDataLogic hrmAnalysisDatalogic)
        {
            this.hrmAnalysisDatalogic = hrmAnalysisDatalogic;
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<HrmAnalysisDataListItemDto>), StatusCodes.Status200OK)]
        public IActionResult LoadHrmSampleId([FromQuery] ListRequestDto<HrmAnalysisDataListFilterDto> listRequest)
        {
            ListResultDto<HrmAnalysisDataListItemDto> result = hrmAnalysisDatalogic.LoadHrmSampleId(listRequest);

            return Ok(result);
        }

        [HttpGet("{analysisCnt}")]
        [ProducesResponseType(typeof(HrmAnalysisDataDetailDto), StatusCodes.Status200OK)]
        public IActionResult Read([FromRoute] int analysisCnt)
        {
            return Ok(hrmAnalysisDatalogic.Read(analysisCnt));
        }
    }

}
