using System.Collections.ObjectModel;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.PlantOverview;
using DA.WI.NSGHSM.IRepo.PlantOverview;
using DA.WI.NSGHSM.Logic.Messaging;
using DA.WI.NSGHSM.Repo;
using Microsoft.Extensions.Logging;

namespace DA.WI.NSGHSM.Logic.PlantOverview
{

    public class FinishingMillDataLogic
    {
        protected FinishingMillDataDto finishingMillDataActual = new FinishingMillDataDto();
        protected MessagingLogic messagingLogic;
        private IFinishingMillDataRepo<ReportDataSource> finishingMillDataRepo;
        ILogger<FinishingMillDataLogic> logger;

        public FinishingMillDataLogic(IFinishingMillDataRepo<ReportDataSource> finishingMillDataRepo, ILogger<FinishingMillDataLogic> _logger, MessagingLogic messagingLogic)
        {
            this.finishingMillDataRepo = finishingMillDataRepo;
            this.messagingLogic = messagingLogic;
            this.logger = _logger;
        }

        public FinishingMillDataDto SelFinishingMillData()
        {
            finishingMillDataActual = messagingLogic.finishingMillDataActual;
            FinishingMillDataDto resp = finishingMillDataRepo.SelFinishingMillData(finishingMillDataActual.PieceNo);
            if (finishingMillDataActual != null)
            {
                resp.PieceNo = finishingMillDataActual.PieceNo;
                resp.ExitData.ActualData = finishingMillDataActual.ExitData.ActualData;
                resp.EntryData.ActualData = finishingMillDataActual.EntryData.ActualData;
                resp.DescalerData.Status = finishingMillDataActual.DescalerData.Status;
                for (int i = 0; i < 6; i++)
                {
                    resp.StandData[i].ActualData = finishingMillDataActual.StandData[i].ActualData;
                    resp.StandData[i].Status = finishingMillDataActual.StandData[i].Status;
                }
            }
            return resp;
        }
    }
}