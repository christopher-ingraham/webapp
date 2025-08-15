using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;


namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class RepHmPieceStatLogic
    {
        private IRepHmPieceStatRepo<ReportDataSource> repHmPieceStatRepo;


        public RepHmPieceStatLogic(IRepHmPieceStatRepo<ReportDataSource> repHmPieceStatRepo)
        {
            this.repHmPieceStatRepo = repHmPieceStatRepo;
        }

           public ListResultDto<RepHmPieceStatListItemDto> ReadList(ListRequestDto<RepHmPieceStatListFilterDto> listRequest)
        {
            return repHmPieceStatRepo.ReadList(listRequest);
        }
    }

}
