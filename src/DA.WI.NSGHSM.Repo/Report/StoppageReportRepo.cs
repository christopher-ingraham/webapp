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
    internal class StoppageReportRepo<TDataSource> : IStoppageReportRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public StoppageReportRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<StoppageReportListItemDto> SelMainStoppageData(ListRequestDto<StoppageReportListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT  D.START_DELAY                                                                        AS StartDelay
                         ,D.END_DELAY                                                                                     AS FilterDate
                         ,D.STOPPAGE_CNT                                                                                  AS StpCounter
                         ,(ROUND(D.DURATION / 60.0, 0) || ':' || (D.DURATION - (60*ROUND(D.DURATION / 60.0, 0))))         AS Duration_HHMM
                         ,CAST(A.INTEGER_VALUE AS VARCHAR(10)) ||': '|| RTRIM(A.VALUE_LABEL)                              AS DelayTime
                         ,CAST(C.REASON_GROUP_ID AS VARCHAR(10)) ||': '|| TRIM(C.DESCRIPTION)                             AS StpGroupLabel
                         ,CAST(B.REASON_ID AS VARCHAR(10)) ||': '|| TRIM(B.DESCRIPTION)                                   AS StpReasonLabel
                         ,D.SHIFT_ID                                                                                      AS ShiftId
                         ,LTRIM(CAST(D.SHIFT_ID AS VARCHAR(10))) || ': ' || RTRIM(E.VALUE_LABEL)                          AS ProductionShiftLabel
                         ,TRIM(D.CREW_ID)                                                                                 AS CrewId
                  FROM    STP_STOPPAGE      D
                         ,STP_REASON        B
                         ,STP_REASON_GROUP  C
                         ,AUX_VALUE         A
                         ,AUX_VALUE         E
                  WHERE   B.REASON_ID       = D.REASON_ID
                  AND     C.REASON_GROUP_ID = D.REASON_GROUP_ID
                  AND     A.INTEGER_VALUE   = D.DELAY_TYPE
                  AND     A.VARIABLE_ID     = 'DELAY_TYPE'
                  AND     E.INTEGER_VALUE   = D.SHIFT_ID
                  AND     E.VARIABLE_ID     = 'PRODUCTION_SHIFT'
                  AND     D.STOP_STATUS     < 9 ";


            string queryCount = @"SELECT COUNT(STOPPAGE_CNT)
                                 FROM    STP_STOPPAGE      D
                         ,STP_REASON        B
                         ,STP_REASON_GROUP  C
                         ,AUX_VALUE         A
                         ,AUX_VALUE         E
                  WHERE   B.REASON_ID       = D.REASON_ID
                  AND     C.REASON_GROUP_ID = D.REASON_GROUP_ID
                  AND     A.INTEGER_VALUE   = D.DELAY_TYPE
                  AND     A.VARIABLE_ID     = 'DELAY_TYPE'
                  AND     E.INTEGER_VALUE   = D.SHIFT_ID
                  AND     E.VARIABLE_ID     = 'PRODUCTION_SHIFT'
                  AND     D.STOP_STATUS     < 9 " ;

            var searchEndDelayDateFrom = listRequest.Filter?.SearchEndDelayDateFrom;
            if (searchEndDelayDateFrom != null)
            {
                query += "AND D.END_DELAY >= :SearchEndDelayDateFrom ";
                queryCount += "AND D.END_DELAY >= :SearchEndDelayDateFrom ";
                queryParam.Add(ctx.CreateParameter("SearchEndDelayDateFrom", searchEndDelayDateFrom, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchEndDelayDateFrom", searchEndDelayDateFrom, DbType.DateTimeOffset));
            }

            var searchEndDelayDateTo = listRequest.Filter?.SearchEndDelayDateTo;
            if (searchEndDelayDateTo != null)
            {
                query += "AND D.END_DELAY <= :SearchEndDelayDateTo ";
                queryCount += "AND D.END_DELAY <= :SearchEndDelayDateTo ";
                queryParam.Add(ctx.CreateParameter("SearchEndDelayDateTo", searchEndDelayDateTo, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchEndDelayDateTo", searchEndDelayDateTo, DbType.DateTimeOffset));
            }

            var searchShiftId = listRequest.Filter?.SearchShiftId;
            if (searchShiftId != null)
            {
                query += "AND (D.SHIFT_ID = :SearchShiftId) ";
                queryCount += "AND (D.SHIFT_ID = :SearchShiftId) ";
                queryParam.Add(ctx.CreateParameter("SearchShiftId", searchShiftId));
                queryCountParam.Add(ctx.CreateParameter("SearchShiftId", searchShiftId));
            }


            query += "ORDER BY D.END_DELAY DESC ";

            var data = ctx.GetEntities<StoppageReportListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<StoppageReportListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}