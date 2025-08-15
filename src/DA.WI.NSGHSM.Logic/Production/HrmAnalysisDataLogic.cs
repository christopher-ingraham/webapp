using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Production
{

    public class HrmAnalysisDataLogic
    {
        private IHrmAnalysisDataRepo<ReportDataSource> hrmAnalysisDataRepo;


        public HrmAnalysisDataLogic(IHrmAnalysisDataRepo<ReportDataSource> hrmAnalysisDataRepo)
        {
            this.hrmAnalysisDataRepo = hrmAnalysisDataRepo;
        }

        public ListResultDto<HrmAnalysisDataListItemDto> LoadHrmSampleId(ListRequestDto<HrmAnalysisDataListFilterDto> listRequest)
        {
            return hrmAnalysisDataRepo.LoadHrmSampleId(listRequest);
        }

        public ListResultDto<HrmAnalysisDataDetailDto> LoadHrmSampleIdList(ListRequestDto<HrmAnalysisDataDetailDto> listRequest, int analysisCnt)
        {
            return hrmAnalysisDataRepo.LoadHrmSampleIdList(listRequest, analysisCnt);
        }

        public HrmAnalysisDataDetailDto Read(int id)
        {
            return hrmAnalysisDataRepo.Read(id);
        }
    }
}
