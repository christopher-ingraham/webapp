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
    public class HrmJobController : ControllerBase
    {
        private HrmJobLogic hrmJoblogic = default(HrmJobLogic);

        public HrmJobController(HrmJobLogic hrmJoblogic)
        {
            this.hrmJoblogic = hrmJoblogic;
        }


        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<HrmJobListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelJobsList([FromQuery] ListRequestDto<HrmJobListFilterDto> listRequest)
        {
            ListResultDto<HrmJobListItemDto> result = hrmJoblogic.SelJobsList(listRequest);

            return Ok(result);
        }

        [HttpGet("lookup")]
        [ProducesResponseType(typeof(ListRequestDto<HrmJobLookupDto>), StatusCodes.Status200OK)]
        public IActionResult SelInPieceForProdCoil([FromQuery] ListRequestDto<HrmJobLookupDto> listRequest)
        {
            ListResultDto<HrmJobLookupDto> result = hrmJoblogic.Lookup(listRequest);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HrmJobDetailDto), StatusCodes.Status200OK)]
        public IActionResult SelJobById([FromRoute]string id)
        {
            HrmJobDetailDto result = hrmJoblogic.SelJobById(id);
            return Ok(result);
        }

        [HttpGet("onlyjob/{id}")]
        [ProducesResponseType(typeof(HrmJobDetailDto), StatusCodes.Status200OK)]
        public IActionResult Read([FromRoute]string id)
        {
            HrmJobDetailDto result = hrmJoblogic.Read(id);
            return Ok(result);
        }


        [HttpPost("new")]
        [ProducesResponseType(typeof(HrmJobDetailDto), StatusCodes.Status201Created)]
        [AuthorizeRoleAll]
        public IActionResult Create([FromBody]HrmJobForInsertDto dto)
        {
            HrmJobDetailDto result = hrmJoblogic.Create(dto);
            return CreatedAtAction(nameof(Read), new { id = result.JobId }, result);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HrmJobDetailDto), StatusCodes.Status200OK)]
        [AuthorizeRolesAllRestricted]
        public IActionResult Update([FromRoute]string id, [FromBody]HrmJobForUpdateDto dto)
        {
            HrmJobDetailDto result = hrmJoblogic.Update(dto,id);
            return Ok(result);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthorizeRoleAll]
        public IActionResult Delete([FromRoute]string id)
        {
            hrmJoblogic.Delete(id);
            return NoContent();
        }
    }

}
