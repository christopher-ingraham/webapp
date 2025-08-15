using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.PlantOverview;

namespace DA.WI.NSGHSM.IRepo.PlantOverview
{
    public interface IFinishingMillDataRepo<TDataSource>
    {
        FinishingMillDataDto SelFinishingMillData(int pieceNo);
    }
}