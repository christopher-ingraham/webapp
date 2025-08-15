using DA.DB.Utils;
using DA.WI.NSGHSM.Core.Extensions;
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
    internal class RepHmPieceRepo<TDataSource> : IRepHmPieceRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public RepHmPieceRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<RepHmPieceListItemDto> ReadList(ListRequestDto<RepHmPieceListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();
            const int coilFinished = 85;
            const int coilPartialRejected = 111;

            string query = @"SELECT    
                            PDO.OUT_PIECE_NO                AS OutPieceNo
                        ,   TRIM(PDO.OUT_PIECE_ID)          AS ProducedCoilId
                        ,   TRIM(PDI.PIECE_ID)              AS InputCoilId
                        ,   PDI.HEAT_NO                     AS HeatNo
                        ,   TRIM(ORD.ORDER_NUMBER)          AS CustomerOrderNo
                        ,   TRIM(ORD.ORDER_POSITION)        AS CustomerLineNo
                        ,   PDO.PRODUCTION_STOP_DATE        AS ProducionStopDate 
                        ,   TRIM(PDI.MATERIAL_GRADE_ID)     AS SteelGradeId
                        ,   PDI.MILL_MODE                   AS MillMode
                        ,   PDO.EXIT_WIDTH                  AS Width
                        ,   PDO.EXIT_THK                    AS Thickness
                        ,   PDO.MEASURED_WEIGHT             AS Weight   
                        ,   PDO.OUTER_DIAMETER              AS ExternalDiameter   
                  FROM      REP_HM_PIECE PDO
                  LEFT JOIN HRM_INPUT_PIECE PDI
                         ON PDI.PIECE_NO = PDO.IN_PIECE_NO
                  LEFT JOIN HRM_ORDER ORD
                         ON ORD.PIECE_NO = PDO.IN_PIECE_NO ";

            string queryCount = @"SELECT COUNT(OUT_PIECE_ID)
                                FROM      REP_HM_PIECE PDO
                  LEFT JOIN HRM_INPUT_PIECE PDI
                         ON PDI.PIECE_NO = PDO.IN_PIECE_NO
                  LEFT JOIN HRM_ORDER ORD
                         ON ORD.PIECE_NO = PDO.IN_PIECE_NO ";
            
            query += "WHERE PDO.STATUS BETWEEN :Status0 AND :Status1 ";
            queryCount += "WHERE PDO.STATUS BETWEEN :Status0 AND :Status1 ";
            queryParam.Add(ctx.CreateParameter("Status0", coilFinished));
            queryParam.Add(ctx.CreateParameter("Status1", coilPartialRejected));
            queryCountParam.Add(ctx.CreateParameter("Status0", coilFinished));
            queryCountParam.Add(ctx.CreateParameter("Status1", coilPartialRejected));

            var searchProducedCoil = listRequest.Filter?.SearchProducedCoil;
            if (searchProducedCoil.IsNullOrEmpty() == false)
            {
                query += "AND (OUT_PIECE_ID LIKE :SearchProducedCoil||'%') ";
                queryCount += "AND (OUT_PIECE_ID LIKE :SearchProducedCoil||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchProducedCoil", searchProducedCoil));
                queryCountParam.Add(ctx.CreateParameter("SearchProducedCoil", searchProducedCoil));
            }

            var searchInputCoil = listRequest.Filter?.SearchInputCoil;
            if (searchInputCoil.IsNullOrEmpty() == false)
            {
                query += "AND (IN_PIECE_ID LIKE :SearchInputCoil||'%') ";
                queryCount += "AND (IN_PIECE_ID LIKE :SearchInputCoil||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchInputCoil", searchInputCoil));
                queryCountParam.Add(ctx.CreateParameter("SearchInputCoil", searchInputCoil));
            }

            var searchDataFrom = listRequest.Filter?.SearchDataFrom;
            if (searchDataFrom != null)
            {
                query += "AND PRODUCTION_STOP_DATE >= :SearchDataFrom ";
                queryCount += "AND PRODUCTION_STOP_DATE >=:SearchDataFrom ";
                queryParam.Add(ctx.CreateParameter("SearchDataFrom", searchDataFrom, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchDataFrom", searchDataFrom, DbType.DateTimeOffset));
            }

            var searchDataTo = listRequest.Filter?.SearchDataTo;
            if (searchDataTo!= null)
            {
                query += "AND PRODUCTION_STOP_DATE <= :SearchDataTo ";
                queryCount += "AND PRODUCTION_STOP_DATE <= :SearchDataTo ";
                queryParam.Add(ctx.CreateParameter("SearchDataTo", searchDataTo, DbType.DateTimeOffset));
                queryCountParam.Add(ctx.CreateParameter("SearchDataTo", searchDataTo, DbType.DateTimeOffset));
            }

            query += "ORDER BY PRODUCTION_STOP_DATE DESC";

            var data = ctx.GetEntities<RepHmPieceListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<RepHmPieceListItemDto>
            {
                Data = data,
                Total = total
            };
        }

    }

}