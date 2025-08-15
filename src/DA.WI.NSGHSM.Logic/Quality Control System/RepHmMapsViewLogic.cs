using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;


namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class RepHmMapsViewLogic
    {
        private IRepHmMapsViewRepo<ReportDataSource> repHmMapsViewRepo;


        public RepHmMapsViewLogic(IRepHmMapsViewRepo<ReportDataSource> repHmMapsViewRepo)
        {
            this.repHmMapsViewRepo = repHmMapsViewRepo;
        }

           public ListResultDto<RepHmMapsViewListItemDto> ReadList(ListRequestDto<RepHmMapsViewListFilterDto> listRequest)
        {
            return repHmMapsViewRepo.ReadList(listRequest);
        }
    }
}