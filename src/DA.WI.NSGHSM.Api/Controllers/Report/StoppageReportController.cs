using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.Report;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Logic.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DA.WI.NSGHSM.Api.Controllers.Report
{
    [Route("report/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class StoppageReportController : ReportBaseController<StoppageReportController>
    {
        private StoppageReportLogic stoppageReportlogic = default(StoppageReportLogic);

        public StoppageReportController(
            IConfiguration iConfiguration,
            ILogger<StoppageReportController> logger,
            StoppageReportLogic stoppageReportlogic
        ): base(iConfiguration, logger) {
            this.stoppageReportlogic = stoppageReportlogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<StoppageReportListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelCoilData([FromQuery] ListRequestDto<StoppageReportListFilterDto> listRequest)
        {
            ListResultDto<StoppageReportListItemDto> result = stoppageReportlogic.SelMainStoppageData(listRequest);

            return Ok(result);
        }
    }

}