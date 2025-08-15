using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class TdbAlloySpecLogic
    {
        private ITdbAlloySpecRepo<ReportDataSource> TdbAlloySpecRepo;


        public TdbAlloySpecLogic(ITdbAlloySpecRepo<ReportDataSource> TdbAlloySpecRepo)
        {
            this.TdbAlloySpecRepo = TdbAlloySpecRepo;
        }

        public ListResultDto<TdbAlloySpecListItemDto> SelTdbAlloySpec(ListRequestDto<TdbAlloySpecListFilterDto> listRequest)
        {
            return TdbAlloySpecRepo.SelTdbAlloySpec(listRequest);
        }

        public TdbAlloySpecDetailDto GetTdbAlloySpec(int AlloySpecCnt)
        {
            return TdbAlloySpecRepo.GetTdbAlloySpec(AlloySpecCnt);
        }
    }
}