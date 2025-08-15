using DA.DB.Utils;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Report;
using DA.WI.NSGHSM.IRepo.Report;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.Report
{
    internal class CoilGeneralReportRepo<TDataSource> : ICoilGeneralReportRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public CoilGeneralReportRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<CoilGeneralReportListItemDto> SelCoilData(ListRequestDto<CoilGeneralReportListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT     A.OUT_PIECE_NO          AS OutPieceNo                                         
                              ,A.IN_PIECE_NO                    AS InPieceNo
                              ,TRIM(A.OUT_PIECE_ID)             AS OutPieceId
                              ,TRIM(I.JOB_ID)                   AS JobId
                              ,TRIM(A.IN_PIECE_ID)              AS InPieceId
                              ,A.OUT_PIECE_SEQ                  AS ProdCoilNum
                              ,TRIM(H.HEAT_ID)                  AS HeatId
                              ,TRIM(I.MATERIAL_GRADE_ID)        AS MaterialGradeId 
                              ,CASE WHEN (O.TRIAL_FLAG > 0) THEN 'Y' ELSE 'N' END AS TrialFlag 
                              ,S1.TARGET_COLD_WDT               AS TargetColdWidth
                              ,A.EXIT_WIDTH                     AS ExitWidth
                              ,S1.TARGET_COLD_THK               AS TargetColdThickness
                              ,A.EXIT_THK                       AS ExitThk
                              ,A.CALCULATED_WEIGHT          AS CalculatedWeight
                              ,A.MEASURED_WEIGHT            AS MeasuredWeight 
                              ,A.OUTER_DIAMETER             AS ExternalDiameter
                              ,A.LENGTH                     AS Length
                              ,TRIM(O.CUSTOMER_ID)          AS CustomerId
                              ,A.PRODUCTION_STOP_DATE       AS ProductionStopDate      
                              ,A.SHIFT_ID                   AS ShiftId
                              ,I.THICKNESS                  AS EntryThk
                              ,A.TARGET_THICKNESS           AS TargetThickness
                              ,A.TARGET_WIDTH               AS NominalWidth
                              ,A.MEASURED_WEIGHT            AS Weight
                              ,A.PRODUCTION_START_DATE      AS ProductionStartDate
                              ,A.PRODUCTION_STOP_DATE       AS FilterDate 
                              ,A.STATUS                     AS Status
                              ,LTRIM(CAST(A.SHIFT_ID AS VARCHAR(10)))   || ': ' || RTRIM(C.VALUE_LABEL)       AS ProductionShiftLabel 
                              ,TRIM(A.CREW_ID)                    AS CrewId
                    FROM REP_HM_PIECE        A  
                    LEFT OUTER JOIN REP_HM_SETUP    S1 ON S1.IN_PIECE_NO = A.IN_PIECE_NO AND S1.AREA_ID = 'HSM' AND S1.CENTER_ID = 'FM'
                    LEFT OUTER JOIN HRM_INPUT_PIECE  I ON A.IN_PIECE_NO = I.PIECE_NO
                    LEFT OUTER JOIN HRM_HEAT         H ON  I.HEAT_NO  = H.HEAT_NO
                    LEFT OUTER JOIN HRM_ORDER        O ON O.PIECE_NO = A.IN_PIECE_NO 
                                ,AUX_VALUE           C
                    WHERE  (A.STATUS BETWEEN 50 AND 100)
                      AND  C.VARIABLE_ID        = 'PRODUCTION_SHIFT' 
                      AND  C.INTEGER_VALUE      = A.SHIFT_ID  ";


            string queryCount = @"SELECT COUNT(A.OUT_PIECE_NO)
                                FROM REP_HM_PIECE        A  
                    LEFT OUTER JOIN REP_HM_SETUP    S1 ON S1.IN_PIECE_NO = A.IN_PIECE_NO AND S1.AREA_ID = 'HSM' AND S1.CENTER_ID = 'FM'
                    LEFT OUTER JOIN HRM_INPUT_PIECE  I ON A.IN_PIECE_NO = I.PIECE_NO
                    LEFT OUTER JOIN HRM_HEAT         H ON  I.HEAT_NO  = H.HEAT_NO
                    LEFT OUTER JOIN HRM_ORDER        O ON O.PIECE_NO = A.IN_PIECE_NO 
                                ,AUX_VALUE           C
                    WHERE  (A.STATUS BETWEEN 50 AND 100)
                      AND  C.VARIABLE_ID        = 'PRODUCTION_SHIFT' 
                      AND  C.INTEGER_VALUE      = A.SHIFT_ID  " ;

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
                query += "AND (A.IN_PIECE_NO = :SearchInputSlabNumber) ";
                queryCount += "AND (A.IN_PIECE_NO = :SearchInputSlabNumber) ";
                queryParam.Add(ctx.CreateParameter("SearchInputSlabNumber", searchInputSlabNumber));
                queryCountParam.Add(ctx.CreateParameter("SearchInputSlabNumber", searchInputSlabNumber));
            }

            var searchHeatNumber = listRequest.Filter?.SearchHeatNumber;
            if (searchHeatNumber != null)
            {
                query += "AND (H.HEAT_ID LIKE :SearchHeatNumber||'%') ";
                queryCount += "AND (H.HEAT_ID = :SearchHeatNumber||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchHeatNumber", searchHeatNumber));
                queryCountParam.Add(ctx.CreateParameter("SearchHeatNumber", searchHeatNumber));
            }

            var searchMaterialGradeId = listRequest.Filter?.SearchMaterialGradeId;
            if (searchMaterialGradeId != null)
            {
                query += "AND (I.MATERIAL_GRADE_ID LIKE :SearchMaterialGradeId||'%') ";
                queryCount += "AND (I.MATERIAL_GRADE_ID LIKE :SearchMaterialGradeId||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchMaterialGradeId", searchMaterialGradeId));
                queryCountParam.Add(ctx.CreateParameter("SearchMaterialGradeId", searchMaterialGradeId));
            }

            var searchCoilStatus = listRequest.Filter?.SearchCoilStatus;
            if (searchCoilStatus != null)
            {
                query += "AND (A.STATUS = :SearchCoilStatus) ";
                queryCount += "AND (A.STATUS = :SearchCoilStatus) ";
                queryParam.Add(ctx.CreateParameter("SearchCoilStatus", searchCoilStatus));
                queryCountParam.Add(ctx.CreateParameter("SearchCoilStatus", searchCoilStatus));
            }

            var searchShiftId = listRequest.Filter?.SearchShiftId;
            if (searchShiftId != null)
            {
                query += "AND (A.SHIFT_ID = :SearchShiftId) ";
                queryCount += "AND (A.SHIFT_ID = :SearchShiftId) ";
                queryParam.Add(ctx.CreateParameter("SearchShiftId", searchShiftId));
                queryCountParam.Add(ctx.CreateParameter("SearchShiftId", searchShiftId));
            }

            var searchExitThicknessFrom = listRequest.Filter?.SearchExitThicknessFrom;
            if (searchExitThicknessFrom != null)
            {
                query += "AND A.EXIT_THK >= :SearchExitThicknessFrom ";
                queryCount += "AND A.EXIT_THK >= :SearchExitThicknessFrom ";
                queryParam.Add(ctx.CreateParameter("SearchExitThicknessFrom", searchExitThicknessFrom));
                queryCountParam.Add(ctx.CreateParameter("SearchExitThicknessFrom", searchExitThicknessFrom));
            }

            var searchExitThicknessTo = listRequest.Filter?.SearchExitThicknessTo;
            if (searchExitThicknessTo != null)
            {
                query += "AND A.EXIT_THK <= :SearchExitThicknessTo ";
                queryCount += "AND A.EXIT_THK <= :SearchExitThicknessTo ";
                queryParam.Add(ctx.CreateParameter("SearchExitThicknessTo", searchExitThicknessTo));
                queryCountParam.Add(ctx.CreateParameter("SearchExitThicknessTo", searchExitThicknessTo));
            }

            var searchExitWidthFrom = listRequest.Filter?.SearchExitWidthFrom;
            if (searchExitWidthFrom != null)
            {
                query += "AND A.EXIT_WIDTH >= :SearchExitWidthFrom ";
                queryCount += "AND A.EXIT_WIDTH >= :SearchExitWidthFrom ";
                queryParam.Add(ctx.CreateParameter("SearchExitWidthFrom", searchExitWidthFrom));
                queryCountParam.Add(ctx.CreateParameter("SearchExitWidthFrom", searchExitWidthFrom));
            }

            var searchExitWidthTo = listRequest.Filter?.SearchExitWidthTo;
            if (searchExitWidthTo != null)
            {
                query += "AND A.EXIT_WIDTH <= :SearchExitWidthTo ";
                queryCount += "AND A.EXIT_WIDTH <= :SearchExitWidthTo ";
                queryParam.Add(ctx.CreateParameter("SearchExitWidthTo", searchExitWidthTo));
                queryCountParam.Add(ctx.CreateParameter("SearchExitWidthTo", searchExitWidthTo));
            }


            query += "ORDER BY  A.PRODUCTION_STOP_DATE DESC ";

            var data = ctx.GetEntities<CoilGeneralReportListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<CoilGeneralReportListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}