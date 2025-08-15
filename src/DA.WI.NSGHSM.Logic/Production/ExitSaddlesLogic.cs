using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Production
{

    public class ExitSaddlesLogic
    {
        private IExitSaddlesRepo<ReportDataSource> exitSaddlesRepo;

        public ExitSaddlesLogic(IExitSaddlesRepo<ReportDataSource> exitSaddlesRepo)
        {
            this.exitSaddlesRepo = exitSaddlesRepo;
        }

        public ListResultDto<ExitSaddlesListItemDto> SelExitSaddlesMap(ListRequestDto<ExitSaddlesListFilterDto> listRequest)
        {
            return exitSaddlesRepo.SelExitSaddlesMap(listRequest);
        }
    }
}