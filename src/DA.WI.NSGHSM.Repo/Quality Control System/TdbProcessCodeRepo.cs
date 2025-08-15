using DA.DB.Utils;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.QualityControlSystem
{
    internal class TdbProcessCodeRepo<TDataSource> : ITdbProcessCodeRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public TdbProcessCodeRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<TdbProcessCodeListItemDto> FillProcessCodesByCodeType(ListRequestDto<TdbProcessCodeListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT RTRIM(CAST(PROCESS_ID AS CHAR(10))) || ': ' || RTRIM(PROCESS_DESCR)    AS ProcessCodeLabel
                           , CODE_ID                                                                       AS CodeId
                           FROM  TDB_PROCESS_CODE ";


            string queryCount = @"SELECT COUNT(CODE_ID)
                                FROM  TDB_PROCESS_CODE " ;

                                
            var codeType = listRequest.Filter?.CodeType;
            if (codeType != null)
            {
                query += "WHERE CODE_TYPE LIKE :CodeType||'%' ";
                queryCount += "WHERE CODE_TYPE LIKE :CodeType||'%' ";
                queryParam.Add(ctx.CreateParameter("CodeType", codeType));
                queryCountParam.Add(ctx.CreateParameter("CodeType", codeType));
            }

            query += "ORDER BY  CODE_ID ";

            var data = ctx.GetEntities<TdbProcessCodeListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<TdbProcessCodeListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}