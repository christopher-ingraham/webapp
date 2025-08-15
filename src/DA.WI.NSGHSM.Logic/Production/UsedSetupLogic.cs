using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo;
using System;
using System.Transactions;

namespace DA.WI.NSGHSM.Logic.Production
{

    public class UsedSetupLogic
    {
        private IUsedSetupRepo<ReportDataSource> usedSetupRepo;


        public UsedSetupLogic(IUsedSetupRepo<ReportDataSource> usedSetupRepo)
        {
            this.usedSetupRepo = usedSetupRepo;
        }
        public ListResultDto<UsedSetupListItemDto> SelUsedSetup(ListRequestDto<UsedSetupListFilterDto> listRequest)
        {
            return usedSetupRepo.SelUsedSetup(listRequest);
        }
    }
}