using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Production
{

    public class TdbMaterialSpecLogic
    {
        private ITdbMaterialSpecRepo<ReportDataSource> tdbMaterialSpecRepo;


        public TdbMaterialSpecLogic(ITdbMaterialSpecRepo<ReportDataSource> tdbMaterialSpecRepo)
        {
            this.tdbMaterialSpecRepo = tdbMaterialSpecRepo;
        }

        public ListResultDto<TdbMaterialSpecLookupDto> SelMaterialSpecList(ListRequestDto<TdbMaterialSpecListFilterDto> listRequest)
        {
            return tdbMaterialSpecRepo.SelMaterialSpecList(listRequest);
        }
    }
}