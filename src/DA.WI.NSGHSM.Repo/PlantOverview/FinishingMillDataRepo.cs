using DA.DB.Utils;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.AuxValue;
using DA.WI.NSGHSM.Dto.PlantOverview;
using DA.WI.NSGHSM.IRepo.PlantOverview;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.PlantOverview
{
    internal class FinishingMillDataRepo<TDataSource> : IFinishingMillDataRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public FinishingMillDataRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public FinishingMillDataDto SelFinishingMillData(int pieceNo)
        {
            var standDataDto = SelStandData(pieceNo);
            var entryData = SelEntryData(pieceNo, 1);
            var exitData = SelExitData(pieceNo, 6);
            var generalData = SelGeneralData();

            FinishingMillDescalerDataDto descalerData = new FinishingMillDescalerDataDto();
            descalerData.Status = new StepStatusDto();


            return new FinishingMillDataDto
            {
                StandData = standDataDto,
                EntryData = entryData,
                ExitData = exitData,
                GeneralData = generalData,
                DescalerData = descalerData
            };
        }

        public FinishingMillEntryData SelEntryData(int actualPieceNo, int stepNo)
        {
            FinishingMillEntryData entryData = new FinishingMillEntryData();
            entryData.SetupData = GetEntryValues(actualPieceNo, stepNo);

            return entryData;
        }

        private FinishingMillGeneralData[] SelGeneralData()
        {

            string query = $@"SELECT POSITION_NO      AS PositionNo
                                ,TRIM(POSITION_LABEL)  AS PositionLabel
                                ,TRIM(PIECE_ID)        AS PieceId
                                ,PIECE_NO              AS PieceNo
                                ,DECODE(POSITION_NO, 35, NULL, 55, NULL, 57, NULL, 60, NULL, 70, NULL, ELAPSED_TIME) AS FurnaceTime
                                FROM RTDB_TRACK_INFO
                                WHERE POSITION_NO IN (15, 30, 35, 50, 55)
                                OR (POSITION_NO BETWEEN 1 AND 55
                                AND TRACK_STATUS = 1)
                                ORDER BY POSITION_NO DESC ";


            var data = ctx.GetEntities<FinishingMillGeneralData>(query).ToArray();

            return data;
        }

        public FinishingMillExitData SelExitData(int actualPieceNo, int stepNo)
        {
            FinishingMillExitData exitData = new FinishingMillExitData();
            exitData.SetupData = GetExitValues(actualPieceNo, stepNo);

            return exitData;
        }


        public FinishingMillStandDataDto[] SelStandData(int actualPieceNo)
        {
            var setupValues = GetStandValues(actualPieceNo);

            FinishingMillStandDataDto[] standsData = new FinishingMillStandDataDto[6];

            for (int i = 0; i < 6; i++)
            {
                standsData[i] = new FinishingMillStandDataDto();
                standsData[i].ActualData = new FinishingMillStandValueDto();
                standsData[i].SetupData = setupValues[i];
                standsData[i].Status = new StepStatusDto();
                ValidationRangeDTO range = getRangeValidation("ROLL_WEAR_THRESHOLD", "F" + (i + 1));
                int upperWorkRoll = getWearValue(i + 1, 10, 10);
                int lowerWorkRoll = getWearValue(i + 1, 10, -10);
                int upperBackUpRoll = getWearValue(i + 1, 30, 10);
                int lowerBackUpRoll = getWearValue(i + 1, 30, -10);
                if (upperWorkRoll > range.max || lowerWorkRoll > range.max || upperBackUpRoll > range.max || lowerBackUpRoll > range.max)
                {
                    standsData[i].Status.Rwa = true;
                }
            }

            return standsData;
        }

        private FinishingMillStandValueDto[] GetStandValues(int actualPieceNo)
        {
            FinishingMillStandValueDto[] result = new FinishingMillStandValueDto[6];

            string query = $@"SELECT STEP_NO    AS StepNo 
                            ,EXIT_THK           AS ExitThickness
                            ,FORCE              AS RollingForce
                            ,MILL_SPEED         AS StandSpeed
                            ,REDUCTION          AS Reduction
                            ,GAP                AS Gap
                            ,WR_BEND            AS WRBending
                            ,WR_SHIFT           AS WrShifting
                            ,LOOPER_ANGLE       AS LooperAngle
                            ,EXIT_TENSION       AS LooperSpecificTension
                            ,STRIP_COOLING_FLOW AS InterstandCoolingFlow
                            FROM REP_HM_SETUP_STEP 
                            WHERE AREA_ID = 'HSM' 
                            AND CENTER_ID = 'FM' 
                            AND IN_PIECE_NO = :ActualPieceNo ";

            query += $"ORDER BY STEP_NO ASC ";

            var data = ctx.GetEntities<FinishingMillStandValueDto>(query, ctx.CreateParameter("ActualPieceNo", actualPieceNo)).ToArray();

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new FinishingMillStandValueDto();
                if (i < data.Length)
                {
                    result[i] = data[i];
                }
            }

            return result;
        }



        private FinishingMillEntrySetupValueDto GetEntryValues(int actualPieceNo, int stepNo)
        {

            string query = $@"SELECT STP.STEP_NO        AS StepNo 
                            ,HDR.ENTRY_WDT              As EntryWidth 
                            ,HDR.ENTRY_STRIP_SPEED      AS EntrySpeed  
                            ,STP.ENTRY_TEMP             AS EntryTemp
                            FROM REP_HM_SETUP_STEP STP
                            JOIN REP_HM_SETUP HDR
                            ON HDR.AREA_ID = STP.AREA_ID 
                            AND HDR.CENTER_ID = STP.CENTER_ID
                            AND HDR.IN_PIECE_NO = STP.IN_PIECE_NO
                            WHERE STP.AREA_ID = 'HSM' 
                            AND STP.CENTER_ID = 'FM' 
                            AND STP.IN_PIECE_NO = :ActualPieceNo 
                            AND STP.STEP_NO = :StepNo ";

            FinishingMillEntrySetupValueDto result = ctx.GetEntity<FinishingMillEntrySetupValueDto>(query, ctx.CreateParameter("ActualPieceNo", actualPieceNo),
                                                                                                 ctx.CreateParameter("StepNo", stepNo));

            if (result != null) { return result; }
            else { return new FinishingMillEntrySetupValueDto(); }
        }


        private FinishingMillExitSetupValueDto GetExitValues(int actualPieceNo, int stepNo)
        {
            string query = $@"SELECT STP.STEP_NO            AS StepNo 
                            ,STP.EXIT_THK                   AS ExitThickness
                            ,STP.EXIT_WIDTH                 AS ExitWidth
                            ,STP.MILL_SPEED * (1 + FSLIP)   AS ExitSpeed
                            ,HDR.TARGET_FLATNESS            AS Flatness
                            ,STP.EXIT_CROWN                 AS Crown
                            ,STP.EXIT_CROWN_ELLE            AS CrownDistance 
                            ,STP.EXIT_TEMP                  AS ExitTemp
                            FROM REP_HM_SETUP_STEP STP
                            JOIN REP_HM_SETUP HDR
                            ON HDR.AREA_ID = STP.AREA_ID 
                            AND HDR.CENTER_ID = STP.CENTER_ID
                            AND HDR.IN_PIECE_NO = STP.IN_PIECE_NO
                            WHERE STP.AREA_ID = 'HSM' 
                            AND STP.CENTER_ID = 'FM' 
                            AND STP.IN_PIECE_NO = :ActualPieceNo 
                            AND STP.STEP_NO = :StepNo  ";

            FinishingMillExitSetupValueDto result = ctx.GetEntity<FinishingMillExitSetupValueDto>(query, ctx.CreateParameter("ActualPieceNo", actualPieceNo),
                                                                                               ctx.CreateParameter("StepNo", stepNo));

            if (result != null) { return result; }
            else { return new FinishingMillExitSetupValueDto(); }

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
                                AND CENTER_ID = 'FM'
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