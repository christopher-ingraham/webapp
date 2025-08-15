using System;
using System.Collections.Generic;
using System.Transactions;
using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.Production
{

    public class ProducedCoilsLogic
    {
        private IProducedCoilsRepo<ReportDataSource> producedCoilsRepo;

        public ProducedCoilsLogic(IProducedCoilsRepo<ReportDataSource> producedCoilsRepo)
        {
            this.producedCoilsRepo = producedCoilsRepo;
        }

        public ListResultDto<ProducedCoilsListItemDto> SelProdCoils(ListRequestDto<ProducedCoilsListFilterDto> listRequest)
        {
            return producedCoilsRepo.SelProdCoils(listRequest);
        }

        public void UpdRepHmProdCoil(ProducedCoilsForUpdateDto dto, long outPieceNo)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                producedCoilsRepo.UpdRepHmProdCoil(dto, outPieceNo);

                ts.Complete();
            }
        }

        public ProducedCoilsDetailDto GetCurrentOutputCoil(long outPieceNo)
        {
            return producedCoilsRepo.GetCurrentOutputCoil(outPieceNo);
        }

        public void Delete(long id)
        {


            using (TransactionScope ts = new TransactionScope())
            {
                producedCoilsRepo.Delete(id);

                ts.Complete();
            }
        }

        public ProducedCoilsDetailDto InsRepHmProdCoil(ProducedCoilsForInsertDto dto)
        {
            ProducedCoilsDetailDto result;

            using (TransactionScope ts = new TransactionScope())
            {

                result = Read(producedCoilsRepo.InsRepHmProdCoil(dto));

                ts.Complete();
            }

            return result;
        }

        private void ValidateForInsert(ProducedCoilsForInsertDto dto)
        {
            var badRequestMap = new Dictionary<string, BadRequest>();

            badRequestMap = Validate(dto);

            if (badRequestMap.Count > 0)
            {
                throw new BadRequestException(badRequestMap);
            }
        }

        private void ValidateForUpdate(ProducedCoilsForUpdateDto dto, int outPieceNo)
        {
            Validate(dto);

            if (producedCoilsRepo.Exists(outPieceNo) == false)
            {
                throw new NotFoundException(typeof(ProducedCoilsForUpdateDto), dto.OutPieceNo);
            }
        }

        private void ValidateForDelete(int outPieceNo, ProducedCoilsBaseDto dto)
        {
            Validate(dto);

            if (producedCoilsRepo.Exists(outPieceNo) == false)
            {
                throw new NotFoundException(typeof(ProducedCoilsForUpdateDto), dto.OutPieceNo);
            }
        }

        private Dictionary<string, BadRequest> Validate(ProducedCoilsBaseDto dto)
        {
            var badRequestMap = new Dictionary<string, BadRequest>();

            return badRequestMap;
        }

        public ProducedCoilsDetailDto Read(long outPieceNo)
        {
            return producedCoilsRepo.Read(outPieceNo);
        }

        public ListResultDto<ProducedCoilsLookupDto> Lookup(ListRequestDto<ProducedCoilsLookupDto> listRequest)
        {
            return producedCoilsRepo.Lookup(listRequest);
        }
    }
}