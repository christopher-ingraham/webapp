using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;

namespace DA.WI.NSGHSM.IRepo.Production
{
    public interface IRmlCrewRepo<TDataSource>
    {
        ListResultDto<RmlCrewLookupDto> SelCrewForOutCoil(ListRequestDto<RmlCrewLookupDto> listRequest);
 
    }
}
