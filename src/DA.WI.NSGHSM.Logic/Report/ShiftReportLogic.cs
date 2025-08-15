using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.Dto.Report;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.Report;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Report
{

    public class ShiftReportLogic
    {
        private IShiftReportRepo<ReportDataSource> shiftReportRepo;


        public ShiftReportLogic(IShiftReportRepo<ReportDataSource> shiftReportRepo)
        {
            this.shiftReportRepo = shiftReportRepo;
        }

        public ListResultDto<ShiftReportListItemDto> SelShiftSummary(ListRequestDto<ShiftReportListFilterDto> listRequest)
        {
            return shiftReportRepo.SelShiftSummary(listRequest);
        }
    }
}