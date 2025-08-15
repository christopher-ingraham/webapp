using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;

namespace DA.WI.NSGHSM.IRepo.Production
{
    public interface IUsedSetupRepo<TDataSource>
    {
        ListResultDto<UsedSetupListItemDto> SelUsedSetup(ListRequestDto<UsedSetupListFilterDto> listRequest);   
    }
}