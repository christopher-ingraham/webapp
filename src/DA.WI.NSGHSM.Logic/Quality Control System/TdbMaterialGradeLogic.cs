using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.QualityControlSystem
{

    public class TdbMaterialGradeLogic
    {
        private ITdbMaterialGradeRepo<ReportDataSource> tdbMaterialGradeRepo;


        public TdbMaterialGradeLogic(ITdbMaterialGradeRepo<ReportDataSource> tdbMaterialGradeRepo)
        {
            this.tdbMaterialGradeRepo = tdbMaterialGradeRepo;
        }

        public ListResultDto<TdbMaterialGradeListItemDto> SelMatGradeForInPiece(ListRequestDto<TdbMaterialGradeListFilterDto> listRequest)
        {
            return tdbMaterialGradeRepo.SelMatGradeForInPiece(listRequest);
        }

        public ListResultDto<TdbMaterialGradeLookupDto> FillMaterialGrade(ListRequestDto<TdbMaterialGradeLookupDto> listRequest)
        {
            return tdbMaterialGradeRepo.FillMaterialGrade(listRequest);
        }
    }
}