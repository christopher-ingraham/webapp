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
    internal class UsedSetupRepo<TDataSource> : IUsedSetupRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public UsedSetupRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<UsedSetupListItemDto> SelUsedSetup(ListRequestDto<UsedSetupListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = $@"SELECT A.IN_PIECE_NO                                                          AS InPieceNo
                          , A.STEP_NO                                                                       AS StepNo
                          , A.PASS_NO                                                                       AS PassNo
                          , A.STAND_NO                                                                      AS StandNo
                          , TRIM(CASE WHEN (CENTER_ID = 'RM') THEN 
                            (SELECT VALUE_LABEL FROM AUX_VALUE WHERE VARIABLE_ID = 'STAND_NAME_RM' 
                            AND INTEGER_VALUE = A.STAND_NO)
                            ELSE (SELECT VALUE_LABEL FROM AUX_VALUE WHERE VARIABLE_ID = 'STAND_NAME_FM' 
                            AND INTEGER_VALUE = A.STAND_NO) END)                                            AS StandNoLabel
                          , A.ENABLED_STAND                                                                 AS EnabledStand
                          , CAST(A.ENABLED_STAND AS VARCHAR(10) ) || ': ' || RTRIM(C.VALUE_LABEL)           AS EnabledStandLabel
                          , A.ENTRY_THK                                                                     AS EntryThk
                          , A.EXIT_THK                                                                      AS ExitThk
                          , A.DRAFT                                                                         AS Draft
                          , A.REDUCTION                                                                     AS Reduction
                          , A.ENTRY_WIDTH                                                                   AS EntryWidth
                          , A.EXIT_WIDTH                                                                    AS ExitWidth
                          , A.EXIT_TEMP                                                                     AS ExitTemp
                          , ROUND(((A.ENTRY_TENSION * A.ENTRY_WIDTH * A.ENTRY_THK) / 1000),1)               AS EntryTension
                          , ROUND(((A.EXIT_TENSION  * A.EXIT_WIDTH * A.EXIT_THK ) / 1000),1)                AS ExitTension
                          , A.ENTRY_TENSION                                                                 AS SpecEntryTens
                          , A.EXIT_TENSION                                                                  AS SpecExitTens 
                          , A.THREADING_SPEED                                                               AS ThreadingSpeed
                          , A.H_FORCE                                                                       AS HForce
                          , A.MILL_SPEED                                                                    AS MillSpeed
                          , A.FORCE                                                                         AS Force
                          , A.WR_BEND                                                                       AS WrBend
                          , A.WR_SHIFT                                                                      AS WrShift
                          , A.STRIP_COOLING_FLOW                                                            AS StripCoolingFlow
                  FROM    REP_HM_SETUP_STEP A
                         ,AUX_VALUE C
                  WHERE A.AREA_ID       = 'HSM'
                  AND   A.ENABLED_STAND = C.INTEGER_VALUE
                  AND   C.VARIABLE_ID   = 'ENABLED_FLAG' ";


            string queryCount = $@"SELECT COUNT(A.IN_PIECE_NO) 
                                 FROM    REP_HM_SETUP_STEP A
                                ,AUX_VALUE C
                  WHERE A.AREA_ID       = 'HSM'
                  AND   A.ENABLED_STAND = C.INTEGER_VALUE
                  AND   C.VARIABLE_ID   = 'ENABLED_FLAG' 
                  AND 1=1 ";


            var searchInPieceNo = listRequest.Filter?.SearchInPieceNo;
            if (searchInPieceNo != null)
            {
                query += "AND (A.IN_PIECE_NO = :SearchInPieceNo) ";
                queryCount += "AND (A.IN_PIECE_NO = :SearchInPieceNo) ";
                queryParam.Add(ctx.CreateParameter("SearchInPieceNo", searchInPieceNo));
                queryCountParam.Add(ctx.CreateParameter("SearchInPieceNo", searchInPieceNo));
            }

            query += "ORDER BY A.CENTER_ID DESC, A.STEP_NO ASC ";

            var data = ctx.GetEntities<UsedSetupListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<UsedSetupListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}