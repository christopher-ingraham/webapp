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
    public class HrmInputPieceController : ControllerBase
    {
        private HrmInputPieceLogic hrmInputPieceLogic = default(HrmInputPieceLogic);
        private AuxValueLogic auxValueLogic = default(AuxValueLogic);

        public HrmInputPieceController(HrmInputPieceLogic hrmInputPieceLogic, AuxValueLogic auxValueLogic)
        {
            this.hrmInputPieceLogic = hrmInputPieceLogic;
            this.auxValueLogic = auxValueLogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<HrmInputPieceListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelInputPiecesList([FromQuery] ListRequestDto<HrmInputPieceListFilterDto> listRequest)
        {
            ListResultDto<HrmInputPieceListItemDto> result = hrmInputPieceLogic.SelInputPiecesList(listRequest);

            return Ok(result);
        }

        [HttpGet("{pieceNo}")]
        [ProducesResponseType(typeof(HrmInputPieceDetailDto), StatusCodes.Status200OK)]
        public IActionResult Read([FromRoute]int pieceNo)
        {
            HrmInputPieceDetailDto result = hrmInputPieceLogic.GetCurrentInputPieceMng(pieceNo);
            return Ok(result);
        }

        [HttpPost("new")]
        [ProducesResponseType(typeof(HrmInputPieceDetailDto), StatusCodes.Status201Created)]
        [AuthorizeRoleAll]
        public IActionResult Create([FromBody]HrmInputPieceForInsertDto dto)
        {
            hrmInputPieceLogic.Create(dto);
            return Ok();
        }

        [HttpPut("{pieceNo}")]
        [ProducesResponseType(typeof(HrmInputPieceForInsertDto), StatusCodes.Status200OK)]
        [AuthorizeRolesAllRestricted]
        public IActionResult Update([FromRoute]int pieceNo, [FromBody]HrmInputPieceForUpdateDto dto)
        {
            hrmInputPieceLogic.Update(dto, pieceNo);
            return Ok();
        }

        [HttpDelete("{pieceNo}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthorizeRoleAll]
        public IActionResult Delete([FromRoute]int pieceNo)
        {
            hrmInputPieceLogic.Delete(pieceNo);
            return NoContent();
        }

        [HttpGet("lookup")]
        [ProducesResponseType(typeof(ListRequestDto<HrmInputPieceLookupDto>), StatusCodes.Status200OK)]
        public IActionResult SelLookupCoil([FromQuery] ListRequestDto<HrmInputPieceLookupDto> listRequest)
        {
            ListResultDto<HrmInputPieceLookupDto> result = hrmInputPieceLogic.Lookup(listRequest);

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

        private Dictionary<KeyValuePair<string, string>, KeyValuePair<string, string>> buildList(){
            Dictionary<KeyValuePair<string, string>, KeyValuePair<string, string>> constantsDictionary = new Dictionary<KeyValuePair<string, string>, KeyValuePair<string, string>>();

            constantsDictionary.Add(new KeyValuePair<string, string>("thicknessHead", AuxValidationConstants.AUX_RANGE_THK), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("thicknessTail", AuxValidationConstants.AUX_RANGE_EX_THK), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("targetThickness", AuxValidationConstants.AUX_RANGE_EX_THK), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("targetThicknessPtol", AuxValidationConstants.AUX_RANGE_EX_THK), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("targetThicknessNtol", AuxValidationConstants.AUX_RANGE_THK), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("widthHead", AuxValidationConstants.AUX_RANGE_WDT), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("widthTail", AuxValidationConstants.AUX_RANGE_WDT), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("measuredWidthHead", AuxValidationConstants.AUX_RANGE_WDT), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("measuredWidthTail", AuxValidationConstants.AUX_RANGE_WDT), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("targetWidth", AuxValidationConstants.AUX_RANGE_WDT), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("weight", AuxValidationConstants.AUX_RANGE_WGT), new KeyValuePair<string, string>("kg", "lb"));
            constantsDictionary.Add(new KeyValuePair<string, string>("targetWeight", AuxValidationConstants.AUX_RANGE_WGT), new KeyValuePair<string, string>("kg", "lb"));
            constantsDictionary.Add(new KeyValuePair<string, string>("length", AuxValidationConstants.AUX_RANGE_LGT), new KeyValuePair<string, string>("m", "ft"));
            constantsDictionary.Add(new KeyValuePair<string, string>("entryTemp", AuxValidationConstants.AUX_RANGE_EN_TEMP), new KeyValuePair<string, string>("C", "F"));
            constantsDictionary.Add(new KeyValuePair<string, string>("measuredTemp", AuxValidationConstants.AUX_RANGE_EN_TEMP), new KeyValuePair<string, string>("C", "F"));
            constantsDictionary.Add(new KeyValuePair<string, string>("furnaceDischargeTemp", AuxValidationConstants.AUX_RANGE_DISCH_TEMP), new KeyValuePair<string, string>("C", "F"));
            constantsDictionary.Add(new KeyValuePair<string, string>("targetTempDC", AuxValidationConstants.AUX_RANGE_COILING_TEMP), new KeyValuePair<string, string>("C", "F"));
            constantsDictionary.Add(new KeyValuePair<string, string>("targetProfile", AuxValidationConstants.AUX_RANGE_CROWN), new KeyValuePair<string, string>("mm", "in"));
            constantsDictionary.Add(new KeyValuePair<string, string>("targetFlatness", AuxValidationConstants.AUX_RANGE_FLATNESS), new KeyValuePair<string, string>("I-Units", "I-Units"));

            return constantsDictionary;
        }
    }

}
