using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;

namespace DA.WI.NSGHSM.IRepo.Production
{    public interface IHrmInputPieceRepo<TDataSource>
    {
        HrmInputPieceForInsertDto Read(int pieceNo);
        HrmInputPieceDetailDto GetCurrentInputPieceMng(int PieceNo);
        ListResultDto<HrmInputPieceListItemDto> SelInputPiecesList(ListRequestDto<HrmInputPieceListFilterDto> listRequest);
        void Create(HrmInputPieceForInsertDto dto);
        void Update(HrmInputPieceForUpdateDto dto, int pieceNo);
        void Delete(int pieceNo);
        bool Exists(int pieceNo);
        ListResultDto<HrmInputPieceLookupDto> Lookup(ListRequestDto<HrmInputPieceLookupDto> listRequest);
    }
}