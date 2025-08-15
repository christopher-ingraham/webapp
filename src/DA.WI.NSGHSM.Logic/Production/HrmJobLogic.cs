using System.Diagnostics;
using System.Data;
using System.Linq;
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

    public class HrmJobLogic
    {
        private IHrmJobRepo<ReportDataSource> hrmJobRepo;


        public HrmJobLogic(IHrmJobRepo<ReportDataSource> hrmJobRepo)
        {
            this.hrmJobRepo = hrmJobRepo;
        }

        public ListResultDto<HrmJobListItemDto> SelJobsList(ListRequestDto<HrmJobListFilterDto> listRequest)
        {
            return hrmJobRepo.SelJobsList(listRequest);
        }

        public HrmJobDetailDto Create(HrmJobForInsertDto dto)
        {
            HrmJobDetailDto result;

            using (TransactionScope ts = new TransactionScope())
            {
                ValidateForInsert(dto);

                result = Read(hrmJobRepo.Create(dto));

                ts.Complete();
            }

            return result;
        }
// invece di fare una lista fare una mappa con chiave JOB_ID e come valore la struttura BadRequest
        private void ValidateForInsert(HrmJobForInsertDto dto)
        {
            var badRequestMap = new Dictionary<string, BadRequest>();

            badRequestMap = Validate(dto);

            if (badRequestMap.Count > 0) {
                throw new BadRequestException(badRequestMap);
            }
        }

        private void ValidateForUpdate(HrmJobForUpdateDto dto, string id)
        {
            var badRequestMap = new Dictionary<string, BadRequest>();

            badRequestMap = Validate(dto);

            if (badRequestMap.Count > 0) {
                throw new BadRequestException(badRequestMap);
            }
        }

        private Dictionary<string, BadRequest> Validate(HrmJobBaseDto dto)
        {
            var badRequestMap = new Dictionary<string, BadRequest>();
            if (String.IsNullOrEmpty(dto.JobId))
            {
                 badRequestMap.Add(nameof(dto.JobId), new BadRequest(BadRequestType.REQUIRED_FIELD));
            }

            if (hrmJobRepo.Exists(dto.JobId) == true)
            {
                badRequestMap.Add(nameof(dto.JobId), new BadRequest(BadRequestType.ALREADY_EXISTS));
            }
            return badRequestMap;
        }


        public HrmJobDetailDto Update(HrmJobForUpdateDto dto, string id)
        {
            HrmJobDetailDto result;

            using (TransactionScope ts = new TransactionScope())
            {
                hrmJobRepo.Update(dto, id);
                result = Read(id);

                ts.Complete();
            }

            return result;
        }

        public void Delete(string id)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                hrmJobRepo.Delete(id);

                ts.Complete();
            }
        }

        public HrmJobDetailDto Read(string id)
        {
            return hrmJobRepo.Read(id);
        }

        public HrmJobDetailDto SelJobById(string id)
        {
            return hrmJobRepo.SelJobById(id);
        }

        public ListResultDto<HrmJobLookupDto> Lookup(ListRequestDto<HrmJobLookupDto> listRequest)
        {
            return hrmJobRepo.Lookup(listRequest);
        }

    }
}
