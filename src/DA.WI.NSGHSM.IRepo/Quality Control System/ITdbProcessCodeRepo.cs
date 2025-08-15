using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface ITdbProcessCodeRepo<TDataSource>
    {
        ListResultDto<TdbProcessCodeListItemDto> FillProcessCodesByCodeType(ListRequestDto<TdbProcessCodeListFilterDto> listRequest);
    }
}
