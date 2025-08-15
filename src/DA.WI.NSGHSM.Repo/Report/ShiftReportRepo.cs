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
    internal class ShiftReportRepo<TDataSource> : IShiftReportRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public ShiftReportRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<ShiftReportListItemDto> SelShiftSummary(ListRequestDto<ShiftReportListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT  SHIFT_CNT                              AS ShiftCnt
                             ,SHIFT_START_DATE                              AS ShiftStartDate
                             ,SHIFT_END_DATE                                AS ShiftEndDate
                             ,SHIFT_START_DATE                              AS FilterDate
                             ,SHIFT_ID                                      AS ShiftId
                             ,(LTRIM (CAST (SHIFT_ID AS VARCHAR (10))) || ': ') || RTRIM (VALUE_LABEL) AS ProductionShiftLabel
                             ,TRIM(CREW_ID)                                 AS CrewId
                             ,CURRENT_MONTH_START                           AS CurrentMonthStart
                             ,CURRENT_MONTH_STOP                            AS CurrentMonthStop
                             ,MTD_PRODUCED                                  AS MtdProduced
                             ,TOT_OUT_WEIGHT                                AS TotOutWeight
                             ,TOT_IN_WEIGHT                                 AS TotInWeight
                             ,OPT_WEIGHT                                    AS OptWeight
                             ,UTILIZATION                                   AS Utilization
                             ,LAST_PIECE_NO                                 AS LastPieceNo
                             ,TOT_OUT_PIECES_NUM                            AS TotOutPiecesNum
                      FROM    REP_SHIFT_SUMMARY A,
                              AUX_VALUE C   
                      WHERE VARIABLE_ID = 'PRODUCTION_SHIFT'
                        AND (SHIFT_ID   = INTEGER_VALUE) ";


            string queryCount = @"SELECT COUNT(SHIFT_CNT)
                                FROM    REP_SHIFT_SUMMARY A,
                              AUX_VALUE C   
                      WHERE VARIABLE_ID = 'PRODUCTION_SHIFT'
                        AND (SHIFT_ID   = INTEGER_VALUE) " ;

            var searchShiftStartDateFrom = listRequest.Filter?.SearchShiftStartDateFrom;
            if (searchShiftStartDateFrom != null)
            {
                query += "AND SHIFT_START_DATE >= :SearchShiftStartDateFrom ";
                queryCount += "AND SHIFT_START_DATE >= :SearchShiftStartDateFrom ";
                queryParam.Add(ctx.CreateParameter("SearchShiftStartDateFrom", searchShiftStartDateFrom, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchShiftStartDateFrom", searchShiftStartDateFrom, DbType.DateTimeOffset));
            }

            var searchShiftStartDateTo = listRequest.Filter?.SearchShiftStartDateTo;
            if (searchShiftStartDateTo != null)
            {
                query += "AND SHIFT_START_DATE <= :SearchShiftStartDateTo ";
                queryCount += "AND SHIFT_START_DATE <= :SearchShiftStartDateTo ";
                queryParam.Add(ctx.CreateParameter("SearchShiftStartDateTo", searchShiftStartDateTo, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchShiftStartDateTo", searchShiftStartDateTo, DbType.DateTimeOffset));
            }

            var searchShiftId = listRequest.Filter?.SearchShiftId;
            if (searchShiftId != null)
            {
                query += "AND (SHIFT_ID = :SearchShiftId) ";
                queryCount += "AND (SHIFT_ID = :SearchShiftId) ";
                queryParam.Add(ctx.CreateParameter("SearchShiftId", searchShiftId));
                queryCountParam.Add(ctx.CreateParameter("SearchShiftId", searchShiftId));
            }

            query += "ORDER BY SHIFT_END_DATE ";

            var data = ctx.GetEntities<ShiftReportListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());
            return new ListResultDto<ShiftReportListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}