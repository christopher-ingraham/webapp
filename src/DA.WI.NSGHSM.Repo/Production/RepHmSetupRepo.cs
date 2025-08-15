using DA.DB.Utils;
using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.Production
{
    internal class RepHmSetupRepo<TDataSource> : IRepHmSetupRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public RepHmSetupRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public RepHmSetupDto GetCurrentREPSetupHeader(int inPieceNo)
        {
            string query = $@"SELECT 
                        TRIM(A.AREA_ID)                                                              AS AreaId
                       ,TRIM(A.CENTER_ID)                                                            AS CenterId
                       ,A.IN_PIECE_NO                                                                AS InPieceNo
                       ,TRIM(A.IN_PIECE_ID)                                                          AS InPieceId
                       ,TRIM(A.HEAT_ID)                                                              AS HeatId
                       ,TRIM(A.JOB_ID)                                                               AS JobId
                       ,TRIM(A.PRACTICE_ID)                                                          AS PracticeId
                       ,A.MILL_MODE                                                                  AS MillMode
                       ,A.DISABLED_STAND_BITMASK                                                     AS DisabledStandBitmask
                       ,TRIM(A.MATERIAL_GRADE_ID)                                                    AS MaterialGradeId
                       ,CAST(B.GRADE_GROUP_ID AS VARCHAR(10))  || ': ' || RTRIM(B.GRADE_GROUP_LABEL) AS GradeGroupLabel
                       ,A.ENTRY_WDT                                                                  AS EntryWdt
                       ,A.ENTRY_THK                                                                  AS EntryThk
                       ,A.ENTRY_TEMP                                                                 AS EntryTemp
                       ,A.PIECE_LENGTH                                                               AS PieceLength
                       ,A.PIECE_WEIGHT                                                               AS PieceWeight
                       ,A.DENSITY                                                                    AS Density
                       ,A.THERMAL_EXP_COEFF                                                          AS ThermalExpCoeff
                       ,A.TRANSFER_BAR_THK                                                           AS TransferBarThk
                       ,A.TRANSFER_BAR_WDT                                                           AS TransferBarWdt
                       ,A.TRANSFER_BAR_TEMP                                                          AS TransferBarTemp
                       ,A.TARGET_WDT                                                                 AS TargetWdt
                       ,A.TARGET_WDT_UP_TOL                                                          AS TargetWdtUpTol
                       ,A.TARGET_WDT_LO_TOL                                                          AS TargetWdtLoTol
                       ,A.TARGET_THK                                                                 AS TargetThk
                       ,A.TARGET_THK_UP_TOL                                                          AS TargetThkUpTol
                       ,A.TARGET_THK_LO_TOL                                                          AS TargetThkLoTol
                       ,A.DESCALER_MODE                                                              AS DescalerMode
                       ,A.DESCALER_HEAD_OFFSET                                                       AS DescalerHeadOffset
                       ,A.IC_HEAD_OFFSET                                                             AS IcHeadOffset
                       ,A.SETUP_MODE                                                                 AS SetupMode
                       ,CAST(A.SETUP_MODE AS VARCHAR(10)) || ': ' || RTRIM(E.VALUE_LABEL)            AS SetupModelLabel
                       ,TRIM(A.OPERATOR)                                                             AS Operator
                       ,A.REVISION                                                                   AS Revision
                  FROM  REP_HM_SETUP        A
                       ,TDB_GRADE_GROUP     B
                       ,TDB_MATERIAL_GRADE  C
                       ,AUX_VALUE           E
                  WHERE B.GRADE_GROUP_ID    = C.GRADE_GROUP_ID
                  AND   A.MATERIAL_GRADE_ID = C.MATERIAL_GRADE_ID
                  AND   A.SETUP_MODE        = E.INTEGER_VALUE
                  AND   E.VARIABLE_ID       = 'SETUP_MODE'
                  AND   A.AREA_ID     = 'HSM'
                  AND   (A.CENTER_ID   = 'FM' OR A.CENTER_ID   = 'RM')
                  AND   A.IN_PIECE_NO = :InPieceNo ";

            RepHmSetupDto result = ctx.GetEntity<RepHmSetupDto>(query, ctx.CreateParameter("InPieceNo", inPieceNo));
            
            if (result == null)
                throw new NotFoundException(typeof(RepHmSetupDto), inPieceNo);
            
            result.GeneralSettings = GetDispositionCodesForHold(inPieceNo);
            return result;
        }

        public RepHmSetupAllStepsListItemDto[] GetDispositionCodesForHold(int inPieceNo)
        {

            string query = $@"
                         SELECT A.STEP_NO                   AS StepNo
                        ,A.PASS_NO                          AS PassNo
                        ,A.STAND_NO                         AS StandNo
                        ,A.ENABLED_STAND                    AS EnabledStand
                        ,A.AGC_CON_MODE                     AS AgcConMode
                        ,A.ENTRY_WIDTH                      AS EntryWidth
                        ,A.EXIT_WIDTH                       AS ExitWidth
                        ,A.ENTRY_THK                        AS EntryThk
                        ,A.EXIT_THK                         AS ExitThk
                        ,A.ENTRY_LENGTH                     AS EntryLength
                        ,A.EXIT_LENGTH                      AS ExitLength
                        ,A.PVR_THK_HEAD                     AS PvrThkHead
                        ,A.PVR_LEN_HEAD                     AS PvrLenHead
                        ,A.PVR_THK_TAIL                     AS PvrThkTail
                        ,A.PVR_LEN_TAIL                     AS PvrLenTail
                        ,A.EDGER_TRG_WIDTH                  AS EdgerTrgWidth 
                        ,A.EDGER_FORCE                      AS EdgerForce
                        ,A.EDGER_SPEED                      AS EdgerSpeed
                        ,A.EDGER_STIFFNESS                  AS EdgerStiffness
                        ,A.EDGER_DELTA_WDT_HEAD             AS EdgerDeltaWdtHead
                        ,A.EDGER_DELTA_LEN_HEAD             AS EdgerDeltaLenHead
                        ,A.EDGER_DELTA_WDT_TAIL             AS EdgerDeltaWdtTail
                        ,A.EDGER_DELTA_LEN_TAIL             AS EdgerDeltaLenTail
                        ,A.ENTRY_STRIP_ROUGH                AS EntryStripRough 
                        ,A.EXIT_STRIP_ROUGH                 AS ExitStripRough
                        ,A.ENTRY_TEMP                       AS EntryTemp
                        ,A.EXIT_TEMP                        AS ExitTemp
                        ,A.ENTRY_TENSION                    AS SpecEntryTension 
                        ,A.EXIT_TENSION                     AS SpecExitTension
                        ,A.LOOPER_ANGLE                     AS LooperAngle
                        ,A.THREADING_SPEED                  AS ThreadingSpeed
                        ,A.MILL_SPEED                       AS MillSpeed
                        ,A.FSLIP                            AS Fslip
                        ,A.BSLIP                            AS Bslip
                        ,A.DRAFT                            AS Draft
                        ,A.BITE_ANGLE                       AS BiteAngle
                        ,A.REDUCTION                        AS Reduction
                        ,A.GAP                              AS Gap
                        ,A.STIFFNESS                        AS Stiffness
                        ,A.FORCE                            AS Force
                        ,A.MOTOR_TORQUE                     AS MotorTorque
                        ,A.MOTOR_POWER                      AS MotorPower
                        ,A.AVG_YS                           AS AvgYs
                        ,A.MU_EST                           AS MuEst
                        ,A.STRETCH_EST_EN                   AS StretchEstEn
                        ,A.H_ENTRY_TEMP                     AS HEntryTemp
                        ,A.H_EXIT_TEMP                      AS HExitTemp
                        ,A.H_ENTRY_WIDTH                    AS HEnrtyWidth
                        ,A.H_EXIT_WIDTH                     AS HExitWidth
                        ,A.H_ENTRY_THK                      AS HEntryThk
                        ,A.H_EXIT_THK                       AS HExitThk
                        ,A.H_GAP                            AS HGap
                        ,A.H_STIFFNESS                      AS HStiffness
                        ,A.H_FSLIP                          AS HFslip
                        ,A.H_BSLIP                          AS HBslip
                        ,A.H_FORCE                          AS HForce
                        ,A.H_MOTOR_TORQUE                   AS HMotorTorque
                        ,A.H_MOTOR_POWER                    AS HMotorPower
                        ,A.H_WR_BEND                        AS HWrBend
                        ,A.H_GAP_OFFSET                     AS HGapOffset
                        ,A.ENTRY_SHEAR_GAP                  AS EntryShearGap
                        ,A.EXIT_SHEAR_GAP                   AS ExitShearGap
                        ,A.EXIT_CROWN                       AS ExitCrown
                        ,A.EXIT_CROWN_ELLE                  AS ExitCrownElle
                        ,A.PASS_TARGET_PROFILE_A2           AS PassTargetProfileA2
                        ,A.PASS_TARGET_PROFILE_A4           AS PassTargetProfileA4
                        ,A.WR_BEND                          AS WrBend      
                        ,A.DWR_BEND                         AS DwrBend     
                        ,A.GAP_OFFSET                       AS GapOffset   
                        ,A.TH_EXP_OFFSET                    AS ThExpOffset
                        ,A.WR_BEND_SLOPE                    AS WrBendSlope
                        ,A.WR_BEND_CROWN                    AS WrBendCrown
                        ,A.WR_SHIFT                         AS WrShift
                        ,A.HEAD_NO_COOLING                  AS HeadNoCooling
                        ,A.TAIL_NO_COOLING                  AS TailNoCooling
                        ,A.WR_COOLING_FLOW                  AS WrCoolingFlow
                        ,A.WR_COOLING_NARROW                AS WrCoolingNarrow
                        ,A.WR_COOLING_MIDDLE                AS WrCoolingMiddle
                        ,A.WR_COOLING_WIDE                  AS WrCoolingWide
                        ,A.STRIP_COOLING_FLOW               AS StripCoolingFlow
                        ,A.FORCE_TO_ENTRY_THK               AS ForceToEntryThk
                        ,A.FORCE_TO_EXIT_THK                AS ForceToExitThk
                        ,A.FORCE_TO_ENTRY_TENS              AS ForceToEntryTens
                        ,A.FORCE_TO_EXIT_TENS               AS ForceToExitTens
                        ,A.FORCE_TO_MILL_SPEED              AS ForceToMillSpeed
                        ,A.VALIDITY_SPEED_RANGE             AS ValiditySpeedRange
                        ,A.SPEED_TO_EXIT_TEMP               AS SpeedToExitTemp
                        ,A.STRIP_COOL_TO_EXIT_TEMP          AS StripCoolToExitTemp
                        ,A.WRB_TO_FLATNESS                  AS WrbToFlatness
                        ,A.WRB_TO_PROFILE                   AS WrbToProfile
                        ,A.ROLL_EXT_TEMP                    AS RollExtTemp
                        ,A.THK_LAST_DEV                     AS ThkLastDev
                        ,A.ROLL_BITE_OIL_PERC               AS RollBiteOilPerc
                        ,A.HEAD_CROP_LENGTH                 AS HeadCropLength
                        ,A.TAIL_CROP_LENGTH                 AS TailCropLength
                  FROM  REP_HM_SETUP_STEP  A
                  WHERE A.AREA_ID    = 'HSM'
                  AND   (A.CENTER_ID  = 'FM' OR A.CENTER_ID  = 'RM')
                  AND   A.IN_PIECE_NO = :InPieceNo ";

            var data = ctx.GetEntities<RepHmSetupAllStepsListItemDto>(query, ctx.CreateParameter("InPieceNo", inPieceNo)).ToArray();

            return data;
        }
    }
}