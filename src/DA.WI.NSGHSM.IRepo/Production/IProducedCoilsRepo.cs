using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;

namespace DA.WI.NSGHSM.IRepo.Production
{
    public interface IProducedCoilsRepo<TDataSource>
    {
        ProducedCoilsDetailDto GetCurrentOutputCoil (long outPieceNo);
        ListResultDto<ProducedCoilsListItemDto> SelProdCoils(ListRequestDto<ProducedCoilsListFilterDto> listRequest);
        void UpdRepHmProdCoil(ProducedCoilsForUpdateDto dto, long outPieceNo);
        long InsRepHmProdCoil(ProducedCoilsForInsertDto dto);
        ProducedCoilsDetailDto Read(long outPieceNo);
        void Delete(long outPieceNo);
        bool Exists(long outPieceNo);
        ListResultDto<ProducedCoilsLookupDto> Lookup(ListRequestDto<ProducedCoilsLookupDto> listRequest);
    }
}