using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.Dto.Report;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.Report;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Report
{

    public class StoppageReportLogic
    {
        private IStoppageReportRepo<ReportDataSource> stoppageReportRepo;


        public StoppageReportLogic(IStoppageReportRepo<ReportDataSource> stoppageReportRepo)
        {
            this.stoppageReportRepo = stoppageReportRepo;
        }

        public ListResultDto<StoppageReportListItemDto> SelMainStoppageData(ListRequestDto<StoppageReportListFilterDto> listRequest)
        {
            return stoppageReportRepo.SelMainStoppageData(listRequest);
        }
    }
}