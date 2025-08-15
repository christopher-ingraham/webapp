using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.Dto.Report;

namespace DA.WI.NSGHSM.IRepo.Report
{
    public interface IPracticeReportRepo<TDataSource>
    {
        ListResultDto<PracticeReportListItemDto> SelPracticeData(ListRequestDto<PracticeReportListFilterDto> listRequest);   
    }
}