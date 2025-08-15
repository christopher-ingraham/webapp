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
    internal class RepHmPieceStatRepo<TDataSource> : IRepHmPieceStatRepo<TDataSource>
         where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public RepHmPieceStatRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<RepHmPieceStatListItemDto> ReadList(ListRequestDto<RepHmPieceStatListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();
            string sampleData = "";
            var chartFlag = listRequest.Filter?.ChartData;

            if (chartFlag == 1)
                sampleData += ",SAMPLE_DATA AS SampleData ";


            string query = $@"SELECT 
                            ,TRIM(SAMPLE_ID)                 AS SignalID
                            ,TRIM(B.DESCRIPTION)             AS Description
                            ,TRIM(B.UNIT)                    AS MeasUnit
                            ,OUT_PIECE_NO                    AS OutPieceNo
                            ,TRIM(OUT_PIECE_AREA)            AS OutPieceArea
                            ,PASS_NO                         AS PassNo 
                            {sampleData} 
                            FROM REP_HM_PIECE_STAT A JOIN AUX_VALUE B
                            ON B.VARIABLE_ID = 'SAMPLE_NAME' AND B.VALUE_NAME = A.SAMPLE_ID WHERE 1 = 1  ";

            string queryCount = "SELECT COUNT(SAMPLE_ID) " +
                                "FROM REP_HM_PIECE_STAT JOIN AUX_VALUE " +
                                "ON 1 = 1 AND AUX_VALUE.VARIABLE_ID = 'SAMPLE_NAME' AND AUX_VALUE.VALUE_NAME = REP_HM_PIECE_STAT.SAMPLE_ID ";


            var outPieceNoEq = listRequest.Filter?.OutPieceNoEq;
            if (outPieceNoEq != null)
            {
                query += "AND (OUT_PIECE_NO LIKE :OutPieceNoEq||'%')";
                queryCount += "AND (OUT_PIECE_NO LIKE :OutPieceNoEq||'%')";
                queryParam.Add(ctx.CreateParameter("OutPieceNoEq", outPieceNoEq));
                queryCountParam.Add(ctx.CreateParameter("OutPieceNoEq", outPieceNoEq));
            }

            var outPieceNoMoreThan = listRequest.Filter?.OutPieceNoMoreThan;
            if (outPieceNoMoreThan != null)
            {
                query += "AND (OUT_PIECE_NO >= :MoreThan)";
                queryCount += "AND (OUT_PIECE_NO >= :MoreThan)";
                queryParam.Add(ctx.CreateParameter("MoreThan", outPieceNoMoreThan));
                queryCountParam.Add(ctx.CreateParameter("MoreThan", outPieceNoMoreThan));
            }

            var outPieceNoLessThan = listRequest.Filter?.OutPieceNoLessThan;
            if (outPieceNoLessThan != null)
            {
                query += "AND (OUT_PIECE_NO <= :LessThan)";
                queryCount += "AND (OUT_PIECE_NO <= :LessThan)";
                queryParam.Add(ctx.CreateParameter("LessThan", outPieceNoLessThan));
                queryCountParam.Add(ctx.CreateParameter("LessThan", outPieceNoLessThan));
            }

            var outPieceAreaEq = listRequest.Filter?.OutPieceAreaEq;
            if (outPieceAreaEq.IsNullOrEmpty() == false)
            {
                query += "AND (OUT_PIECE_AREA LIKE :OutPieceAreaEq||'%')";
                queryCount += "AND (OUT_PIECE_AREA LIKE :OutPieceAreaEq||'%')";
                queryParam.Add(ctx.CreateParameter("OutPieceAreaEq", outPieceAreaEq));
                queryCountParam.Add(ctx.CreateParameter("OutPieceAreaEq", outPieceAreaEq));
            }

            var sampleIdEq = listRequest.Filter?.SampleIdEq;
            if (sampleIdEq.IsNullOrEmpty() == false)
            {
                query += "AND (SAMPLE_ID LIKE :SampleIdEq||'%')";
                queryCount += "AND (SAMPLE_ID LIKE :SampleIdEq||'%')";
                queryParam.Add(ctx.CreateParameter("SampleIdEq", sampleIdEq));
                queryCountParam.Add(ctx.CreateParameter("SampleIdEq", sampleIdEq));
            }

            var sampleIdNe = listRequest.Filter?.SampleIdNe;
            if (sampleIdNe.IsNullOrEmpty() == false)
            {
                query += "AND (SAMPLE_ID NOT LIKE :SampleIdNe||'%')";
                queryCount += "AND (SAMPLE_ID NOT LIKE :SampleIdNe||'%')";
                queryParam.Add(ctx.CreateParameter("SampleIdNe", sampleIdNe));
                queryCountParam.Add(ctx.CreateParameter("SampleIdNe", sampleIdNe));
            }

            var passNoEq = listRequest.Filter?.PassNoEq;
            if (passNoEq != null)
            {
                query += "AND (PASS_NO LIKE :PassNoEq||'%')";
                queryCount += "AND (PASS_NO LIKE :PassNoEq||'%')";
                queryParam.Add(ctx.CreateParameter("PassNoEq", passNoEq));
                queryCountParam.Add(ctx.CreateParameter("PassNoEq", passNoEq));
            }

            var passNoMoreThan = listRequest.Filter?.PassNoMoreThan;
            if (passNoMoreThan != null)
            {
                query += "AND (PASS_NO >= :MoreThan)";
                queryCount += "AND (PASS_NO >= :MoreThan)";
                queryParam.Add(ctx.CreateParameter("MoreThan", passNoMoreThan));
                queryCountParam.Add(ctx.CreateParameter("MoreThan", passNoMoreThan));
            }

            var passNoLessThan = listRequest.Filter?.PassNoLessThan;
            if (passNoLessThan != null)
            {
                query += "AND (PASS_NO <= :LessThan)";
                queryCount += "AND (PASS_NO <= :LessThan)";
                queryParam.Add(ctx.CreateParameter("LessThan", passNoLessThan));
                queryCountParam.Add(ctx.CreateParameter("LessThan", passNoLessThan));
            }

            query += "ORDER BY OUT_PIECE_ID";

            var data = ctx.GetEntities<RepHmPieceStatListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            if (chartFlag == 1)
            {
                foreach (var item in data)
                {
                    var floatArray = new float[item.SampleData.Length / 4];
                    Buffer.BlockCopy(item.SampleData, 0, floatArray, 0, item.SampleData.Length);
                    item.ChartDataY = floatArray;
                    string query2 = $@"SELECT 
                            SAMPLE_DATA     AS SampleData
                            FROM REP_HM_PIECE_STAT A JOIN AUX_VALUE B
                            ON B.VARIABLE_ID = 'SAMPLE_NAME' AND B.VALUE_NAME = A.SAMPLE_ID 
                            WHERE 
                            A.OUT_PIECE_NO = :OutPieceNo AND A.PASS_NO = :PassNo AND SAMPLE_ID = 'SAMPLE_OFFSET'
                            AND 1 = 1  ";
                    List<IDbDataParameter> queryParam2 = new List<IDbDataParameter>();
                    var data2 = ctx.GetEntities<RepHmPieceStatListItemDto>(query2, ctx.CreateParameter("OutPieceNo", item.OutPieceNo), ctx.CreateParameter("PassNo", item.PassNo)).ToArray();
                    foreach (var item2 in data2)
                    {
                        var floatArray2 = new float[item2.SampleData.Length / 4];
                        Buffer.BlockCopy(item2.SampleData, 0, floatArray2, 0, item2.SampleData.Length);
                        item.ChartDataX = floatArray2;
                    }
                    item.SampleData = null;
                }
            }

            return new ListResultDto<RepHmPieceStatListItemDto>
            {
                Data = data,
                Total = total
            };
        }
    }
}