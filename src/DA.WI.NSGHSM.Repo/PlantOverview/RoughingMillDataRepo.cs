using System.Linq;
using System.Resources;
using DA.DB.Utils;
using DA.WI.NSGHSM.Dto.PlantOverview;
using DA.WI.NSGHSM.IRepo.PlantOverview;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using DA.WI.NSGHSM.IRepo.AuxValue;
using DA.WI.NSGHSM.Dto.AuxValue;

namespace DA.WI.NSGHSM.Repo.PlantOverview
{
    internal class RoughingMillDataRepo<TDataSource> : IRoughingMillDataRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public RoughingMillDataRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }


        public RoughingMillDataDto SelRoughingMillData(int pieceNo)
        {

            RoughingMillDataDto result = new RoughingMillDataDto();

            result.AfterData = new RoughingMillAfterDataDto();
            result.AfterData.SetupData = new RoughingMillAfterSetupValueDto();

            result.AfterData.SetupData = GetAfterValues(pieceNo, 2);
            result.StandData = SelStandData(pieceNo);
            result.DescalerData = SelDescaler();
            result.EdgerData = SelEdger(pieceNo);
            result.IntensiveData = SelIntensive();
            result.GeneralData = SelGeneralData();

            return result;
        }

        public RoughingMillDescalerDataDto SelDescaler()
        {

            RoughingMillDescalerDataDto descaler = new RoughingMillDescalerDataDto();
            descaler.Status = new StepStatusDto();

            return descaler;
        }

        public RoughingMillEdgerDataDto SelEdger(int actualPieceNo)
        {
            RoughingMillEdgerDataDto result = new RoughingMillEdgerDataDto();

            var setupData = GetEdgerValues(actualPieceNo, 1);

            result.SetupData = setupData;
            result.Status = new StepStatusDto();

            if (result.SetupData == null)
            {
                result.SetupData = new RoughingMillEdgerSetupValueDto();
            }

            return result;
        }

        public RoughingMillGeneralData[] SelGeneralData()
        {

            string query = $@"SELECT POSITION_NO      AS PositionNo
                                ,TRIM(POSITION_LABEL)  AS PositionLabel
                                ,TRIM(PIECE_ID)        AS PieceId
                                ,PIECE_NO              AS PieceNo
                                ,DECODE(POSITION_NO, 35, NULL, 55, NULL, 57, NULL, 60, NULL, 70, NULL, ELAPSED_TIME) AS FurnaceTime
                                FROM RTDB_TRACK_INFO
                                WHERE POSITION_NO IN (15, 30, 35)
                                OR (POSITION_NO BETWEEN 1 AND 35
                                AND TRACK_STATUS = 1)
                                ORDER BY POSITION_NO DESC ";


            var data = ctx.GetEntities<RoughingMillGeneralData>(query).ToArray();

            return data;
        }


        public RoughingMillIntensiveDataDto[] SelIntensive()
        {

            RoughingMillIntensiveDataDto intensive1 = new RoughingMillIntensiveDataDto();
            RoughingMillIntensiveDataDto intensive2 = new RoughingMillIntensiveDataDto();

            intensive1.BottomStatus = new StepStatusDto();
            intensive1.TopStatus = new StepStatusDto();
            intensive2.BottomStatus = new StepStatusDto();
            intensive2.TopStatus = new StepStatusDto();

            RoughingMillIntensiveDataDto[] result = new RoughingMillIntensiveDataDto[2];
            result[0] = intensive1;
            result[1] = intensive2;

            return result;
        }


        public RoughingMillStandDataDto[] SelStandData(int actualPieceNo)
        {
            //-------------------------------------------------------- STAND 1 --------------------
            var setupData1 = GetStandValues(actualPieceNo, 1);

            RoughingMillStandDataDto data1 = new RoughingMillStandDataDto();
            data1.SetupData = setupData1;
            data1.Status = new StepStatusDto();
            data1.Status.Rwa = false;
            ValidationRangeDTO range1 = getRangeValidation("ROLL_WEAR_THRESHOLD","R1");
            int upperWorkRoll1 = getWearValue(1, 10, 10);
            int lowerWorkRoll1 = getWearValue(1, 10, -10);
            int upperBackUpRoll1 = getWearValue(1, 30, 10);
            int lowerBackUpRoll1 = getWearValue(1, 30, -10);

            if(upperBackUpRoll1 > range1.max || upperWorkRoll1 > range1.max || lowerBackUpRoll1 > range1.max || lowerWorkRoll1 > range1.max){
                data1.Status.Rwa = true;
            }

            //-------------------------------------------------------- STAND 2 --------------------

            var setupData2 = GetStandValues(actualPieceNo, 2);

            RoughingMillStandDataDto data2 = new RoughingMillStandDataDto();
            data2.Status = new StepStatusDto();
            data2.SetupData = setupData2;
            data2.Status.Rwa = false;
            ValidationRangeDTO range2 = getRangeValidation("ROLL_WEAR_THRESHOLD","R2");
            int upperWorkRoll2 = getWearValue(2, 10, 10);
            int lowerWorkRoll2 = getWearValue(2, 10, -10);
            int upperBackUpRoll2 = getWearValue(2, 30, 10);
            int lowerBackUpRoll2 = getWearValue(2, 30, -10);

            if(upperWorkRoll2 > range2.max || lowerWorkRoll2 > range2.max || upperBackUpRoll2 > range2.max || lowerBackUpRoll2 > range2.max){
                data2.Status.Rwa = true;
            }

            //-------------------------------------------------------------------------------------

            RoughingMillStandDataDto[] result = new RoughingMillStandDataDto[2];

            result[0] = data1;
            result[1] = data2;
            return result;
        }

        private RoughingMillValueDto GetStandValues(int actualPieceNo, int stepNo)
        {


            string query = $@"SELECT STEP_NO    AS StepNo 
                            ,EXIT_THK           AS ExitThickness
                            ,FORCE              AS RollingForce
                            ,MILL_SPEED         AS StandSpeed
                            ,REDUCTION          AS Reduction
                            ,GAP                AS Gap
                            ,WR_BEND            AS WRBending
                            FROM REP_HM_SETUP_STEP 
                            WHERE AREA_ID = 'HSM' 
                            AND CENTER_ID = 'RM' 
                            AND STEP_NO = :StepNo
                            AND IN_PIECE_NO = :ActualPieceNo ";


            RoughingMillValueDto result = ctx.GetEntity<RoughingMillValueDto>(query, ctx.CreateParameter("StepNo", stepNo),
                                                                                     ctx.CreateParameter("ActualPieceNo", actualPieceNo));

            if (result != null) { return result; }
            else { return new RoughingMillValueDto(); }

        }


        private RoughingMillEdgerSetupValueDto GetEdgerValues(int actualPieceNo, int stepNo)
        {

            string query = $@"SELECT STP.STEP_NO       AS StepNo 
                            ,HDR.ENTRY_WDT             As EntryWidth
                            ,HDR.ENTRY_STRIP_SPEED     AS EntrySpeed
                            ,STP.ENTRY_TEMP            AS EntryTemp
                            ,STP.EDGER_TRG_WIDTH       AS EdgerWidth
                            ,STP.EDGER_FORCE           AS EdgerForce
                            FROM REP_HM_SETUP_STEP STP
                            JOIN REP_HM_SETUP HDR
                            ON HDR.AREA_ID = STP.AREA_ID 
                            AND HDR.CENTER_ID = STP.CENTER_ID
                            AND HDR.IN_PIECE_NO = STP.IN_PIECE_NO
                            WHERE STP.AREA_ID = 'HSM' 
                            AND STP.CENTER_ID = 'RM' 
                            AND STP.STEP_NO = :StepNo
                            AND STP.IN_PIECE_NO = :ActualPieceNo ";

            RoughingMillEdgerSetupValueDto result = ctx.GetEntity<RoughingMillEdgerSetupValueDto>(query, ctx.CreateParameter("StepNo", stepNo),
                                                                                               ctx.CreateParameter("ActualPieceNo", actualPieceNo));

            if (result != null) { return result; }
            else { return new RoughingMillEdgerSetupValueDto(); }
        }


        private RoughingMillAfterSetupValueDto GetAfterValues(int actualPieceNo, int stepNo)
        {

            string query = $@"SELECT STEP_NO            AS StepNo 
                            ,EXIT_WIDTH                 AS ExitWidth
                            ,MILL_SPEED * (1 + FSLIP)   AS ExitSpeed
                            ,EXIT_TEMP                  AS ExitTemp
                            FROM REP_HM_SETUP_STEP 
                            WHERE AREA_ID = 'HSM' 
                            AND CENTER_ID = 'RM' 
                            AND STEP_NO = :StepNo
                            AND IN_PIECE_NO = :ActualPieceNo ";

            RoughingMillAfterSetupValueDto result = ctx.GetEntity<RoughingMillAfterSetupValueDto>(query, ctx.CreateParameter("StepNo", stepNo),
                                                                                        ctx.CreateParameter("ActualPieceNo", actualPieceNo));

            if (result != null) { return result; }
            else { return new RoughingMillAfterSetupValueDto(); }

        }
        private ValidationRangeDTO getRangeValidation(string variableId, string value)
        {
            variableId = variableId.PadRight(32, ' ');
            value = value.PadRight(80, ' ');
            string query = $@"SELECT
                          TRIM(VALUE_NAME)    AS valueName
                        , MIN_VALUE           AS min
                        , MAX_VALUE           AS max
                  FROM    AUX_VALUE
                  WHERE   VARIABLE_ID = :VariableId
                  AND     VALUE_NAME = :Value ";

            var result = ctx.GetEntity<ValidationRangeDTO>(query, ctx.CreateParameter("VariableId", variableId),
                                                                  ctx.CreateParameter("Value", value));

            return result;
        }

        private int getWearValue(int standNo, int rollType, int position)
        {
            string query = $@"SELECT WEAR
                                FROM ROL_ROLL_ACTUAL
                                WHERE AREA_ID = 'HSM'
                                AND CENTER_ID = 'RM'
                                AND STAND_NO = :StandNo
                                AND ROLL_TYPE = :RollType
                                AND POSITION = :Position ";

            var result = ctx.GetEntity<int>(query, ctx.CreateParameter("StandNo", standNo),
                                                    ctx.CreateParameter("RollType", rollType),
                                                    ctx.CreateParameter("Position", position));

            return result;
        }




    }
}