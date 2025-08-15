using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class TdbGradeGroupLogic
    {
        private ITdbGradeGroupRepo<ReportDataSource> TdbGradeGroupRepo;


        public TdbGradeGroupLogic(ITdbGradeGroupRepo<ReportDataSource> TdbGradeGroupRepo)
        {
            this.TdbGradeGroupRepo = TdbGradeGroupRepo;
        }

        public ListResultDto<TdbGradeGroupListItemDto> FillGradeGroup(ListRequestDto<TdbGradeGroupListFilterDto> listRequest)
        {
            return TdbGradeGroupRepo.FillGradeGroup(listRequest);
        }
    }
}