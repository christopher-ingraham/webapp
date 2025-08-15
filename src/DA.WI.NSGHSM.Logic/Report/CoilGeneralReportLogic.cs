using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.Dto.Report;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.Report;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Report
{

    public class CoilGeneralReportLogic
    {
        private ICoilGeneralReportRepo<ReportDataSource> coilGeneralReportRepo;


        public CoilGeneralReportLogic(ICoilGeneralReportRepo<ReportDataSource> coilGeneralReportRepo)
        {
            this.coilGeneralReportRepo = coilGeneralReportRepo;
        }

        public ListResultDto<CoilGeneralReportListItemDto> SelCoilData(ListRequestDto<CoilGeneralReportListFilterDto> listRequest)
        {
            return coilGeneralReportRepo.SelCoilData(listRequest);
        }
    }
}