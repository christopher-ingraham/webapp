using System.Resources;
using DA.DB.Utils;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.PlantOverview;
using DA.WI.NSGHSM.IRepo.PlantOverview;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.PlantOverview
{
    internal class CoolingDowncoilersDataRepo<TDataSource> : ICoolingDowncoilersDataRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public CoolingDowncoilersDataRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public CoolingDowncoilersDataDto SelCoolingDowncoilersData(int actualPieceNo)
        {
            CoolingDowncoilersDataDto result = new CoolingDowncoilersDataDto();

            result.DowncoilerData = new DowncoilersDataDto[2];
            for(int i = 0; i<2; i++){
                result.DowncoilerData[i] = new DowncoilersDataDto();
                result.DowncoilerData[i].SetupData = selDowncoilerValues(actualPieceNo);
                result.DowncoilerData[i].Status = new StepStatusDto();
            }
           
            result.EntryPyrometerData = new EntryPyrometerDataDto();
            result.EntryPyrometerData.SetupData = new EntryPyrometerSetupValueDto();
            result.EntryPyrometerData.Status = new StepStatusDto();
            result.EntryPyrometerData.SetupData = selEntryPyrometerCoolingValue(actualPieceNo);

            result.IntPyrometerData = new IntPyrometerDataDto();
            result.IntPyrometerData.SetupData = new IntPyrometerSetupValueDto();
            result.IntPyrometerData.Status = new StepStatusDto();
            result.IntPyrometerData.SetupData = selIntPyrometerCoolingValue(actualPieceNo);

            result.ExitPyrometerData = new ExitPyrometerDataDto();
            result.ExitPyrometerData.SetupData = new ExitPyrometerSetupValueDto();
            result.ExitPyrometerData.Status = new StepStatusDto();

            result.ExitPyrometerData.SetupData = selExitPyrometerCoolingValue(actualPieceNo);
            result.GeneralData = SelGeneralData();


            result.IntensiveData = new IntensiveDataDto[3];
            for(int i=0; i<3; i++){
                result.IntensiveData[i] = new IntensiveDataDto();
                result.IntensiveData[i].BottomStatus = new StepStatusDto();
                result.IntensiveData[i].TopStatus = new StepStatusDto();
            }

            result.NormalData = new NormalDataDto[6];
            for(int i=0; i<6; i++){
                result.NormalData[i] = new NormalDataDto();
                result.NormalData[i].ActualData = new NormalDataValueDto();
                result.NormalData[i].BottomStatus = new StepStatusDto();
                result.NormalData[i].TopStatus = new StepStatusDto();
            }

            result.TrimmingData = new TrimmingDataDto[2];
            for(int i=0; i<2; i++){
                result.TrimmingData[i] = new TrimmingDataDto();
                result.TrimmingData[i].ActualData = new TrimmingDataValueDto();
                result.TrimmingData[i].BottomStatus = new StepStatusDto();
                result.TrimmingData[i].TopStatus = new StepStatusDto();
            }

            result.RollssmallData = new RollssmallData();
            result.RollssmallData.Status = new StepStatusDto();

            return result;
        }

        private DowncoilersDataSetupValueDto selDowncoilerValues(int actualPieceNo){

            string query = $@"SELECT STEP_NO                    AS StepNo
                                ,HDR.TRG_INN_DIAM               AS TrgInnerDiameter
                                ,EXIT_TENSION                   AS ExitTension
                                ,MILL_SPEED*(1+FSLIP)           AS MillSpeed
                                FROM REP_HM_SETUP_STEP STP
                                JOIN REP_HM_SETUP HDR ON STP.AREA_ID = HDR.AREA_ID 
                                AND STP.CENTER_ID = HDR.CENTER_ID AND STP.IN_PIECE_NO = HDR.IN_PIECE_NO
                                WHERE STP.AREA_ID = 'HSM' AND STP.CENTER_ID = 'FM' 
                                AND STP.IN_PIECE_NO = :ActualPieceNo
                                AND STEP_NO = 6
                                ORDER BY STEP_NO ASC ";

            var data = ctx.GetEntity<DowncoilersDataSetupValueDto>(query, ctx.CreateParameter("ActualPieceNo", actualPieceNo));


            if (data != null){
                return data;
            } else {
                return new DowncoilersDataSetupValueDto();
            }
        }

        private EntryPyrometerSetupValueDto selEntryPyrometerCoolingValue(int actualPieceNo){

            string query = $@"SELECT F_TARGET_TEMP  AS  TargetTemp  
                                FROM REP_ROT_SETUP WHERE AREA_ID = 'HSM' 
                                AND CENTER_ID = 'DC' 
                                AND IN_PIECE_NO = :ActualPieceNo ";

            var data = ctx.GetEntity<EntryPyrometerSetupValueDto>(query, ctx.CreateParameter("ActualPieceNo", actualPieceNo));
            
            if (data != null){
                return data;
            } else {
                return new EntryPyrometerSetupValueDto();
            }
        }

        private IntPyrometerSetupValueDto selIntPyrometerCoolingValue(int actualPieceNo){

            string query = $@"SELECT TARGET_TEMP_INTERM  AS  TargetTemp  
                                FROM REP_ROT_SETUP WHERE AREA_ID = 'HSM' 
                                AND CENTER_ID = 'DC' 
                                AND IN_PIECE_NO = :ActualPieceNo ";

            var data = ctx.GetEntity<IntPyrometerSetupValueDto>(query, ctx.CreateParameter("ActualPieceNo", actualPieceNo));

            if (data != null){
                return data;
            } else {
                return new IntPyrometerSetupValueDto();
            }
        }

        private ExitPyrometerSetupValueDto selExitPyrometerCoolingValue(int actualPieceNo){

            string query = $@"SELECT DC_TARGET_TEMP  AS  TargetTemp  
                                FROM REP_ROT_SETUP WHERE AREA_ID = 'HSM' 
                                AND CENTER_ID = 'DC' 
                                AND IN_PIECE_NO = :ActualPieceNo ";

            var data = ctx.GetEntity<ExitPyrometerSetupValueDto>(query, ctx.CreateParameter("ActualPieceNo", actualPieceNo));

            if (data != null){
                return data;
            } else {
                return new ExitPyrometerSetupValueDto();
            }
        }

        private CoolingDowncoilerGeneralData[] SelGeneralData()
        {

             string query = $@"SELECT POSITION_NO      AS PositionNo
                                ,TRIM(POSITION_LABEL)  AS PositionLabel
                                ,TRIM(PIECE_ID)        AS PieceId
                                ,PIECE_NO              AS PieceNo
                                ,DECODE(POSITION_NO, 35, NULL, 55, NULL, 57, NULL, 60, NULL, 70, NULL, ELAPSED_TIME) AS FurnaceTime
                                FROM RTDB_TRACK_INFO
                                WHERE POSITION_NO IN (15, 30, 35, 50, 55, 57)
                                OR (POSITION_NO BETWEEN 1 AND 57
                                AND TRACK_STATUS = 1)
                                ORDER BY POSITION_NO DESC ";


            var data = ctx.GetEntities<CoolingDowncoilerGeneralData>(query).ToArray();

            return data;
        }
    }
}