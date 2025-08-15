using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;

namespace DA.WI.NSGHSM.IRepo.Production
{
    public interface IHrmJobRepo<TDataSource>
    {
        HrmJobDetailDto Read(string JobId);
        HrmJobDetailDto SelJobById(string JobId);
        ListResultDto<HrmJobListItemDto> SelJobsList(ListRequestDto<HrmJobListFilterDto> listRequest);
        string Create(HrmJobForInsertDto dto);

        void Update(HrmJobForUpdateDto dto,string id);

        void Delete(string id);

        bool Exists(string JobId);

        ListResultDto<HrmJobLookupDto> Lookup(ListRequestDto<HrmJobLookupDto> listRequest);
    }
}
