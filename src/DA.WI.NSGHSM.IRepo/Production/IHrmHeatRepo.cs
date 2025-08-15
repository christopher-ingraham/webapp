using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;

namespace DA.WI.NSGHSM.IRepo.Production
{
    public interface IHrmHeatRepo<TDataSource>
    {
        ListResultDto<HrmHeatListItemDto> FillAllHeatIdList(ListRequestDto<HrmHeatListFilterDto> listRequest);
    }
}
