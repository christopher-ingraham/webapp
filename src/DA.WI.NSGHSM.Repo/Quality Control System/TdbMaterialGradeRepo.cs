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
    internal class TdbMaterialGradeRepo<TDataSource> : ITdbMaterialGradeRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public TdbMaterialGradeRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<TdbMaterialGradeListItemDto> SelMatGradeForInPiece(ListRequestDto<TdbMaterialGradeListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT  TRIM(A.MATERIAL_GRADE_ID)      AS MaterialGradeId
                                    ,A.ALLOY_CODE_CORE              AS AlloyCodeCore
                            FROM TDB_MATERIAL_GRADE  A   LEFT OUTER JOIN
                            TDB_GRADE_GROUP     B   ON  B.GRADE_GROUP_ID  = A.GRADE_GROUP_ID
                            AND B.GROUP_TYPE      = 0 ";


            string queryCount = @"SELECT COUNT(A.MATERIAL_GRADE_ID)
                                FROM      TDB_MATERIAL_GRADE  A   LEFT OUTER JOIN
                                TDB_GRADE_GROUP     B   ON  B.GRADE_GROUP_ID  = A.GRADE_GROUP_ID
                                AND B.GROUP_TYPE      = 0 ";

            query += "ORDER BY MATERIAL_GRADE_ID ";

            var data = ctx.GetEntities<TdbMaterialGradeListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<TdbMaterialGradeListItemDto>
            {
                Data = data,
                Total = total
            };
        }

        public ListResultDto<TdbMaterialGradeLookupDto> FillMaterialGrade(ListRequestDto<TdbMaterialGradeLookupDto> listRequest)
        {

            string query = @"SELECT  TRIM(MATERIAL_GRADE_ID)    AS Display,
                                     TRIM(MATERIAL_GRADE_ID)    AS Value
                  FROM      TDB_MATERIAL_GRADE ";

            string queryCount = @"SELECT COUNT(MATERIAL_GRADE_ID)
                                FROM TDB_MATERIAL_GRADE
                                WHERE 1=1 ";

            query += "ORDER BY  MATERIAL_GRADE_ID ";

            var data = ctx.GetEntities<TdbMaterialGradeLookupDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0))).ToArray();
            var total = ctx.GetEntity<long>(queryCount);

            return new ListResultDto<TdbMaterialGradeLookupDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}