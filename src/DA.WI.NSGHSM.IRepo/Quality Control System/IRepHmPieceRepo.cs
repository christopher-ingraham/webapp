using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface IRepHmPieceRepo<TDataSource>
    {

        ListResultDto<RepHmPieceListItemDto> ReadList(ListRequestDto<RepHmPieceListFilterDto> listRequest);
    }
}
