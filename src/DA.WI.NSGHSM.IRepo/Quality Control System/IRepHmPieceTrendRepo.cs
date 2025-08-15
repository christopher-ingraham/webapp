using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface IRepHmPieceTrendRepo<TDataSource>
    {
        ListResultDto<RepHmPieceTrendListItemDto> ReadList(ListRequestDto<RepHmPieceTrendListFilterDto> listRequest);
    }
}