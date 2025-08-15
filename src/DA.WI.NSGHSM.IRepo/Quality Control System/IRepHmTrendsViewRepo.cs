using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface IRepHmTrendsViewRepo<TDataSource>
    {
        ListResultDto<RepHmTrendsViewListItemDto> ReadList(ListRequestDto<RepHmTrendsViewListFilterDto> listRequest);
    }
}