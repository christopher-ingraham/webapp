using DA.DB.Utils;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.Production
{
    internal class RmlCrewRepo<TDataSource> : IRmlCrewRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public RmlCrewRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<RmlCrewLookupDto> SelCrewForOutCoil(ListRequestDto<RmlCrewLookupDto> listRequest)
        {

            string query = @"SELECT    TRIM(CREW_ID) AS Display,
                                       TRIM(CREW_ID) AS Value
                            FROM  RML_CREW ";

            string queryCount = @"SELECT COUNT(CREW_ID)
                                FROM RML_CREW
                                WHERE 1=1 ";

            query += "ORDER BY  CREW_ID ";

            var data = ctx.GetEntities<RmlCrewLookupDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0))).ToArray();
            var total = ctx.GetEntity<long>(queryCount);

            return new ListResultDto<RmlCrewLookupDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}