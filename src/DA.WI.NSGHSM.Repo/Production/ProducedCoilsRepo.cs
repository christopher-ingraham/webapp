using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    internal class ProducedCoilsRepo<TDataSource> : IProducedCoilsRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public ProducedCoilsRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<ProducedCoilsListItemDto> SelProdCoils(ListRequestDto<ProducedCoilsListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT  A.OUT_PIECE_NO  AS OutPieceNo
                           ,TRIM(A.OUT_PIECE_ID)     AS OutPieceId
                           ,TRIM(A.IN_PIECE_ID)      AS InPieceId
                           ,A.IN_PIECE_NO            AS InPieceNo
                           ,TRIM(H.HEAT_ID)          AS HeatId
                           ,TRIM(E.MATERIAL_GRADE_ID)  AS MaterialGradeId
                           ,G.GRADE_GROUP_ID         AS GradeGroupId
                           ,LTRIM(TO_CHAR(G.GRADE_GROUP_ID, '999'))  || ': ' || RTRIM(G.GRADE_GROUP_LABEL)  AS GradeGroupLabel
                           , COALESCE(R.TARGET_COLD_WDT, 0)       AS TargetColdWidth
                           ,A.EXIT_WIDTH            AS ExitWidth
                           , COALESCE(R.TARGET_COLD_THK, 0)       AS TargetColdThk
                           ,A.EXIT_THK              AS ExitThk
                           ,A.EXIT_TEMP             AS ExitTemp
                           ,A.CALCULATED_WEIGHT     AS CalculatedWeight
                           ,A.MEASURED_WEIGHT       AS MeasurededWeight
                           ,A.INNER_DIAMETER        AS InnerDiameter
                           ,A.OUTER_DIAMETER        AS OuterDiameter
                           ,A.LENGTH                AS Length
                           ,TRIM(O.CUSTOMER_ID)     AS CustomerId
                           ,A.PRODUCTION_START_DATE AS ProductionStartDate
                           ,A.PRODUCTION_STOP_DATE  AS ProductionStopDate
                           ,A.STATUS                AS Status
                           ,LTRIM(TO_CHAR(A.STATUS, '999')) || ': ' || RTRIM(C.VALUE_LABEL) AS StatusLabel
                           ,TRIM(A.OPERATOR)        AS Operator
                           ,A.REVISION              AS Revision
                       FROM REP_HM_PIECE   A
                       LEFT OUTER JOIN REP_HM_SETUP R
                       ON R.IN_PIECE_NO = A.IN_PIECE_NO
                       AND R.AREA_ID = 'HSM'
                       AND R.CENTER_ID = 'FM'
                       LEFT OUTER JOIN HRM_INPUT_PIECE E
                       ON E.PIECE_NO = A.IN_PIECE_NO
                       LEFT OUTER JOIN HRM_HEAT H
                       ON H.HEAT_NO = E.HEAT_NO
                       LEFT OUTER JOIN HRM_ORDER O
                       ON O.PIECE_NO = A.IN_PIECE_NO
                           ,AUX_VALUE               C
                           ,TDB_GRADE_GROUP         G
                           ,TDB_MATERIAL_GRADE      S
                  WHERE     A.STATUS                = C.INTEGER_VALUE
                  AND       C.VARIABLE_ID           = 'STATUS_PRODUCED'
                  AND       E.MATERIAL_GRADE_ID     = S.MATERIAL_GRADE_ID
                  AND       S.GRADE_GROUP_ID        = G.GRADE_GROUP_ID ";


            string queryCount = @"SELECT COUNT(A.OUT_PIECE_NO)
                                FROM REP_HM_PIECE   A
                       LEFT OUTER JOIN REP_HM_SETUP R
                       ON R.IN_PIECE_NO = A.IN_PIECE_NO
                       AND R.AREA_ID = 'HSM'
                       AND R.CENTER_ID = 'FM'
                       LEFT OUTER JOIN HRM_INPUT_PIECE E
                       ON E.PIECE_NO = A.IN_PIECE_NO
                       LEFT OUTER JOIN HRM_HEAT H
                       ON H.HEAT_NO = E.HEAT_NO
                       LEFT OUTER JOIN HRM_ORDER O
                       ON O.PIECE_NO = A.IN_PIECE_NO
                           ,AUX_VALUE               C
                           ,TDB_GRADE_GROUP         G
                           ,TDB_MATERIAL_GRADE      S
                  WHERE     A.STATUS                = C.INTEGER_VALUE
                  AND       C.VARIABLE_ID           = 'STATUS_PRODUCED'
                  AND       E.MATERIAL_GRADE_ID     = S.MATERIAL_GRADE_ID
                  AND       S.GRADE_GROUP_ID        = G.GRADE_GROUP_ID
                  AND 1=1 ";

            var searchProductionStopDateFrom = listRequest.Filter?.SearchProductionStopDateFrom;
            if (searchProductionStopDateFrom != null)
            {
                query += "AND A.PRODUCTION_STOP_DATE >= :SearchProductionStopDateFrom ";
                queryCount += "AND A.PRODUCTION_STOP_DATE >= :SearchProductionStopDateFrom ";
                queryParam.Add(ctx.CreateParameter("SearchProductionStopDateFrom", searchProductionStopDateFrom, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchProductionStopDateFrom", searchProductionStopDateFrom, DbType.DateTimeOffset));
            }

            var searchProductionStopDateTo = listRequest.Filter?.SearchProductionStopDateTo;
            if (searchProductionStopDateTo != null)
            {
                query += "AND A.PRODUCTION_STOP_DATE <= :SearchProductionStopDateTo ";
                queryCount += "AND A.PRODUCTION_STOP_DATE <= :SearchProductionStopDateTo ";
                queryParam.Add(ctx.CreateParameter("SearchProductionStopDateTo", searchProductionStopDateTo, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchProductionStopDateTo", searchProductionStopDateTo, DbType.DateTimeOffset));
            }

            var searchProducedPieceId = listRequest.Filter?.SearchProducedPieceId;
            if (searchProducedPieceId != null)
            {
                query += "AND (A.OUT_PIECE_ID LIKE :SearchProducedPieceId||'%') ";
                queryCount += "AND (A.OUT_PIECE_ID LIKE :SearchProducedPieceId||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchProducedPieceId", searchProducedPieceId));
                queryCountParam.Add(ctx.CreateParameter("SearchProducedPieceId", searchProducedPieceId));
            }

            var searchInputSlabNumber = listRequest.Filter?.SearchInputSlabNumber;
            if (searchInputSlabNumber != null)
            {
                query += "AND (A.IN_PIECE_ID LIKE :SearchInputSlabNumber||'%') ";
                queryCount += "AND (A.IN_PIECE_ID LIKE :SearchInputSlabNumber||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchInputSlabNumber", searchInputSlabNumber));
                queryCountParam.Add(ctx.CreateParameter("SearchInputSlabNumber", searchInputSlabNumber));
            }

            var searchHeatNumber = listRequest.Filter?.SearchHeatNumber;
            if (searchHeatNumber != null)
            {
                query += "AND (H.HEAT_ID LIKE :SearchHeatNumber||'%') ";
                queryCount += "AND (H.HEAT_ID LIKE :SearchHeatNumber||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchHeatNumber", searchHeatNumber));
                queryCountParam.Add(ctx.CreateParameter("SearchHeatNumber", searchHeatNumber));
            }

            var searchCoilStatus = listRequest.Filter?.SearchCoilStatus;
            if (searchCoilStatus != null)
            {
                query += "AND (A.STATUS = :SearchCoilStatus) ";
                queryCount += "AND (A.STATUS = :SearchCoilStatus) ";
                queryParam.Add(ctx.CreateParameter("SearchCoilStatus", searchCoilStatus));
                queryCountParam.Add(ctx.CreateParameter("SearchCoilStatus", searchCoilStatus));
            }

            query += "ORDER BY  A.PRODUCTION_STOP_DATE  DESC ";

            var data = ctx.GetEntities<ProducedCoilsListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<ProducedCoilsListItemDto>
            {
                Data = data,
                Total = total
            };
        }
        public void UpdRepHmProdCoil(ProducedCoilsForUpdateDto dto, long outPieceNo)
        {
            int inPieceNo = findInPieceNo(dto.InPieceId);

            string update = $@"UPDATE REP_HM_PIECE SET
            OUT_PIECE_ID = :OutPieceId, OUT_PIECE_CNT = :OutPieceCnt, OUT_PIECE_SEQ = :OutPieceSeq, IN_PIECE_ID = :InPieceId,
            JOB_ID = :JobId, PRODUCTION_START_DATE = :ProductionStartDate, PRODUCTION_STOP_DATE = :ProductionStopDate, MEASURED_WEIGHT = :MaesuredWeight,
            CALCULATED_WEIGHT = :CalculatedWeight, LENGTH = :Length, EXIT_THK = :ExitThk, TARGET_WIDTH = :TargetWidth, TARGET_THICKNESS = :TargetThickness,
            INNER_DIAMETER = :InnerDiameter, OUTER_DIAMETER = :OuterDiameter, END_OF_INGOT_FLAG = :EndOfIngotFlag, TEST_CUT = :TestCut, TRIAL_FLAG = :TrialFlag,
            CREW_ID = :CrewId, SHIFT_ID = :ShiftId, STATUS = :Status, REMARK = :Remark, SOAK_TIME = :SoakTime, GAP_TIME = :GapTime, ROLLING_TIME = :RollingTime,
            STRIP_CROWN_INTOL_FLAG = :StripCrownIntolFlag, EXIT_THK_INTOL_FLAG = :ExitThkIntolFlag, EXIT_WIDTH_INTOL_FLAG = :ExitWidthIntolFlag, EXIT_WIDTH = :ExitWidth,
            EXIT_TEMP_INTOL_FLAG = :ExitTempIntolFlag, DOWNCOIL_TEMP_INTOL_FLAG = :DowncoilTempIntolFlag, INTERSTAND_COOLING_BITMASK = :InterstandCoolingBitmask,
            OPERATOR = :Operator, REVISION = :Revision
            WHERE OUT_PIECE_NO = {outPieceNo} ";

            if (validate(outPieceNo))
            {
                int affectedRows = ctx.ExecuteNonQuery(update,
                    ctx.CreateParameter("OutPieceId", dto.OutPieceId),     //Produced Coil ID
                    ctx.CreateParameter("OutPieceCnt", findMax("OUT_PIECE_CNT", "REP_HM_PIECE") + 1),
                    ctx.CreateParameter("OutPieceSeq", dto.OutPieceSeq),   // Coil Sequence
                    ctx.CreateParameter("InPieceNo", inPieceNo),       //  Lookup
                    ctx.CreateParameter("InPieceId", dto.InPieceId),       //  Input Slab ID
                    ctx.CreateParameter("JobId", dto.JobId),               // String No
                    ctx.CreateParameter("ProductionStartDate", dto.ProductionStartDate),  // Start Production Time
                    ctx.CreateParameter("ProductionStopDate", dto.ProductionStopDate),    // Stop Production Time
                    ctx.CreateParameter("MaesuredWeight", dto.MeasuredWeight),           //  Measured Weight
                    ctx.CreateParameter("CalculatedWeight", dto.CalculatedWeight),       // Calculated Weight
                    ctx.CreateParameter("Length", dto.Length),                           // Exit Length
                    ctx.CreateParameter("ExitThk", dto.ExitThk),                        //  Exit Thickness
                    ctx.CreateParameter("TargetWidth", dto.TargetWidth),                // Target Width 
                    ctx.CreateParameter("TargetThickness", dto.TargetThickness),        // Target Thickness
                    ctx.CreateParameter("InnerDiameter", dto.InnerDiameter),          // Internal Diameter
                    ctx.CreateParameter("OuterDiameter", dto.OuterDiameter),          // Outer Diameter
                    ctx.CreateParameter("EndOfIngotFlag", dto.EndOfInGotFlag),       //  [TAB Disposition Data] EndOfInGotFlag
                    ctx.CreateParameter("TestCut", dto.TestCut),                    //  [TAB Disposition Data] Test Cut
                    ctx.CreateParameter("TrialFlag", dto.TrialFlag),                //  Trial Coil
                    ctx.CreateParameter("ExitWidth", dto.ExitWidth),
                    ctx.CreateParameter("CrewId", dto.CrewId),                      // Production Crew
                    ctx.CreateParameter("ShiftId", dto.ShiftId),                    // Production Shift
                    ctx.CreateParameter("Status", dto.Status),                      // Coil Status
                    ctx.CreateParameter("Remark", dto.Remark),                      // Note
                    ctx.CreateParameter("SoakTime", dto.SoakTime),                  // Soak Time
                    ctx.CreateParameter("GapTime", dto.GapTime),                    // Gap Time
                    ctx.CreateParameter("RollingTime", dto.RollingTime),            //   Rolling Time
                    ctx.CreateParameter("StripCrownIntolFlag", dto.StripCrownIntolFlag),   // [TAB Measured Data] Strip Profile [mm] --> colonna In Tolerance
                    ctx.CreateParameter("ExitThkIntolFlag", dto.ExitThkIntolFlag),          // [TAB Measured Data] Exit thickness [mm] --> colonna In Tolerance
                    ctx.CreateParameter("ExitWidthIntolFlag", dto.ExitWidthIntolFlag),          // [TAB Measured Data] Exit Width [mm] --> colonna In Tolerance
                    ctx.CreateParameter("ExitTempIntolFlag", dto.ExitTempIntolFlag),                 // [TAB Measured Data] Finishing Temperature [^C] --> colonna In Tolerance
                    ctx.CreateParameter("DowncoilTempIntolFlag", dto.DowncoilTempIntolFlag),         // [TAB Measured Data] Coiling Temperature [^C] --> colonna In Tolerance
                    ctx.CreateParameter("InterstandCoolingBitmask", dto.InterstandCoolingBitmask),   // [TAB Rolls Data]  Interstand Cooling bitmask
                    ctx.CreateParameter("Operator", dto.Operator),                                    // Operator
                    ctx.CreateParameter("Revision", DateTime.Now)
                );

                UpdRepHmHold(dto.DispositionCodesForHold, outPieceNo);
            }
            else
            {
                var badRequestMap = new Dictionary<string, BadRequest>();
                badRequestMap.Add(nameof(outPieceNo), new BadRequest(BadRequestType.NOT_EXISTS));
                throw new BadRequestException(badRequestMap);
            }

        }

        public ListResultDto<ProducedCoilsLookupDto> Lookup(ListRequestDto<ProducedCoilsLookupDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = String.Format(
                @"SELECT
                    TRIM(PIECE_ID) AS Display,
                    PIECE_NO AS Value
                FROM
                    HRM_INPUT_PIECE
                ORDER BY
                    PIECE_NO ASC"
            );

            string queryCount = String.Format(
                @"SELECT
                    COUNT(PIECE_NO)
                FROM
                    HRM_INPUT_PIECE"
            );

            var data = ctx.GetEntities<ProducedCoilsLookupDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<ProducedCoilsLookupDto>
            {
                Data = data,
                Total = total
            };
        }


        public void UpdRepHmHold(Boolean[] dispositionCodesForHold, long outPieceNo)
        {
            for (int i = 0; i < 30; i++)
            {

                if (dispositionCodesForHold[i] == true)
                {
                    string update = $@"UPDATE REP_HM_HOLD SET
                        OPER_CANCEL_FLAG = 1
                        WHERE OUT_PIECE_NO = :OutPieceNo
                        AND VALUE_SEQ = {i + 1} ";
                    ctx.ExecuteNonQuery(update, ctx.CreateParameter("OutPieceNo", outPieceNo));
                }
                else
                {
                    string update = $@"UPDATE REP_HM_HOLD SET
                        OPER_CANCEL_FLAG = 0
                        WHERE OUT_PIECE_NO = :OutPieceNo
                        AND VALUE_SEQ = {i + 1} ";
                    ctx.ExecuteNonQuery(update, ctx.CreateParameter("OutPieceNo", outPieceNo));
                }
            }
        }

        public ProducedCoilsDetailDto GetCurrentOutputCoil(long outPieceNo)
        {
            string query = @"SELECT TRIM(A.OUT_PIECE_ID) AS OutPieceId
                       ,A.OUT_PIECE_CNT            AS OutPieceCnt
                       ,A.OUT_PIECE_SEQ            AS OutPieceSeq
                       ,TRIM(A.IN_PIECE_ID)        AS InPieceId
                       ,A.IN_PIECE_NO              AS InPieceNo
                       ,TRIM(B.JOB_ID)             AS JobId
                       ,B.JOB_PIECE_SEQ            AS JobPieceSeq
                       ,A.PRODUCTION_START_DATE    AS ProductionStartDate
                       ,CASE WHEN A.PRODUCTION_STOP_DATE IS NULL THEN A.PRODUCTION_START_DATE ELSE A.PRODUCTION_STOP_DATE END AS ProductionStopDate
                       ,A.MEASURED_WEIGHT          AS MeasuredWeight
                       ,A.CALCULATED_WEIGHT        AS CalculatedWeight
                       ,A.LENGTH                   AS Length
                       ,B.LENGTH                   AS EntryLength
                       ,A.EXIT_THK                 AS ExitThk
                       ,A.EXIT_WIDTH               AS ExitWidth
                       ,B.THICKNESS                AS EntryThickness
                       ,A.TARGET_WIDTH             AS TargetWidth
                       ,B.USE_DEFAULT_CHEM_COMP    AS UsedDefaultChemComp
                       ,A.TARGET_THICKNESS         AS TargetThickness
                       ,A.INNER_DIAMETER           AS InnerDiameter
                       ,A.OUTER_DIAMETER           AS OuterDiameter
                       ,A.END_OF_INGOT_FLAG        AS EndOfInGotFlag
                       ,A.TEST_CUT                 AS TestCut
                       ,A.TRIAL_FLAG               AS TrialFlag
                       ,TRIM(A.CREW_ID)            AS CrewId
                       ,A.SHIFT_ID                 AS ShiftId
                       ,A.STATUS                   AS Status
                       ,A.REMARK                   AS Remark
                       ,A.SOAK_TIME                AS SoakTime
                       ,A.GAP_TIME                 AS GapTime
                       ,A.ROLLING_TIME             AS RollingTime
                       ,A.STRIP_CROWN_TOL_PERC     AS StripCrownTolPerc
                       ,A.STRIP_CROWN_INTOL_FLAG   AS StripCrownIntolFlag
                       ,A.EXIT_THK_TOL_PERC        AS ExitThkTolPerc
                       ,A.EXIT_THK_INTOL_FLAG      AS ExitThkIntolFlag
                       ,A.EXIT_WIDTH_TOL_PERC      AS ExitWidthTolPerc
                       ,A.EXIT_WIDTH_INTOL_FLAG    AS ExitWidthIntolFlag
                       ,A.EXIT_TEMP_TOL_PERC       AS ExitTempTolPerc
                       ,A.EXIT_TEMP_INTOL_FLAG     AS ExitTempIntolFlag
                       ,A.DOWNCOIL_TEMP_TOL_PERC   AS DowncoilTempTolPerc
                       ,A.DOWNCOIL_TEMP_INTOL_FLAG AS DowncoilTempIntolFlag
                       ,TRIM(A.OPERATOR)           AS Operator
                       ,A.REVISION                 AS Revision
                       ,TRIM(C.ORDER_NUMBER)       AS OrderNumber
                       ,TRIM(C.ORDER_POSITION)     AS OrderPosition
                       ,TRIM(C.CUSTOMER_ID)        AS CustomerId
                       ,A.INTERSTAND_COOLING_BITMASK AS InterstandCoolingBitmask
                       ,TRIM(D.HEAT_ID)            AS HeatId
                       ,B.TARGET_WIDTH             AS TargetColdWidth
                       ,TRIM(B.MATERIAL_GRADE_ID)  AS MaterialGradeId
                       ,LTRIM(TO_CHAR(G.GRADE_GROUP_ID, '999'))  || ': ' || RTRIM(G.GRADE_GROUP_LABEL) AS GradeGroupId
                       ,B.WIDTH_HEAD               AS EntryHeadWidth
                       ,B.THICKNESS_HEAD           AS EntryHeadThickness
                  FROM  REP_HM_PIECE            A
                       ,HRM_INPUT_PIECE         B
                       ,HRM_ORDER               C
                       ,HRM_HEAT                D
                       ,TDB_GRADE_GROUP         G
                       ,TDB_MATERIAL_GRADE      S
                  WHERE A.OUT_PIECE_NO = :OutPieceNo
                  AND   A.IN_PIECE_NO  = B.PIECE_NO
                  AND   B.PIECE_NO     = C.PIECE_NO
                  AND   B.HEAT_NO     = D.HEAT_NO
                  AND   B.MATERIAL_GRADE_ID     = S.MATERIAL_GRADE_ID
                  AND   S.GRADE_GROUP_ID        = G.GRADE_GROUP_ID  ";

            ProducedCoilsDetailDto result = ctx.GetEntity<ProducedCoilsDetailDto>(query, ctx.CreateParameter("OutPieceNo", outPieceNo));

            if (result == null)
            {
                var badRequestMap = new Dictionary<string, BadRequest>();
                badRequestMap.Add(nameof(outPieceNo), new BadRequest(BadRequestType.NOT_EXISTS));
                throw new BadRequestException(badRequestMap);
            }

            result.InputCoilTrgMeas = GetInputCoilTrgMeas(outPieceNo);
            result.OutCoilSetupIntermediateTemp = GetOutCoilSetupIntermediateTemp(outPieceNo);
            result.RollDataForStands = GetCurrentProducedCoilsDetail(outPieceNo);
            result.DispositionCodesForHold = GetDispositionCodesForHold(outPieceNo);
            result.TotalReduction = CalcReduction(result.EntryHeadThickness, result.ExitThk);
            result.CurrentOutCoilMeas = GetCurrentOutCoilMeas(outPieceNo);
            result.OutPieceNo = outPieceNo;

            return result;
        }

        public static double CalcReduction(double entryThickness, double exitThickness)
        {
            double fReduction = 0;
            double fDraft = 0;


            if ((entryThickness > 0) && (exitThickness >= 0) && (entryThickness >= exitThickness))
            {
                fReduction = (entryThickness - exitThickness) * 100 / entryThickness;
                fDraft = entryThickness - exitThickness;
            }

            return fReduction;
        }

        public InputCoilTrgMeasDetailDto GetInputCoilTrgMeas(long outPieceNo)
        {
            string query = @"SELECT HRM_INPUT_PIECE.TARGET_WIDTH        AS TargetWidth,
                          TARGET_WIDTH_PTOL                             AS TargetWidthPtol,
                          TARGET_WIDTH_NTOL                             AS TargetWidthNtol,
                          HRM_INPUT_PIECE.TARGET_THICKNESS              AS TargetThickness,
                          TARGET_THICKNESS_PTOL                         AS TargetThicknessPtol,
                          TARGET_THICKNESS_NTOL                         AS TargetThicknessNtol,
                          TARGET_TEMP_FM                                AS TargetTempFm,
                          TARGET_TEMP_FM_PTOL                           AS TargetTempFmPtol,
                          TARGET_TEMP_FM_NTOL                           AS TargetTempFmNtol,
                          TARGET_TEMP_DC                                AS TargetTempDc,
                          TARGET_TEMP_DC_PTOL                           AS TargetTempDcPtol,
                          TARGET_TEMP_DC_NTOL                           AS TargetTempDcNtol,
                          TARGET_PROFILE                                AS TargetProfile,
                          TARGET_PROFILE_PTOL                           AS TargetProfilePtol,
                          TARGET_PROFILE_NTOL                           AS TargetProfileNtol
                  FROM HRM_INPUT_PIECE INNER JOIN REP_HM_PIECE ON (PIECE_NO = IN_PIECE_NO)
                  WHERE OUT_PIECE_NO = :OutPieceNo ";

            InputCoilTrgMeasDetailDto result = ctx.GetEntity<InputCoilTrgMeasDetailDto>(query, ctx.CreateParameter("OutPieceNo", outPieceNo));

            if (result == null)
            {
                result = new InputCoilTrgMeasDetailDto();
                // throw new NotFoundException(typeof(InputCoilTrgMeasDetailDto), outPieceNo);
            }
            return result;
        }

        public Boolean[] GetDispositionCodesForHold(long outPieceNo)
        {

            string query = $@"SELECT
                           A.REASON_CNT             AS ReasonCnt
                          ,A.OPER_CANCEL_FLAG       AS OperCancelFlag
                          ,B.VALUE_SEQ              AS ValueSeq
                          ,TRIM(B.VALUE_NAME)       AS ValueName
                          ,TRIM(A.DESCRIPTION)      AS Description
                   FROM    REP_HM_HOLD A,
                           AUX_VALUE B
                   WHERE  (B.VARIABLE_ID  = 'DISPOSITION_CODES')
                    AND   (A.OUT_PIECE_NO = :OutPieceNo)
                    AND   (A.VALUE_SEQ    = B.VALUE_SEQ) ";

            string queryCount = $@"SELECT COUNT(A.OUT_PIECE_NO)
                                FROM    REP_HM_HOLD A,
                           AUX_VALUE B
                   WHERE  (B.VARIABLE_ID  = 'DISPOSITION_CODES')
                    AND   (A.OUT_PIECE_NO = :OutPieceNo)
                    AND   (A.VALUE_SEQ    = B.VALUE_SEQ) ";

            query += "ORDER BY B.VALUE_SEQ ASC ";


            var total = ctx.GetEntity<int>(queryCount, ctx.CreateParameter("OutPieceNo", outPieceNo));
            var data = ctx.GetEntities<DispositionCodesForHoldDto>(query, ctx.CreateParameter("OutPieceNo", outPieceNo)).ToArray();

            Boolean[] dispositionCodesForHold = new Boolean[30];

            if (data.Length == 30)
            {
                for (int i = 0; i < 30; i++)
                {
                    if (data[i].OperCancelFlag == 1)
                    {
                        dispositionCodesForHold[i] = true;
                    }
                    else
                    {
                        dispositionCodesForHold[i] = false;
                    }

                }
            }
            return dispositionCodesForHold;
        }

        public OutCoilSetupIntermediateTempDetailDto GetOutCoilSetupIntermediateTemp(long outPieceNo)
        {
            string query = @"SELECT TARGET_TEMP_INTERM      AS TargetTempInterm,
                          TARGET_TEMP_INTERM_UP_TOL         AS TargetTempIntermUpTol,
                          TARGET_TEMP_INTERM_LO_TOL         AS TargetTempIntermLoTol
                    FROM   HRM_INPUT_PIECE  C
                          INNER JOIN REP_HM_SETUP B ON (PIECE_NO = B.IN_PIECE_NO)
                          INNER JOIN REP_HM_PIECE A ON (PIECE_NO = A.IN_PIECE_NO)
                    WHERE OUT_PIECE_NO = :OutPieceNo ";

            OutCoilSetupIntermediateTempDetailDto result = ctx.GetEntity<OutCoilSetupIntermediateTempDetailDto>(query, ctx.CreateParameter("OutPieceNo", outPieceNo));

            if (result == null)
                result = new OutCoilSetupIntermediateTempDetailDto();
            //throw new NotFoundException(typeof(OutCoilSetupIntermediateTempDetailDto), outPieceNo);
            return result;
        }

        protected RepHmRollDetailDto GetRollDataForStands(long outPieceNo, int standNo)
        {
            string query = $@" SELECT
                    OUT_PIECE_NO            AS OutPieceNo
                   ,TRIM(AREA_ID)           AS AreaId
                   ,TRIM(CENTER_ID)         AS CenterId
                   ,STAND_NO                AS StandNo
                   ,TRIM(WR_LO_ID)          AS WrLoId
                   ,WR_LO_DIAMETER          AS WrLoDiameter
                   ,WR_LO_ROLLED_LEN        AS WrLoRolledLen
                   ,TRIM(WR_UP_ID)          AS WrUpId
                   ,WR_UP_DIAMETER          AS WrUpDiameter
                   ,WR_UP_ROLLED_LEN        AS WrUpRolledLen
                   ,TRIM(BR_LO_ID)          AS BrLoId
                   ,BR_LO_DIAMETER          AS BrLoDiameter
                   ,BR_LO_ROLLED_LEN        AS BrLoRolledLen
                   ,TRIM(BR_UP_ID)          AS BrUpId
                   ,BR_UP_DIAMETER          AS BrUpDiameter
                   ,BR_UP_ROLLED_LEN        AS BrUpRolledLen
                    FROM   REP_HM_ROLL
                    WHERE  (OUT_PIECE_NO = :OutPieceNo)
                    AND    (CENTER_ID    = 'FM')
                    AND    (STAND_NO     = :StandNo) ";

            var result = ctx.GetEntity<RepHmRollDetailDto>(query, ctx.CreateParameter("OutPieceNo", outPieceNo), 
                                                                  ctx.CreateParameter("StandNo", standNo));

            return result;
        }


        public RepHmRollDetailDto[] GetCurrentProducedCoilsDetail(long outPieceNo)
        {
            RepHmRollDetailDto[] standsArray = new RepHmRollDetailDto[6];

            if (outPieceNo != 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    standsArray[i] = GetRollDataForStands(outPieceNo, i + 1);
                }
            }
            return standsArray;
        }


        public long InsRepHmProdCoil(ProducedCoilsForInsertDto dto)
        {
            long outPieceNo = findMax("OUT_PIECE_NO", "REP_HM_PIECE") + 1;
            int inPieceNo = findInPieceNo(dto.InPieceId);

            string insert = @"INSERT INTO REP_HM_PIECE
                  (
                      OUT_PIECE_NO
                     ,OUT_PIECE_ID
                     ,OUT_PIECE_CNT
                     ,OUT_PIECE_SEQ
                     ,IN_PIECE_NO
                     ,IN_PIECE_ID
                     ,JOB_ID
                     ,PRODUCTION_START_DATE
                     ,PRODUCTION_STOP_DATE
                     ,MEASURED_WEIGHT
                     ,CALCULATED_WEIGHT
                     ,LENGTH
                     ,EXIT_THK
                     ,TARGET_WIDTH
                     ,TARGET_THICKNESS
                     ,INNER_DIAMETER
                     ,OUTER_DIAMETER
                     ,END_OF_INGOT_FLAG
                     ,TEST_CUT
                     ,TRIAL_FLAG
                     ,CREW_ID
                     ,SHIFT_ID
                     ,STATUS
                     ,REMARK
                     ,SOAK_TIME
                     ,GAP_TIME
                     ,ROLLING_TIME
                     ,STRIP_CROWN_TOL_PERC
                     ,STRIP_CROWN_INTOL_FLAG
                     ,EXIT_THK_TOL_PERC
                     ,EXIT_THK_INTOL_FLAG
                     ,EXIT_WIDTH_TOL_PERC
                     ,EXIT_WIDTH_INTOL_FLAG
                     ,EXIT_TEMP_TOL_PERC
                     ,EXIT_TEMP_INTOL_FLAG
                     ,DOWNCOIL_TEMP_TOL_PERC
                     ,DOWNCOIL_TEMP_INTOL_FLAG
                     ,INTERSTAND_COOLING_BITMASK
                     ,OPERATOR
                     ,REVISION
                     ,OUT_PIECE_AREA
                     ,DEST_CODE_ID
                     ,COMBINATION_NO
                    ) VALUES (
                        :OutPieceNo, :OutPieceId, :OutPieceCnt, :OutPieceSeq, :InPieceNo, :InPieceId, :JobId, :ProductionStartDate, :ProductionStopDate,
                        :MeasuredWeight, :CalculatedWeight, :Length, :ExitThk, :TargetWidth, :TargetThickness, :InnerDiameter, :OuterDiameter,
                        :EndOfIngotFlag, :TestCut, :TrialFlag, :CrewId, :ShiftId, :Status, :Remark, :SoakTime,
                        :GapTime, :RollingTime, :StripCrownTolPerc, :StripCrownIntolFlag, :ExitThkTolPerc, :ExitThkIntolFlag,
                        :ExitWidthTolPerc, :ExitWidthIntolFlag,
                        :ExitTempTolPerc, :ExitTempIntolFlag, :DowncoilTempTolPerc, :DowncoilTempIntolFlag, :InterstandCoolingBitmask,
                        :Operator, :Revision, :OutPieceArea, :DestCodeId, :CombinationNo
                    )";


            ctx.ExecuteNonQuery(insert,
                ctx.CreateParameter("OutPieceNo", findMax("OUT_PIECE_NO", "REP_HM_PIECE") + 1),
                ctx.CreateParameter("OutPieceId", dto.OutPieceId),   //Produced Coild ID
                ctx.CreateParameter("OutPieceCnt", findMax("OUT_PIECE_CNT", "REP_HM_PIECE") + 1),
                ctx.CreateParameter("OutPieceSeq", dto.OutPieceSeq),  // Coil Sequence
                ctx.CreateParameter("InPieceNo", inPieceNo),
                ctx.CreateParameter("InPieceId", dto.InPieceId),     // Input Slab ID
                ctx.CreateParameter("JobId", dto.JobId),             // Sring No
                ctx.CreateParameter("ProductionStartDate", dto.ProductionStartDate),  // Start Production Time
                ctx.CreateParameter("ProductionStopDate", dto.ProductionStopDate),    // Stop production Time
                ctx.CreateParameter("MeasuredWeight", dto.MeasuredWeight),    //  Measured Weight
                ctx.CreateParameter("CalculatedWeight", dto.CalculatedWeight),  // Calculated Weight
                ctx.CreateParameter("Length", dto.Length),   // Exit Length
                ctx.CreateParameter("ExitThk", dto.ExitThk),    //  Exit Thickness
                ctx.CreateParameter("TargetWidth", dto.TargetWidth),  // Target Width
                ctx.CreateParameter("TargetThickness", dto.TargetThickness),   // Target Thickness
                ctx.CreateParameter("InnerDiameter", dto.InnerDiameter),   // Internal Diameter
                ctx.CreateParameter("OuterDiameter", dto.OuterDiameter),   // Outer Diameter
                ctx.CreateParameter("EndOfIngotFlag", dto.EndOfInGotFlag),  //  [TAB Disposition Data] EndOfInGotFlag
                ctx.CreateParameter("TestCut", dto.TestCut),            //  [TAB Disposition Data] Test Cut
                ctx.CreateParameter("TrialFlag", dto.TrialFlag),    //  Trial Coil
                ctx.CreateParameter("CrewId", dto.CrewId),          // Production Crew
                ctx.CreateParameter("ShiftId", dto.ShiftId),       // Production Shift
                ctx.CreateParameter("Status", dto.Status),          // Coil Status
                ctx.CreateParameter("Remark", dto.Remark),          // Note
                ctx.CreateParameter("SoakTime", dto.SoakTime),      // Soak Time
                ctx.CreateParameter("GapTime", dto.GapTime),        // Gap Time
                ctx.CreateParameter("RollingTime", dto.RollingTime),   // Rolling Time
                ctx.CreateParameter("StripCrownTolPerc", dto.StripCrownTolPerc),      // [TAB Measured Data] Strip Profile [mm] --> colonna In Tol[%]
                ctx.CreateParameter("StripCrownIntolFlag", dto.StripCrownIntolFlag),  // [TAB Measured Data] Strip Profile [mm] --> colonna In Tolerance
                ctx.CreateParameter("ExitThkTolPerc", dto.ExitThkTolPerc),            // [TAB Measured Data] Exit thickness [mm] --> colonna In Tol[%]
                ctx.CreateParameter("ExitThkIntolFlag", dto.ExitThkIntolFlag),        // [TAB Measured Data] Exit thickness [mm] --> colonna In Tolerance
                ctx.CreateParameter("ExitWidthTolPerc", dto.ExitWidthTolPerc),        // [TAB Measured Data] Exit Width [mm] --> colonna In Tol[%]
                ctx.CreateParameter("ExitWidthIntolFlag", dto.ExitWidthIntolFlag),    // [TAB Measured Data] Exit Width [mm] --> colonna In Tolerance
                ctx.CreateParameter("ExitTempTolPerc", dto.ExitTempTolPerc),          // [TAB Measured Data] Finishing Temperature [^C] --> colonna In Tol[%]        
                ctx.CreateParameter("ExitTempIntolFlag", dto.ExitTempIntolFlag),       // [TAB Measured Data] Finishing Temperature [^C] --> colonna In Tolerance
                ctx.CreateParameter("DowncoilTempTolPerc", dto.DowncoilTempTolPerc),     // [TAB Measured Data] Coiling Temperature [^C] --> colonna In Tol[%] 
                ctx.CreateParameter("DowncoilTempIntolFlag", dto.DowncoilTempIntolFlag),  // [TAB Measured Data] Coiling Temperature [^C] --> colonna In Tolerance
                ctx.CreateParameter("InterstandCoolingBitmask", dto.InterstandCoolingBitmask),   // [TAB Rolls Data]  Interstand Cooling bitmask
                ctx.CreateParameter("Operator", dto.Operator),                                  // Operator
                ctx.CreateParameter("Revision", DateTime.Now),
                ctx.CreateParameter("OutPieceArea", "FM"),   
                ctx.CreateParameter("DestCodeId", 0),
                ctx.CreateParameter("CombinationNo", "")
            );

            InsRepHmHold(dto.DispositionCodesForHold, outPieceNo, dto.Operator, DateTime.Now);
            return outPieceNo;
        }

        private int findInPieceNo (string inPieceId){
            string padded = inPieceId.PadRight(32, ' ');

            string query = "SELECT MAX(PIECE_NO) FROM HRM_INPUT_PIECE WHERE PIECE_ID = :Parameter";

            var result = ctx.GetEntity<int>(query, ctx.CreateParameter("Parameter", padded));

            return result;
        }

        public void Delete(long outPieceNo)
        {
            if (validate(outPieceNo))
            {
                string delete1 = "DELETE FROM REP_HM_HOLD WHERE OUT_PIECE_NO = :OutPieceNo ";
                string delete2 = "DELETE FROM REP_HM_PIECE WHERE OUT_PIECE_NO = :OutPieceNo ";

                ctx.ExecuteNonQuery(delete1, ctx.CreateParameter("OutPieceNo", outPieceNo));
                ctx.ExecuteNonQuery(delete2, ctx.CreateParameter("OutPieceNo", outPieceNo));
            }
            else
            {
                var badRequestMap = new Dictionary<string, BadRequest>();
                badRequestMap.Add(nameof(outPieceNo), new BadRequest(BadRequestType.NOT_EXISTS));
                throw new BadRequestException(badRequestMap);
            }
        }

        public Boolean validate(long outPieceNo)
        {
            string query = $@"SELECT OUT_PIECE_NO FROM REP_HM_PIECE WHERE OUT_PIECE_NO = {outPieceNo} ";

            var result = ctx.GetEntity<long>(query);

            if (result != 0) { return true; }
            else { return false; }
        }

        public ProducedCoilsDetailDto Read(long outPieceNo)
        {
            return GetCurrentOutputCoil(outPieceNo);
        }

        public bool Exists(long id)
        {
            try
            {
                Read(id);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public CurrentOutCoilMeasDto[] GetCurrentOutCoilMeas(long outPieceNo)
        {
            CurrentOutCoilMeasDto[] outCoilArray = new CurrentOutCoilMeasDto[4];
            if (outPieceNo != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    outCoilArray[i] = GetCurrentOutCoilMeasDetail(outPieceNo, i);
                }
            }
            return outCoilArray;
        }

        public CurrentOutCoilMeasDto GetCurrentOutCoilMeasDetail(long outPieceNo, int statType)
        {
            string query = $@"SELECT  A.EXIT_THK             AS ExitThk
                         , A.EXIT_WDT                        AS ExitWdt
                         , A.EXIT_STRIP_TEMP                 AS ExitStripTemp
                         , A.DOWNCOIL_TEMP                   AS DowncoilTemp
                         , A.PROFILE_THK                     AS ProfileThk
                         , A.PROFILE_WDT                     AS ProfileWdt
                         , A.PROFILE_SYM                     AS Profile
                         , A.PROFILE_ASYM                    AS ProfileAsym
                         , A.STRIP_EDGE_DROP                 AS StripEdgeDrop
                         , A.STRIP_QB_FLATNESS               AS StripQbFlatness
                         , A.INTERMEDIATE_TEMP               AS IntermediateTemp
                   FROM    REP_HM_DATA   A
                   WHERE   A.OUT_PIECE_NO = :OutPieceNo
                   AND     A.STAT_TYPE    = :StatType
                   AND     A.STEP_NO      = (SELECT COALESCE(MAX(STEP_NO), 1) FROM REP_HM_DATA WHERE OUT_PIECE_NO = :OutPieceNo) ";

            var result = ctx.GetEntity<CurrentOutCoilMeasDto>(query, ctx.CreateParameter("OutPieceNo", outPieceNo), 
                                                                     ctx.CreateParameter("StatType", statType));

            return result;

        }

        public void InsRepHmHold(Boolean[] dispositionCodesForHold, long outPieceNo, string oper, DateTime revision)
        {
            var descriptions = buildDescriptions();
            var reasonValuesNames = buildReasonValueNames();

            string insert = @"INSERT INTO REP_HM_HOLD
                  (
                      OUT_PIECE_NO
                     ,REASON_CNT
                     ,VALUE_SEQ
                     ,REASON_VALUE_NAME
                     ,DESCRIPTION
                     ,OPER_CANCEL_FLAG
                     ,OPERATOR
                     ,REVISION
                    ) VALUES (
                        :OutPieceNo, :ReasonCnt, :ValueSeq, :ReasonValueName, :Description, :OperCancelFlag, :Operator, :Revision
                    )";

            for (int i = 0; i < 30; i++)
            {
                ctx.ExecuteNonQuery(insert,
                    ctx.CreateParameter("OutPieceNo", outPieceNo),
                    ctx.CreateParameter("ReasonCnt", findMax("REASON_CNT", "REP_HM_HOLD") + 1),
                    ctx.CreateParameter("ValueSeq", i + 1),
                    ctx.CreateParameter("ReasonValueName", reasonValuesNames[i]),
                    ctx.CreateParameter("Description", descriptions[i]),
                    ctx.CreateParameter("OperCancelFlag", (dispositionCodesForHold[i] == true ? 1 : 0)),
                    ctx.CreateParameter("Operator", oper),
                    ctx.CreateParameter("Revision", revision)
                );
            }

        }

        private string[] buildDescriptions()
        {
            string[] descriptions = new string[30];

            descriptions[0] = "F1 BITE SPRAY PRESSURE WAS TOO LOW";
            descriptions[1] = "AIM THICKNESS WAS CHANGED";
            descriptions[2] = "COIL WAS COBBLED";
            descriptions[3] = "OPERATOR PRESSED THE COBBLE PREVENT BUTTON";
            descriptions[4] = "COILING TEMPERATURE IS OUTSIDE THE CUSTOMER TOLERANCE";
            descriptions[5] = "CASTER WEDGE NOT OK";
            descriptions[6] = "OPERATOR PRESSED DAMAGED TAIL BUTTON";
            descriptions[7] = "FOLDED HEAD";
            descriptions[8] = "FINISHING TEMPERATURE IS OUTSIDE THE CUSTOMER TOLERANCE";
            descriptions[9] = "PROFILE IS OUTSIDE CUSTOMER TOLERANCE";
            descriptions[10] = "STEEL GRADE WAS CHANGED";
            descriptions[11] = "COIL HEAD WAS TOO HOT";
            descriptions[12] = "DESCALE PRESSURE WAS TOO LOW";
            descriptions[13] = "HEAT RIDGE";
            descriptions[14] = "COILER PYROMETER IS NOT WORKING";
            descriptions[15] = "FINISH MILL PYROMETER IS NOT WORKING";
            descriptions[16] = "WIDTH GAUGE IS NOT WORKING";
            descriptions[17] = "XGAUGE IS NOT WORKING";
            descriptions[18] = "CUSTOMER ORDER NUMBER WAS CHANGED";
            descriptions[19] = "EDGE THICKNESS BELOW CUSTOMER MIN";
            descriptions[20] = "WIDTH CHANGE IN THE COIL";
            descriptions[21] = "COIL SLIPPED IN THE BITE OF F1 ON THREADING";
            descriptions[22] = "OPERATOR PRESSED THE SLIP PREVENT BUTTON";
            descriptions[23] = "AIM COILING TEMPERATURE WAS CHANGED";
            descriptions[24] = "SLAB WAS IN THE FURNACE TOO LONG";
            descriptions[25] = "WIDTH CHANGE IN THE COIL";
            descriptions[26] = "WIDTH DROPPED BELOW THE CUSTOMER MINIMUM";
            descriptions[27] = "COIL THICKNESS IS OUTSIDE CUSTOMER TOLERANCE";
            descriptions[28] = "COIL WEIGHT OUTSIDE CUSTOMER TOLERANCE";
            descriptions[29] = "WIDTH IS BECOMING CLOSE TO BEING OUT OF TOLERANCE";

            return descriptions;
        }

        private string[] buildReasonValueNames()
        {
            string[] reasonValuesNames = new string[30];

            reasonValuesNames[0] = "BS";
            reasonValuesNames[1] = "CA";
            reasonValuesNames[2] = "CO";
            reasonValuesNames[3] = "CP";
            reasonValuesNames[4] = "CT";
            reasonValuesNames[5] = "CW";
            reasonValuesNames[6] = "DT";
            reasonValuesNames[7] = "FH";
            reasonValuesNames[8] = "FT";
            reasonValuesNames[9] = "GA";
            reasonValuesNames[10] = "GC";
            reasonValuesNames[11] = "HH";
            reasonValuesNames[12] = "HP";
            reasonValuesNames[13] = "HR";
            reasonValuesNames[14] = "NC";
            reasonValuesNames[15] = "NF";
            reasonValuesNames[16] = "NW";
            reasonValuesNames[17] = "NX";
            reasonValuesNames[18] = "OC";
            reasonValuesNames[19] = "PR";
            reasonValuesNames[20] = "PW";
            reasonValuesNames[21] = "SG";
            reasonValuesNames[22] = "SP";
            reasonValuesNames[23] = "TC";
            reasonValuesNames[24] = "TF";
            reasonValuesNames[25] = "TW";
            reasonValuesNames[26] = "WD";
            reasonValuesNames[27] = "WI";
            reasonValuesNames[28] = "WT";
            reasonValuesNames[29] = "WW";

            return reasonValuesNames;
        }

        private long findMax(string field, string table)
        {
            string query = $@"SELECT MAX({field}) FROM {table} ";

            var result = ctx.GetEntity<int>(query, ctx.CreateParameter("Field", field), ctx.CreateParameter("Table", table));

            return result;
        }


    }
}
