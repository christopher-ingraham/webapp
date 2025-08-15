using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.AuxValue;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Logic.AuxValue;
using DA.WI.NSGHSM.Logic.Production;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace DA.WI.NSGHSM.Api.Controllers.Production
{
    [Route("production/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class ProducedCoilsController : ControllerBase
    {
        private ProducedCoilsLogic producedCoilslogic = default(ProducedCoilsLogic);
        private AuxValueLogic auxValueLogic = default(AuxValueLogic);

        public ProducedCoilsController(ProducedCoilsLogic producedCoilslogic, AuxValueLogic auxValueLogic)
        {
            this.producedCoilslogic = producedCoilslogic;
            this.auxValueLogic = auxValueLogic;
        }



        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<ProducedCoilsListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelProdCoils([FromQuery] ListRequestDto<ProducedCoilsListFilterDto> listRequest)
        {
            ListResultDto<ProducedCoilsListItemDto> result = producedCoilslogic.SelProdCoils(listRequest);

            return Ok(result);
        }

        [HttpGet("{outPieceNo}")]
        [ProducesResponseType(typeof(ProducedCoilsDetailDto), StatusCodes.Status200OK)]
        public IActionResult GetCurrentOutputCoil([FromRoute]long outPieceNo)
        {
            ProducedCoilsDetailDto result = producedCoilslogic.GetCurrentOutputCoil(outPieceNo);
            return Ok(result);
        }

        [HttpPut("{outPieceNo}")]
        [ProducesResponseType(typeof(ProducedCoilsForUpdateDto), StatusCodes.Status200OK)]
        [AuthorizeRolesAllRestricted]
        public IActionResult UpdRepHmProdCoil([FromRoute]long outPieceNo, [FromBody]ProducedCoilsForUpdateDto dto)
        {
            producedCoilslogic.UpdRepHmProdCoil(dto, outPieceNo);
            return Ok();
        }

        [HttpPost("new")]
        [ProducesResponseType(typeof(ProducedCoilsForInsertDto), StatusCodes.Status201Created)]
        [AuthorizeRoleAll]
        public IActionResult Create([FromBody]ProducedCoilsForInsertDto dto)
        {
            ProducedCoilsDetailDto result = producedCoilslogic.InsRepHmProdCoil(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthorizeRoleAll]
        public IActionResult Delete([FromRoute]long id)
        {
            producedCoilslogic.Delete(id);
            return Ok("Deleted OutPieceNo = "+id);
        }

        [HttpGet("lookup")]
        [ProducesResponseType(typeof(ListRequestDto<ProducedCoilsLookupDto>), StatusCodes.Status200OK)]
        public IActionResult SelInPieceForProdCoil([FromQuery] ListRequestDto<ProducedCoilsLookupDto> listRequest)
        {
            ListResultDto<ProducedCoilsLookupDto> result = producedCoilslogic.Lookup(listRequest);

            return Ok(result);
        }

        [HttpGet("validation")]
        [ProducesResponseType(typeof(CustomValidationDTO), StatusCodes.Status200OK)]
        public IActionResult getCustomValidation()
        {
            var list = buildList();
            CustomValidationDTO result = auxValueLogic.getCustomValidation(AuxValidationConstants.AUX_RANGE_PHY, list);

            return Ok(result);
        }

        private Dictionary<KeyValuePair<string, string>,KeyValuePair<string, string>> buildList(){
            Dictionary<KeyValuePair<string, string>, KeyValuePair<string, string>> constantsDictionary = new Dictionary<KeyValuePair<string, string>,KeyValuePair<string, string>>();

            constantsDictionary.Add(new KeyValuePair<string, string>("exitThk", AuxValidationConstants.AUX_RANGE_EX_THK), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("exitWidth", AuxValidationConstants.AUX_RANGE_WDT), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("measuredWeight", AuxValidationConstants.AUX_RANGE_WGT), new KeyValuePair<string, string>("kg", "lb"));
            constantsDictionary.Add(new KeyValuePair<string, string>("calculatedWeight", AuxValidationConstants.AUX_RANGE_WGT), new KeyValuePair<string, string>("kg", "lb"));
            constantsDictionary.Add(new KeyValuePair<string, string>("length", AuxValidationConstants.AUX_RANGE_LGT), new KeyValuePair<string, string>("m", "ft"));
            constantsDictionary.Add(new KeyValuePair<string, string>("outerDiameter", AuxValidationConstants.AUX_RANGE_EXT_DIAM), new KeyValuePair<string, string>("mm", "in"));

            return constantsDictionary;
        }

    }

}
