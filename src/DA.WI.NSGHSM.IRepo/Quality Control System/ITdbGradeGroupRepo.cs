using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface ITdbGradeGroupRepo<TDataSource>
    {
        ListResultDto<TdbGradeGroupListItemDto> FillGradeGroup(ListRequestDto<TdbGradeGroupListFilterDto> listRequest);
    }
}
