using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.Dto.Report;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.Report;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Report
{

    public class PracticeReportLogic
    {
        private IPracticeReportRepo<ReportDataSource> practiceReportRepo;


        public PracticeReportLogic(IPracticeReportRepo<ReportDataSource> practiceReportRepo)
        {
            this.practiceReportRepo = practiceReportRepo;
        }

        public ListResultDto<PracticeReportListItemDto> SelPracticeData(ListRequestDto<PracticeReportListFilterDto> listRequest)
        {
            return practiceReportRepo.SelPracticeData(listRequest);
        }
    }
}