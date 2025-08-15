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
    public class PracticeReportController : ReportBaseController<PracticeReportController>
    {
        private PracticeReportLogic practiceReportlogic = default(PracticeReportLogic);

        public PracticeReportController(
            IConfiguration iConfiguration,
            ILogger<PracticeReportController> logger,
            PracticeReportLogic practiceReportlogic
        ): base(iConfiguration, logger) {
            this.practiceReportlogic = practiceReportlogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<PracticeReportListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelCoilData([FromQuery] ListRequestDto<PracticeReportListFilterDto> listRequest)
        {
            ListResultDto<PracticeReportListItemDto> result = practiceReportlogic.SelPracticeData(listRequest);

            return Ok(result);
        }
    }

}