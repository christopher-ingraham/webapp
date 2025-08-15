using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Production
{

    public class HrmHeatLogic
    {
        private IHrmHeatRepo<ReportDataSource> hrmHeatRepo;


        public HrmHeatLogic(IHrmHeatRepo<ReportDataSource> hrmHeatRepo)
        {
            this.hrmHeatRepo = hrmHeatRepo;
        }

        public ListResultDto<HrmHeatListItemDto> FillAllHeatIdList(ListRequestDto<HrmHeatListFilterDto> listRequest)
        {
            return hrmHeatRepo.FillAllHeatIdList(listRequest);
        }
    }
}