using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface ITdbMaterialGradeRepo<TDataSource>
    {
        ListResultDto<TdbMaterialGradeListItemDto> SelMatGradeForInPiece(ListRequestDto<TdbMaterialGradeListFilterDto> listRequest);
        ListResultDto<TdbMaterialGradeLookupDto> FillMaterialGrade(ListRequestDto<TdbMaterialGradeLookupDto> listRequest);
    }
}
