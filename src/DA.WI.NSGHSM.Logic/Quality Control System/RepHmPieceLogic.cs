using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class RepHmPieceLogic
    {
        private IRepHmPieceRepo<ReportDataSource> repHmPieceRepo;


        public RepHmPieceLogic(IRepHmPieceRepo<ReportDataSource> repHmPieceRepo)
        {
            this.repHmPieceRepo = repHmPieceRepo;
        }

        public ListResultDto<RepHmPieceListItemDto> ReadList(ListRequestDto<RepHmPieceListFilterDto> listRequest)
        {
            return repHmPieceRepo.ReadList(listRequest);
        }
    }
}