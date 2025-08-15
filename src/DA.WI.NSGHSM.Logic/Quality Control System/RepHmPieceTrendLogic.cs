using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;


namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class RepHmPieceTrendLogic
    {
        private IRepHmPieceTrendRepo<ReportDataSource> repHmPieceTrendRepo;


        public RepHmPieceTrendLogic(IRepHmPieceTrendRepo<ReportDataSource> repHmPieceTrendRepo)
        {
            this.repHmPieceTrendRepo = repHmPieceTrendRepo;
        }

           public ListResultDto<RepHmPieceTrendListItemDto> ReadList(ListRequestDto<RepHmPieceTrendListFilterDto> listRequest)
        {
            return repHmPieceTrendRepo.ReadList(listRequest);
        }
    }

}