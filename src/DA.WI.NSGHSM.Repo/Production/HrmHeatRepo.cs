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
    internal class HrmHeatRepo<TDataSource> : IHrmHeatRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public HrmHeatRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<HrmHeatListItemDto> FillAllHeatIdList(ListRequestDto<HrmHeatListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT HEAT_NO   AS HeatNo,
                         TRIM(HEAT_ID)        AS HeatId
                        FROM  HRM_HEAT ";


            string queryCount = @"SELECT COUNT(HEAT_ID)
                                FROM  HRM_HEAT ";

            var searchJobId = listRequest.Filter?.SearchJobId;
            if (searchJobId != null)
            {
                query += "WHERE JOB_ID LIKE :SearchJobId||'%' ";
                queryCount += "WHERE JOB_ID LIKE :SearchJobId||'%' ";
                queryParam.Add(ctx.CreateParameter("SearchJobId", searchJobId));
                queryCountParam.Add(ctx.CreateParameter("SearchJobId", searchJobId));
            }

            query += "ORDER BY HEAT_NO ";

            var data = ctx.GetEntities<HrmHeatListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<HrmHeatListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}