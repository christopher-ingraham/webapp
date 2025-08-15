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

    public class CoolingDowncoilersDataLogic
    {
        protected CoolingDowncoilersDataDto coolingDowncoilersDataActual = new CoolingDowncoilersDataDto();
        protected MessagingLogic messagingLogic;
        private ICoolingDowncoilersDataRepo<ReportDataSource> coolingDowncoilersDataRepo;
        ILogger<CoolingDowncoilersDataLogic> logger;

        public CoolingDowncoilersDataLogic(ICoolingDowncoilersDataRepo<ReportDataSource> coolingDowncoilersDataRepo, ILogger<CoolingDowncoilersDataLogic> _logger, MessagingLogic messagingLogic)
        {
            this.coolingDowncoilersDataRepo = coolingDowncoilersDataRepo;
            this.messagingLogic = messagingLogic;
            this.logger = _logger;
        }

        public CoolingDowncoilersDataDto SelCoolingDowncoilersData()

        {   coolingDowncoilersDataActual = messagingLogic.coolingDowncoilersDataDto;
            CoolingDowncoilersDataDto resp = coolingDowncoilersDataRepo.SelCoolingDowncoilersData(coolingDowncoilersDataActual.PieceNo);
            // questo per gestire persistenza dati
            resp.PieceNo = coolingDowncoilersDataActual.PieceNo;
            if (coolingDowncoilersDataActual != null) {
                resp.EntryPyrometerData.ActualData = coolingDowncoilersDataActual.EntryPyrometerData.ActualData;
                resp.IntPyrometerData.ActualData = coolingDowncoilersDataActual.IntPyrometerData.ActualData;
                resp.ExitPyrometerData.ActualData = coolingDowncoilersDataActual.ExitPyrometerData.ActualData;

                for(int i = 0; i<2; i++){
                    resp.DowncoilerData[i].ActualData = coolingDowncoilersDataActual.DowncoilerData[i].ActualData;
                    resp.DowncoilerData[i].Status = coolingDowncoilersDataActual.DowncoilerData[i].Status;
                    resp.DowncoilerData[i].ActiveDC = coolingDowncoilersDataActual.DowncoilerData[i].ActiveDC;
                }

                for(int i = 0; i<3; i++){
                    resp.IntensiveData[i].BottomStatus = coolingDowncoilersDataActual.IntensiveData[i].BottomStatus;
                    resp.IntensiveData[i].TopStatus = coolingDowncoilersDataActual.IntensiveData[i].TopStatus;
                }

                for(int i = 0; i<6; i++){
                    resp.NormalData[i].ActualData = coolingDowncoilersDataActual.NormalData[i].ActualData;
                    resp.NormalData[i].BottomStatus = coolingDowncoilersDataActual.NormalData[i].BottomStatus;
                    resp.NormalData[i].TopStatus = coolingDowncoilersDataActual.NormalData[i].TopStatus;
                }

                for(int i = 0; i<2; i++){
                    resp.TrimmingData[i].ActualData = coolingDowncoilersDataActual.TrimmingData[i].ActualData;
                    resp.TrimmingData[i].BottomStatus = coolingDowncoilersDataActual.TrimmingData[i].BottomStatus;
                    resp.TrimmingData[i].TopStatus = coolingDowncoilersDataActual.TrimmingData[i].TopStatus;
                }

                resp.RollssmallData.Status = coolingDowncoilersDataActual.RollssmallData.Status;
            }
            return resp;
        }
    }
}