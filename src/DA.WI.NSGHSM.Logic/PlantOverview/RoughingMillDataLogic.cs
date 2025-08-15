using System.Collections.ObjectModel;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.PlantOverview;
using DA.WI.NSGHSM.IRepo.PlantOverview;
using DA.WI.NSGHSM.Logic.Messaging;
using DA.WI.NSGHSM.Repo;
using System.Reactive.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace DA.WI.NSGHSM.Logic.PlantOverview
{

    public class RoughingMillDataLogic
    {
        protected RoughingMillDataDto roughingMillDataActual = new RoughingMillDataDto();
        protected MessagingLogic messagingLogic;
        private IRoughingMillDataRepo<ReportDataSource> roughingMillDataRepo;
        ILogger<RoughingMillDataLogic> logger;

        public RoughingMillDataLogic(IRoughingMillDataRepo<ReportDataSource> roughingMillDataRepo, ILogger<RoughingMillDataLogic> _logger, MessagingLogic messagingLogic)
        {
            this.roughingMillDataRepo = roughingMillDataRepo;
            this.messagingLogic = messagingLogic;
            this.logger = _logger;
        }

        public RoughingMillDataDto SelRoughingMillData()
        {
            roughingMillDataActual = messagingLogic.roughingMillDataActual;
            RoughingMillDataDto resp = roughingMillDataRepo.SelRoughingMillData(roughingMillDataActual.PieceNo);
            // questo per gestire persistenza dati
            if (roughingMillDataActual != null) {
                resp.PieceNo = roughingMillDataActual.PieceNo;
                resp.AfterData.ActualData = roughingMillDataActual.AfterData.ActualData;
                resp.DescalerData.Status = roughingMillDataActual.DescalerData.Status;
                resp.EdgerData.Status = roughingMillDataActual.EdgerData.Status;
                resp.EdgerData.ActualData = roughingMillDataActual.EdgerData.ActualData;
                resp.StandData[0].ActualData = roughingMillDataActual.StandData[0].ActualData;
                resp.StandData[0].Status = roughingMillDataActual.StandData[0].Status;
                resp.StandData[1].Status = roughingMillDataActual.StandData[1].Status;
                resp.IntensiveData[0].BottomStatus = roughingMillDataActual.IntensiveData[0].BottomStatus;
                resp.IntensiveData[0].TopStatus = roughingMillDataActual.IntensiveData[0].TopStatus;
                resp.IntensiveData[1].BottomStatus = roughingMillDataActual.IntensiveData[1].BottomStatus;
                resp.IntensiveData[1].TopStatus = roughingMillDataActual.IntensiveData[1].TopStatus;
                resp.StandData[1].ActualData = roughingMillDataActual.StandData[1].ActualData;
            }
            return resp;
        }
    }
}