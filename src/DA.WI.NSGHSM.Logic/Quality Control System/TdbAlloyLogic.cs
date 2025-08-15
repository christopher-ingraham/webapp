using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;


namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class TdbAlloyLogic
    {
        private ITdbAlloyRepo<ReportDataSource> TdbAlloyRepo;


        public TdbAlloyLogic(ITdbAlloyRepo<ReportDataSource> TdbAlloyRepo)
        {
            this.TdbAlloyRepo = TdbAlloyRepo;
        }

        public TdbAlloyDto GetCurrentTdbAlloy(int AlloyCode)
        {
            return TdbAlloyRepo.GetCurrentTdbAlloy(AlloyCode);
        }
    }
}