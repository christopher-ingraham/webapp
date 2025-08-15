using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface ITdbAlloySpecRepo<TDataSource>
    {
        ListResultDto<TdbAlloySpecListItemDto> SelTdbAlloySpec(ListRequestDto<TdbAlloySpecListFilterDto> listRequest);

        TdbAlloySpecDetailDto GetTdbAlloySpec(int AlloySpecCnt);
    }
}
