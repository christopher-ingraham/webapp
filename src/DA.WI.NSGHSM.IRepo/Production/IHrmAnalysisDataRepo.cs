using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;

namespace DA.WI.NSGHSM.IRepo.Production
{
    public interface IHrmAnalysisDataRepo<TDataSource>
    {
        ListResultDto<HrmAnalysisDataListItemDto> LoadHrmSampleId(ListRequestDto<HrmAnalysisDataListFilterDto> listRequest);
        ListResultDto<HrmAnalysisDataDetailDto> LoadHrmSampleIdList(ListRequestDto<HrmAnalysisDataDetailDto> listRequest, int analysisCnt);

        HrmAnalysisDataDetailDto Read(int analysisCnt);
    }
}
