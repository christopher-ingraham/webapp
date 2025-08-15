using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Production
{

    public class RepHmSetupLogic
    {
        private IRepHmSetupRepo<ReportDataSource> repHmSetupRepo;


        public RepHmSetupLogic(IRepHmSetupRepo<ReportDataSource> repHmSetupRepo)
        {
            this.repHmSetupRepo = repHmSetupRepo;
        }

        public RepHmSetupDto GetCurrentREPSetupHeader(int inPieceNo)
        {
            return repHmSetupRepo.GetCurrentREPSetupHeader(inPieceNo);
        }
    }
}