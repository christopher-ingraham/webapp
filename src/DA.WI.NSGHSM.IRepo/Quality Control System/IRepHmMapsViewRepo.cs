using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface IRepHmMapsViewRepo<TDataSource>
    {
        ListResultDto<RepHmMapsViewListItemDto> ReadList(ListRequestDto<RepHmMapsViewListFilterDto> listRequest);
    }
}