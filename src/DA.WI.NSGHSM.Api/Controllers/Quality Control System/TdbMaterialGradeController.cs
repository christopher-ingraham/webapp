using DA.WI.NSGHSM.Api._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Logic.QualityControlSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DA.WI.NSGHSM.Api.Controllers.QualityControlSystem
{
    [Route("qcs/[controller]")]
    [ApiController]
    [AuthorizeAllRoles]
    public class TdbMaterialGradeController : ControllerBase
    {
        private TdbMaterialGradeLogic tdbMaterialGradelogic = default(TdbMaterialGradeLogic);

        public TdbMaterialGradeController(TdbMaterialGradeLogic tdbMaterialGradelogic)
        {
            this.tdbMaterialGradelogic = tdbMaterialGradelogic;
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(ListResultDto<TdbMaterialGradeListItemDto>), StatusCodes.Status200OK)]
        public IActionResult SelMatGradeForInPiece([FromQuery] ListRequestDto<TdbMaterialGradeListFilterDto> listRequest)
        {
            ListResultDto<TdbMaterialGradeListItemDto> result = tdbMaterialGradelogic.SelMatGradeForInPiece(listRequest);

            return Ok(result);
        }

        [HttpGet("lookup")]
        [ProducesResponseType(typeof(ListRequestDto<TdbMaterialGradeLookupDto>), StatusCodes.Status200OK)]
        public IActionResult FillMaterialGrade([FromQuery] ListRequestDto<TdbMaterialGradeLookupDto> listRequest)
        {
            ListResultDto<TdbMaterialGradeLookupDto> result = tdbMaterialGradelogic.FillMaterialGrade(listRequest);

            return Ok(result);
        }
    }

}
