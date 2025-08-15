using DA.DB.Utils;
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
    internal class ExitSaddlesRepo<TDataSource> : IExitSaddlesRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public ExitSaddlesRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<ExitSaddlesListItemDto> SelExitSaddlesMap(ListRequestDto<ExitSaddlesListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT A.POSITION_NO AS PositionNo
                    , RTRIM(CAST(A.POSITION_NO AS CHAR(8))) || ': ' || RTRIM(B.VALUE_LABEL) AS SaddleLabel
                    , A.PIECE_NO AS PieceNo
                    , A.JOB_ID AS JobId
                    , A.PIECE_ID AS OutPieceId
                    FROM RTDB_TRACK_INFO A
                    LEFT OUTER JOIN AUX_VALUE B
                    ON A.POSITION_NO = B.INTEGER_VALUE
                    AND B.VARIABLE_ID = 'EX_SADDLE_CODE'
                    WHERE A.TRACK_AREA_ID = 'EX_SADDLE_CODE'
                    AND A.POSITION_NO BETWEEN 60 AND 80 ";

            string queryCount = @"SELECT COUNT(A.POSITION_NO) 
                                FROM      RTDB_TRACK_INFO A
                  LEFT OUTER JOIN AUX_VALUE B
                  ON A.POSITION_NO = B.INTEGER_VALUE
                  AND B.VARIABLE_ID = 'EX_SADDLE_CODE'
                  WHERE     A.TRACK_AREA_ID = 'EXIT_SECTION'
                  AND       A.POSITION_NO BETWEEN 30 AND 70  
                  AND 1=1 ";

            query += "ORDER BY A.POSITION_NO ASC ";

            var data = ctx.GetEntities<ExitSaddlesListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<ExitSaddlesListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}