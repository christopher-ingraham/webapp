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

    public class RmlCrewLogic
    {
        private IRmlCrewRepo<ReportDataSource> rmlCrewRepo;

        public RmlCrewLogic(IRmlCrewRepo<ReportDataSource> rmlCrewRepo)
        {
            this.rmlCrewRepo = rmlCrewRepo;
        }

        public ListResultDto<RmlCrewLookupDto> SelCrewForOutCoil(ListRequestDto<RmlCrewLookupDto> listRequest)
        {
            return rmlCrewRepo.SelCrewForOutCoil(listRequest);
        }
    }
}