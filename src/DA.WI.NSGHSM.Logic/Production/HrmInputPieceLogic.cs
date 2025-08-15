using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace DA.WI.NSGHSM.Logic.Production
{

    public class HrmInputPieceLogic
    {
        private IHrmInputPieceRepo<ReportDataSource> hrmInputPieceRepo;


        public HrmInputPieceLogic(IHrmInputPieceRepo<ReportDataSource> hrmInputPieceRepo)
        {
            this.hrmInputPieceRepo = hrmInputPieceRepo;
        }


        public void Create(HrmInputPieceForInsertDto dto)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                hrmInputPieceRepo.Create(dto);

                ts.Complete();
            }
        }


        private void ValidateForUpdate(HrmInputPieceForUpdateDto dto)
        {
            var badRequestMap = new Dictionary<string, BadRequest>();
            
            badRequestMap = Validate(dto);

            if (badRequestMap.Count > 0) {
                throw new BadRequestException(badRequestMap);
            }
        }


        private Dictionary<string, BadRequest> Validate(HrmInputPieceBaseDto dto)
        {
            var badRequestMap = new Dictionary<string, BadRequest>();

            if (hrmInputPieceRepo.Exists(dto.PieceNo) == false)
            {
                badRequestMap.Add(nameof(dto.PieceNo), new BadRequest(BadRequestType.NOT_EXISTS));
            }
            return badRequestMap;
        }


        public void Update(HrmInputPieceForUpdateDto dto, int pieceNo)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                hrmInputPieceRepo.Update(dto, pieceNo);

                ts.Complete();
            }
        }

        public void Delete(int pieceNo)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                hrmInputPieceRepo.Delete(pieceNo);

                ts.Complete();
            }
        }

        public HrmInputPieceForInsertDto Read(int pieceNo)
        {
            return hrmInputPieceRepo.Read(pieceNo);
        }

        public ListResultDto<HrmInputPieceListItemDto> SelInputPiecesList(ListRequestDto<HrmInputPieceListFilterDto> listRequest)
        {
            return hrmInputPieceRepo.SelInputPiecesList(listRequest);
        }

        public HrmInputPieceDetailDto GetCurrentInputPieceMng(int pieceNo)
        {
            return hrmInputPieceRepo.GetCurrentInputPieceMng(pieceNo);
        }

        public ListResultDto<HrmInputPieceLookupDto> Lookup(ListRequestDto<HrmInputPieceLookupDto> listRequest)
        {
            return hrmInputPieceRepo.Lookup(listRequest);
        }
    }
}