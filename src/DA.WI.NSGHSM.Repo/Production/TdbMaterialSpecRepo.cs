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
    internal class TdbMaterialSpecRepo<TDataSource> : ITdbMaterialSpecRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public TdbMaterialSpecRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<TdbMaterialSpecLookupDto> SelMaterialSpecList(ListRequestDto<TdbMaterialSpecListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT 
                            TRIM(MATERIAL_SPEC_ID)      AS Display,
		                    MATERIAL_SPEC_CODE          AS Value
		                    FROM
		                    TDB_MATERIAL_SPEC ";


            string queryCount = @"SELECT COUNT(MATERIAL_SPEC_ID)
                                FROM  TDB_MATERIAL_SPEC ";

            query += "ORDER BY MATERIAL_SPEC_CODE ";

            var data = ctx.GetEntities<TdbMaterialSpecLookupDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<TdbMaterialSpecLookupDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}