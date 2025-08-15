using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;


namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class RepHmTrendsViewLogic
    {
        private IRepHmTrendsViewRepo<ReportDataSource> repHmTrendsViewRepo;


        public RepHmTrendsViewLogic(IRepHmTrendsViewRepo<ReportDataSource> repHmTrendsViewRepo)
        {
            this.repHmTrendsViewRepo = repHmTrendsViewRepo;
        }

           public ListResultDto<RepHmTrendsViewListItemDto> ReadList(ListRequestDto<RepHmTrendsViewListFilterDto> listRequest)
        {
            return repHmTrendsViewRepo.ReadList(listRequest);
        }
    }

}