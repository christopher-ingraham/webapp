using System.Diagnostics;
using System.Xml;
using System.Net;
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
    internal class RepHmMapsViewRepo<TDataSource> : IRepHmMapsViewRepo<TDataSource>
         where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public RepHmMapsViewRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<RepHmMapsViewListItemDto> ReadList(ListRequestDto<RepHmMapsViewListFilterDto> listRequest)
        {
            if (listRequest.Filter.ChartData == 1)
            {
                switch (listRequest.Filter.SignalTypeEq)
                {
                    case 1:
                        return getDataSignalType1(listRequest);
                    case 2:
                        return getDataSignalType2(listRequest);
                    case 3:
                        return getDataSignalType3(listRequest);
                }
            }
            List<IDbDataParameter> dataQueryParams = new List<IDbDataParameter>();
            List<IDbDataParameter> countQueryParams = new List<IDbDataParameter>();
            List<IDbDataParameter> param = new List<IDbDataParameter>();
            const int coilFinished = 85;
            const int coilPartialRejected = 111;

            string query1 = @"SELECT    
                             NVL(PDI.MILL_MODE, -1) AS MillMode
                  FROM      REP_HM_PIECE PDO
                  LEFT JOIN HRM_INPUT_PIECE PDI
                         ON PDI.PIECE_NO = PDO.IN_PIECE_NO
                  LEFT JOIN HRM_ORDER ORD
                         ON ORD.PIECE_NO = PDO.IN_PIECE_NO ";
            query1 += "WHERE PDO.STATUS BETWEEN :Status0 AND :Status1 ";
            query1 += "AND PDO.OUT_PIECE_NO = :OutPieceNo ";
            param.Add(ctx.CreateParameter("Status0", coilFinished));
            param.Add(ctx.CreateParameter("Status1", coilPartialRejected));
            param.Add(ctx.CreateParameter("OutPieceNo", listRequest.Filter?.OutPieceNoEq));
            int millMode = ctx.GetEntity<int>(query1, param.ToArray());

            string dataQuery =
                $@" SELECT    DISTINCT
                    S.MAP_NO                AS MapNo
                  , TRIM(M.MAP_ID)          AS signalId
                  , TRIM(S.CENTER_ID)       AS CenterId
                  , TRIM(M.DESCRIPTION)     AS DESCRIPTION
                  , TRIM(S.UNIT)            AS MeasUnit
                  , S.SIGNAL_NO             AS SignalNo
                  , S.SIGNAL_TYPE           AS SignalType
                  , S.OFFSET_SIGNAL_NO      AS OffsetSignalNo
                  FROM      REP_HM_PROC_SIGNAL  S
                  , REP_HM_PROC_SIGNAL_MAP M
                  WHERE     S.MAP_NO = M.MAP_NO
                  AND S.SIGNAL_NO = (SELECT MIN(SIGNAL_NO) FROM REP_HM_PROC_SIGNAL WHERE MAP_NO = M.MAP_NO)
                  AND S.DISPLAY_MODE >= 0
                  AND S.ENABLE_FLG = 1
                  AND S.CENTER_ID <> :CenterId ";

            string queryCount =
                $@" SELECT COUNT(S.MAP_NO)
                    FROM      REP_HM_PROC_SIGNAL  S
                  , REP_HM_PROC_SIGNAL_MAP M
                  WHERE  S.MAP_NO = M.MAP_NO
                  AND S.SIGNAL_NO = (SELECT MIN(SIGNAL_NO) FROM REP_HM_PROC_SIGNAL WHERE MAP_NO = M.MAP_NO)
                  AND S.DISPLAY_MODE >= 0
                  AND S.ENABLE_FLG = 1
                  AND S.CENTER_ID <> :CenterId ";

            if (millMode == 0)
            {
                string centerId = "RM";
                centerId = centerId.PadRight(32, ' ');
                dataQueryParams.Add(ctx.CreateParameter("CenterId", centerId));
                countQueryParams.Add(ctx.CreateParameter("CenterId", centerId));
            }
            else
            {
                string centerId = "EMPTY";
                centerId = centerId.PadRight(32, ' ');
                dataQueryParams.Add(ctx.CreateParameter("CenterId", centerId));
                countQueryParams.Add(ctx.CreateParameter("CenterId", centerId));
            }

            dataQuery += "ORDER BY  S.MAP_NO ASC";

            var data = ctx.GetEntities<RepHmMapsViewListItemDto>(ctx.PaginateSqlStatement(dataQuery, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), dataQueryParams.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, countQueryParams.ToArray());

            foreach (var item in data)
            {
                item.OutPieceNo = listRequest.Filter.OutPieceNoEq;
            }


            return new ListResultDto<RepHmMapsViewListItemDto>
            {
                Data = data,
                Total = total
            };
        }

        private ListResultDto<RepHmMapsViewListItemDto> getDataSignalType1(ListRequestDto<RepHmMapsViewListFilterDto> listRequest)
        {
            return getDataSignalType2(listRequest);
        }

        private ListResultDto<RepHmMapsViewListItemDto> getDataSignalType2(ListRequestDto<RepHmMapsViewListFilterDto> listRequest)
        {
            string signalId = listRequest.Filter.SampleIdEq.PadRight(32, ' ');
            RepHmMapsViewListItemDto dto = new RepHmMapsViewListItemDto();

            string query1 =
               $@" SELECT DISTINCT
                    M.MAP_NO                    AS MapNo,
                    TRIM(M.MAP_ID)              AS SignalId,
                    TRIM(M.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    SIGNAL_TYPE                 AS SignalType,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS               AS NumSignals,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo 
                FROM
                    REP_HM_PIECE_TREND A, REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                            WHERE (B.SIGNAL_ID = A.SIGNAL_ID)
 	                         AND(M.MAP_NO =B.MAP_NO)
                     AND B.SIGNAL_TYPE = 2  
                     AND m.MAP_ID = :SignalId
                     AND A.OUT_PIECE_NO = :OutPieceNo ";

            var data1 = ctx.GetEntity<RepHmMapsViewListItemDto>(query1, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                    ctx.CreateParameter("SignalId", signalId));
            if (data1 != null)
            {
                if (data1.NumSignals == 1)
                {
                    string query2 =
                       $@" SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalIdPartial,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS               AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) AND(M.MAP_NO=B.MAP_NO)
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND B.SIGNAL_TYPE = 2
                    AND M.MAP_ID = :SignalId ";

                    var data2 = ctx.GetEntities<RepHmMapsViewListItemDto>(query2, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                              ctx.CreateParameter("SignalId", signalId)
                    ).ToArray();


                    string query3 = @"SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalIdPartial,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS               AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID)) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_NO = :OffsetSignalNo
                    ORDER BY A.SIGNAL_ID ";

                    var data3 = ctx.GetEntity<RepHmMapsViewListItemDto>(query3, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                            ctx.CreateParameter("OffsetSignalNo", data1.OffsetSignalNo)
                    );

                    if (data3.CompressionLevel != 0)
                    {
                        data3.SampleData = DataCompressionUtility.Decompress(data3.SampleData);
                    }

                    int i = 0;
                    foreach (var item in data2)
                    {
                        if (item.CompressionLevel != 0)
                        {
                            item.SampleData = DataCompressionUtility.Decompress(item.SampleData);
                        }

                        int num = item.SampleData.Length / 4;
                        var floatArray = new float[num];
                        Buffer.BlockCopy(item.SampleData, 0, floatArray, 0, item.SampleData.Length);
                        if (dto.ChartDataY == null)
                        {
                            dto.ChartDataY = new float[data2.Count()];
                        }
                        if (dto.ChartDataZ == null)
                        {
                            dto.ChartDataZ = new float[data2.Count()][];
                        }
                        dto.ChartDataZ[i] = new float[num - 1];
                        for (int j = 1; j < num; j++)
                        {
                            dto.ChartDataZ[i][j - 1] = floatArray[j];
                        }
                        dto.ChartDataY[i] = floatArray[0];
                        i++;

                        int sampleLength = data3.SampleData.Length / 4;
                        var floatArrayX = new float[sampleLength];
                        Buffer.BlockCopy(data3.SampleData, 0, floatArrayX, 0, data3.SampleData.Length);
                        dto.ChartDataX = floatArrayX.Skip(1).ToArray();

                    }
                }
                else if (data1.NumSignals > 1)
                {
                    string query2 =
                        $@" SELECT 
                    M.MAP_NO                    AS MapNo,
                    TRIM(M.MAP_ID )             AS SignalId,
                    TRIM(M.DESCRIPTION )        AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    B.CENTER_ID                 AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS               AS NumSignals,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo,
                    SAMPLE_DATA                 AS SampleData,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    A.NUM_SAMPLES               AS NumSamples
                FROM
                    REP_HM_PIECE_TREND A, REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                    WHERE (B.SIGNAL_ID = A.SIGNAL_ID)
 	                AND(M.MAP_NO =B.MAP_NO)
                    AND B.SIGNAL_TYPE = 2
                    AND NUM_SIGNALS > 1
                    AND OUT_PIECE_NO = :OutPieceNo
                    AND M.MAP_ID = :SignalId
                    ORDER BY A.NUM_SAMPLES DESC  ";

                    var data2 = ctx.GetEntity<RepHmMapsViewListItemDto>(query2, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                                  ctx.CreateParameter("SignalId", signalId));

                    if (data2.CompressionLevel != 0)
                    {
                        data2.SampleData = DataCompressionUtility.Decompress(data2.SampleData);
                    }

                    int x = (data2.NumSamples) / (data2.NumSignals);
                    int y = data2.NumSignals;
                    dto.ChartDataY = new float[y];

                    int num = data2.SampleData.Length / 4;

                    var floatArray = new float[num];
                    Buffer.BlockCopy(data2.SampleData, 0, floatArray, 0, data2.SampleData.Length);

                    dto.ChartDataZ = new float[y][];
                    for (int i = 0; i < y; i++)
                    {
                        dto.ChartDataZ[i] = new float[x-1];
                    }

                    for (int i = 0; i < y; i++)
                    {
                        dto.ChartDataY[i] = floatArray[i * x];
                        for (int j = 1; j < x; j++)
                        {
                            dto.ChartDataZ[i][j-1] = floatArray[i * x + j];
                        }
                    }

                    string centerId = data2.CenterId.PadRight(32, ' ');

                    string query3 = @"SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalIdPartial,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS               AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE (B.SIGNAL_ID = A.SIGNAL_ID) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_NO = :OffsetSignalNo
                    AND B.CENTER_ID = :CenterId ";

                    var dataX = ctx.GetEntity<RepHmMapsViewListItemDto>(query3, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                                ctx.CreateParameter("OffsetSignalNo", data2.OffsetSignalNo),
                                                                                ctx.CreateParameter("CenterId", centerId)
                    );

                    if (dataX.CompressionLevel != 0)
                    {
                        dataX.SampleData = DataCompressionUtility.Decompress(dataX.SampleData);
                    }

                    int sampleLength = dataX.SampleData.Length / 4;

                    var floatArrayX = new float[sampleLength];
                    Buffer.BlockCopy(dataX.SampleData, 0, floatArrayX, 0, dataX.SampleData.Length);
                    dto.ChartDataX = floatArrayX.Skip(1).ToArray();

                }

                dto.MapNo = data1.MapNo;
                dto.Description = data1.Description;
                dto.NumSignals = data1.NumSignals;
                dto.OffsetSignalNo = data1.OffsetSignalNo;
                dto.SignalId = data1.SignalId;
                dto.MeasUnit = data1.MeasUnit;
                dto.OutPieceNo = data1.OutPieceNo;
                dto.PassNo = data1.PassNo;
                dto.SignalType = data1.SignalType;
                if (data1.DisplayMode == 1)
                {
                    dto.ChartType = "heatmap";
                }
                else
                {
                    dto.ChartType = "surface";
                }
            }

            RepHmMapsViewListItemDto[] dtoArray = new RepHmMapsViewListItemDto[1];
            dtoArray[0] = dto;

            return new ListResultDto<RepHmMapsViewListItemDto>
            {
                Data = dtoArray,
                Total = dtoArray.Length
            };
        }

        private ListResultDto<RepHmMapsViewListItemDto> getDataSignalType3(ListRequestDto<RepHmMapsViewListFilterDto> listRequest)
        {
            string signalId = listRequest.Filter.SampleIdEq.PadRight(32, ' ');
            RepHmMapsViewListItemDto dto = new RepHmMapsViewListItemDto();

            string query1 =
                   $@" SELECT
                        M.MAP_NO                    AS MapNo,
                        TRIM(A.SIGNAL_ID)           AS SignalId,
                        TRIM(B.DESCRIPTION)         AS Description,
                        TRIM(B.UNIT)                AS MeasUnit,
                        A.OUT_PIECE_NO              AS OutPieceNo,
                        TRIM(B.CENTER_ID)           AS CenterId,
                        SIGNAL_TYPE                 AS SignalType,
                        A.PASS_NO                   AS PassNo,
                        A.NUM_SIGNALS               AS NumSignals,
                        SAMPLE_DATA                 AS SampleData,
                        A.COMPRESSION_LEVEL         AS CompressionLevel,
                        B.DISPLAY_MODE              AS DisplayMode,
                        A.NUM_SAMPLES               AS NumSamples,
                        B.OFFSET_SIGNAL_NO          AS OffsetSignalNo,
                        B.DISPL_SIGNAL_NO           AS DisplSignalNo
                    FROM
                        REP_HM_PIECE_TREND A,
                        REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                    WHERE (B.SIGNAL_ID = A.SIGNAL_ID) AND(M.MAP_NO=B.MAP_NO)
                        AND (A.OUT_PIECE_NO = :OutPieceNo)
                        AND B.SIGNAL_TYPE = 3
                        AND NUM_SIGNALS > 1
                        AND M.MAP_ID = :SignalId ";

            var data1 = ctx.GetEntity<RepHmMapsViewListItemDto>(query1, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                        ctx.CreateParameter("SignalId", signalId));

            if (data1 != null)
            {
                if (data1.CompressionLevel != 0)
                {
                    data1.SampleData = DataCompressionUtility.Decompress(data1.SampleData);
                }

                if (data1.NumSignals == 1)
                {


                }
                else if (data1.NumSignals > 1)
                {

                    int x = (data1.NumSamples) / (data1.NumSignals);
                    int y = data1.NumSignals;
                    dto.ChartDataY = new float[y];

                    int num = data1.SampleData.Length / 4;
                    var floatArray = new float[num];
                    Buffer.BlockCopy(data1.SampleData, 0, floatArray, 0, data1.SampleData.Length);

                    dto.ChartDataZ = new float[y][];
                    for (int i = 0; i < y; i++)
                    {
                        dto.ChartDataZ[i] = new float[x - 1];
                    }

                    for (int i = 0; i < y; i++)
                    {
                        for (int j = 1; j < x; j++)
                        {
                            dto.ChartDataZ[i][j - 1] = floatArray[i * x + j];
                        }
                    }

                    string centerId = data1.CenterId.PadRight(32, ' ');

                    string queryX = @"SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalIdPartial,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.NUM_SIGNALS               AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE (B.SIGNAL_ID = A.SIGNAL_ID) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_NO = :OffsetSignalNo
                    AND B.CENTER_ID = :CenterId ";

                    var dataX = ctx.GetEntity<RepHmMapsViewListItemDto>(queryX, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                                   ctx.CreateParameter("OffsetSignalNo", data1.OffsetSignalNo),
                                                                                   ctx.CreateParameter("CenterId", centerId)
                    );

                    if (dataX.CompressionLevel != 0)
                    {
                        dataX.SampleData = DataCompressionUtility.Decompress(dataX.SampleData);
                    }

                    int sampleLength2 = dataX.SampleData.Length / 4;
                    var floatArrayX = new float[sampleLength2];
                    Buffer.BlockCopy(dataX.SampleData, 0, floatArrayX, 0, dataX.SampleData.Length);
                    dto.ChartDataX = floatArrayX.Skip(1).ToArray();

                    string queryY = @"SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalIdPartial,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.NUM_SIGNALS				AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE (B.SIGNAL_ID = A.SIGNAL_ID) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND B.DISPL_SIGNAL_NO = :DisplSignalNo
                    AND B.CENTER_ID = :CenterId ";

                    var dataY = ctx.GetEntity<RepHmMapsViewListItemDto>(queryY, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                                   ctx.CreateParameter("DisplSignalNo", data1.DisplSignalNo),
                                                                                   ctx.CreateParameter("CenterId", centerId)
                    );

                    if (dataY.CompressionLevel != 0)
                    {
                        dataY.SampleData = DataCompressionUtility.Decompress(dataY.SampleData);
                    }

                    int sampleLengthY = dataY.SampleData.Length / 4;
                    var floatArrayY = new float[sampleLengthY];
                    Buffer.BlockCopy(dataY.SampleData, 0, floatArrayY, 0, dataY.SampleData.Length);
                    dto.ChartDataY = floatArrayY.Skip(1).ToArray();

                }

                dto.MapNo = data1.MapNo;
                dto.Description = data1.Description;
                dto.NumSignals = data1.NumSignals;
                dto.OffsetSignalNo = data1.OffsetSignalNo;
                dto.SignalId = data1.SignalId;
                dto.MeasUnit = data1.MeasUnit;
                dto.OutPieceNo = data1.OutPieceNo;
                dto.PassNo = data1.PassNo;
                dto.SignalType = data1.SignalType;
                dto.DisplayMode = data1.DisplayMode;
                dto.CenterId = data1.CenterId;
                if (data1.DisplayMode == 1)
                {
                    dto.ChartType = "heatmap";
                }
                else
                {
                    dto.ChartType = "surface";
                }
            }

            RepHmMapsViewListItemDto[] dtoArray = new RepHmMapsViewListItemDto[1];
            dtoArray[0] = dto;

            return new ListResultDto<RepHmMapsViewListItemDto>
            {
                Data = dtoArray,
                Total = dtoArray.Length
            };

        }
    }
}
