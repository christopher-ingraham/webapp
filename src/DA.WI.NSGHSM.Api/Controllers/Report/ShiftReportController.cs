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
    public class ShiftReportController : ReportBaseController<ShiftReportController>
    {
        private ShiftReportLogic shiftReportlogic = default(ShiftReportLogic);

        public ShiftReportController(
            IConfiguration iConfiguration,
            ILogger<ShiftReportController> logger,
            ShiftReportLogic shiftReportlogic
        ): base(iConfiguration, logger) {
            this.shiftReportlogic = shiftReportlogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<ShiftReportListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelCoilData([FromQuery] ListRequestDto<ShiftReportListFilterDto> listRequest)
        {
            ListResultDto<ShiftReportListItemDto> result = shiftReportlogic.SelShiftSummary(listRequest);

            return Ok(result);
        }
    }

}