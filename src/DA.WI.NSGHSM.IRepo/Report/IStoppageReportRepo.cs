using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.Dto.Report;

namespace DA.WI.NSGHSM.IRepo.Report
{
    public interface IStoppageReportRepo<TDataSource>
    {
        ListResultDto<StoppageReportListItemDto> SelMainStoppageData(ListRequestDto<StoppageReportListFilterDto> listRequest);   
    }
}