using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface IRepHmPieceStatRepo<TDataSource>
    {
        ListResultDto<RepHmPieceStatListItemDto> ReadList(ListRequestDto<RepHmPieceStatListFilterDto> listRequest);
    }
}