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
    internal class PracticeReportRepo<TDataSource> : IPracticeReportRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public PracticeReportRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<PracticeReportListItemDto> SelPracticeData(ListRequestDto<PracticeReportListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT DISTINCT
                           TRIM(A.PRACTICE_ID)                                              AS PracticeId 
                           ,TRIM(A.CENTER_ID)                                               AS CenterId
                           ,A.MILL_MODE                                                     AS MillMode        
                           ,CAST(A.MILL_MODE AS VARCHAR(10)) ||': '|| TRIM(E.VALUE_LABEL)   AS MillModeLabel
                           ,TRIM(A.MATERIAL_GRADE_ID)                                       AS MaterialGradeId
                           ,B.GRADE_GROUP_ID                                                AS GradeGroupId
                           ,CAST(B.GRADE_GROUP_ID AS VARCHAR(10)) ||': '|| TRIM(B.GRADE_GROUP_LABEL) AS GradeGroupLabel
                           ,A.DENSITY                                                       AS Density
                           ,A.ENTRY_THK                                                     AS EntryThk
                           ,A.TARGET_THK                                                    AS TargetThk
                           ,A.TARGET_COLD_THK                                               AS TargetColdThk
                           ,A.ENTRY_WDT                                                     AS EntryWdt
                           ,A.TARGET_WDT                                                    AS TargetWdt
                           ,A.TARGET_COLD_WDT                                               AS TargetColdWdt
                           ,A.PIECE_LENGTH                                                  AS PieceLength
                           ,A.ENTRY_TEMP                                                    AS EntryTemp                         
                           ,TRIM(A.OPERATOR)                                                AS Operator
                           ,A.DISABLED_STAND_BITMASK                                        AS nDisabledStand
                           ,(SELECT MAX(REVISION) FROM TDB_HM_SETUP WHERE AREA_ID = A.AREA_ID AND PRACTICE_ID = A.PRACTICE_ID AND MILL_MODE = A.MILL_MODE AND DISABLED_STAND_BITMASK = A.DISABLED_STAND_BITMASK) AS Revision 
                           ,A.REVISION                                                      AS FilterDate
                  FROM      TDB_HM_SETUP        A
                           ,TDB_GRADE_GROUP     B
                           ,TDB_MATERIAL_GRADE  C
                           ,AUX_VALUE           E
                  WHERE     B.GRADE_GROUP_ID    = C.GRADE_GROUP_ID
                  AND       A.MATERIAL_GRADE_ID = C.MATERIAL_GRADE_ID
                  AND       A.MILL_MODE      = E.INTEGER_VALUE
                  AND       E.VARIABLE_ID       = 'MILL_MODE' ";


            string queryCount = @"SELECT COUNT(PRACTICE_ID)
                                 FROM      TDB_HM_SETUP        A
                           ,TDB_GRADE_GROUP     B
                           ,TDB_MATERIAL_GRADE  C
                           ,AUX_VALUE           E
                  WHERE     B.GRADE_GROUP_ID    = C.GRADE_GROUP_ID
                  AND       A.MATERIAL_GRADE_ID = C.MATERIAL_GRADE_ID
                  AND       A.MILL_MODE      = E.INTEGER_VALUE
                  AND       E.VARIABLE_ID       = 'MILL_MODE' " ;

            var searchRevisionDateFrom = listRequest.Filter?.SearchRevisionDateFrom;
            if (searchRevisionDateFrom != null)
            {
                query += "AND A.REVISION >= :SearchRevisionDateFrom ";
                queryCount += "AND A.REVISION >= :SearchRevisionDateFrom ";
                queryParam.Add(ctx.CreateParameter("SearchRevisionDateFrom", searchRevisionDateFrom, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchRevisionDateFrom", searchRevisionDateFrom, DbType.DateTimeOffset));
            }

            var searchRevisionDateTo = listRequest.Filter?.SearchRevisionDateTo;
            if (searchRevisionDateTo != null)
            {
                query += "AND A.REVISION <= :SearchRevisionDateTo ";
                queryCount += "AND A.REVISION <= :SearchRevisionDateTo ";
                queryParam.Add(ctx.CreateParameter("SearchRevisionDateTo", searchRevisionDateTo, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchRevisionDateTo", searchRevisionDateTo, DbType.DateTimeOffset));
            }

            var searchCenterId = listRequest.Filter?.SearchCenterId;
            if (searchCenterId != null)
            {
                query += "AND (A.CENTER_ID LIKE :SearchCenterId||'%') ";
                queryCount += "AND (A.CENTER_ID LIKE :SearchCenterId||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchCenterId", searchCenterId));
                queryCountParam.Add(ctx.CreateParameter("SearchCenterId", searchCenterId));
            }

            var searchPracticeId = listRequest.Filter?.SearchPracticeId;
            if (searchPracticeId != null)
            {
                query += "AND (A.PRACTICE_ID LIKE :SearchPracticeId||'%') ";
                queryCount += "AND (A.PRACTICE_ID LIKE :SearchPracticeId||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchPracticeId", searchPracticeId));
                queryCountParam.Add(ctx.CreateParameter("SearchPracticeId", searchPracticeId));
            }

            var searchMillMode = listRequest.Filter?.SearchMillMode;
            if (searchMillMode != null)
            {
                query += "AND (A.MILL_MODE = :SearchMillMode) ";
                queryCount += "AND (A.MILL_MODE = :SearchMillMode) ";
                queryParam.Add(ctx.CreateParameter("SearchMillMode", searchMillMode));
                queryCountParam.Add(ctx.CreateParameter("SearchMillMode", searchMillMode));
            }

            var searchMaterialGradeId = listRequest.Filter?.SearchMaterialGradeId;
            if (searchMaterialGradeId != null)
            {
                query += "AND (A.MATERIAL_GRADE_ID LIKE :SearchMaterialGradeId||'%') ";
                queryCount += "AND (A.MATERIAL_GRADE_ID LIKE :SearchMaterialGradeId||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchMaterialGradeId", searchMaterialGradeId));
                queryCountParam.Add(ctx.CreateParameter("SearchMaterialGradeId", searchMaterialGradeId));
            }

            var searchEntryThicknessFrom = listRequest.Filter?.SearchEntryThicknessFrom;
            if (searchEntryThicknessFrom != null)
            {
                query += "AND A.ENTRY_THK >= :SearchEntryThicknessFrom ";
                queryCount += "AND A.ENTRY_THK >= :SearchEntryThicknessFrom ";
                queryParam.Add(ctx.CreateParameter("SearchEntryThicknessFrom", searchEntryThicknessFrom));
                queryCountParam.Add(ctx.CreateParameter("SearchEntryThicknessFrom", searchEntryThicknessFrom));
            }

            var searchEntryThicknessTo = listRequest.Filter?.SearchEntryThicknessTo;
            if (searchEntryThicknessTo != null)
            {
                query += "AND A.ENTRY_THK <= :SearchEntryThicknessTo ";
                queryCount += "AND A.ENTRY_THK <= :SearchEntryThicknessTo ";
                queryParam.Add(ctx.CreateParameter("SearchEntryThicknessTo", searchEntryThicknessTo));
                queryCountParam.Add(ctx.CreateParameter("SearchEntryThicknessTo", searchEntryThicknessTo));
            }


            var data = ctx.GetEntities<PracticeReportListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());
            return new ListResultDto<PracticeReportListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}