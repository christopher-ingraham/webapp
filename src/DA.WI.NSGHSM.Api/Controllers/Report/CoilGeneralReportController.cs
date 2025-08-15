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
    public class CoilGeneralReportController : ReportBaseController<CoilGeneralReportController>
    {
        private CoilGeneralReportLogic coilGeneralReportlogic = default(CoilGeneralReportLogic);

        public CoilGeneralReportController(
            IConfiguration iConfiguration,
            ILogger<CoilGeneralReportController> logger,
            CoilGeneralReportLogic coilGeneralReportlogic
        ) : base(iConfiguration, logger) {
            this.coilGeneralReportlogic = coilGeneralReportlogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<CoilGeneralReportListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelCoilData([FromQuery] ListRequestDto<CoilGeneralReportListFilterDto> listRequest)
        {
            ListResultDto<CoilGeneralReportListItemDto> result = coilGeneralReportlogic.SelCoilData(listRequest);

            return Ok(result);
        }
    }

}