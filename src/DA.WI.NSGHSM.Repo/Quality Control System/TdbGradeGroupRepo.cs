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
    internal class TdbGradeGroupRepo<TDataSource> : ITdbGradeGroupRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public TdbGradeGroupRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<TdbGradeGroupListItemDto> FillGradeGroup(ListRequestDto<TdbGradeGroupListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT
                            TRIM(GRADE_GROUP_ID) || ': ' || TRIM(MATERIAL_GROUP)    AS GradeGroupLabel
                          , GRADE_GROUP_ID                                          AS GradeGroupId
                            FROM  TDB_GRADE_GROUP ";


            string queryCount = @"SELECT COUNT(GRADE_GROUP_ID)
                                FROM  TDB_GRADE_GROUP ";

            query += "ORDER BY  GRADE_GROUP_ID ";

            var data = ctx.GetEntities<TdbGradeGroupListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<TdbGradeGroupListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}