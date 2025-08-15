using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class TdbProcessCodeLogic
    {
        private ITdbProcessCodeRepo<ReportDataSource> TdbProcessCodeRepo;


        public TdbProcessCodeLogic(ITdbProcessCodeRepo<ReportDataSource> TdbProcessCodeRepo)
        {
            this.TdbProcessCodeRepo = TdbProcessCodeRepo;
        }

        public ListResultDto<TdbProcessCodeListItemDto> FillProcessCodesByCodeType(ListRequestDto<TdbProcessCodeListFilterDto> listRequest)
        {
            return TdbProcessCodeRepo.FillProcessCodesByCodeType(listRequest);
        }
    }
}