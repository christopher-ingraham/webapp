using DA.WI.NSGHSM.Dto.QualityControlSystem;

namespace DA.WI.NSGHSM.IRepo.QualityControlSystem
{
    public interface ITdbAlloyRepo<TDataSource>
    {
        TdbAlloyDto GetCurrentTdbAlloy(int AlloyCode);
    }
}
