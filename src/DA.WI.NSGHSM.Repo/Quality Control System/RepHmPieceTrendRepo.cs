using Microsoft.VisualBasic.CompilerServices;
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
using System.IO;
using System.IO.Compression;

namespace DA.WI.NSGHSM.Repo.QualityControlSystem
{
    internal class RepHmPieceTrendRepo<TDataSource> : IRepHmPieceTrendRepo<TDataSource>
         where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public RepHmPieceTrendRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<RepHmPieceTrendListItemDto> ReadList(ListRequestDto<RepHmPieceTrendListFilterDto> listRequest)
        {
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


            if (listRequest.Filter.ChartData == 1)
            {
                switch (listRequest.Filter.SignalTypeEq)
                {
                    case 0:
                        return getDataSignalType0(listRequest);
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

            string dataQuery =
                $@"SELECT
                    S.SIGNAL_NO                 AS SignalNo  
                  , TRIM(S.SIGNAL_ID)           AS SignalId
                  , TRIM(S.CENTER_ID)           AS CenterId
                  , TRIM(S.DESCRIPTION)         AS Description
                  , TRIM(S.UNIT)                AS MeasUnit
                  , S.OFFSET_SIGNAL_NO          AS OffsetSignalNo
                  FROM      REP_HM_PROC_SIGNAL  S
                  WHERE     S.MAP_NO IS NULL
                  AND S.DISPLAY_MODE >= 0
                  AND S.ENABLE_FLG = 1
                  AND S.CENTER_ID <> :CenterId ";

            string queryCount =
            $@" SELECT COUNT(S.SIGNAL_NO)
                    FROM      REP_HM_PROC_SIGNAL  S
                  WHERE     S.MAP_NO IS NULL
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

            dataQuery += "ORDER BY S.SIGNAL_NO ASC";

            var data = ctx.GetEntities<RepHmPieceTrendListItemDto>(ctx.PaginateSqlStatement(dataQuery, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), dataQueryParams.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, countQueryParams.ToArray());

            foreach (var item in data)
            {
                item.OutPieceNo = listRequest.Filter.OutPieceNoEq;
            }


            return new ListResultDto<RepHmPieceTrendListItemDto>
            {
                Data = data,
                Total = total
            };
        }


        private ListResultDto<RepHmPieceTrendListItemDto> getDataSignalType0(ListRequestDto<RepHmPieceTrendListFilterDto> listRequest)
        {
            string signalId = listRequest.Filter.SampleIdEq.PadRight(32, ' ');

            string query1 =
                $@" SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalId,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS				AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo
                FROM
                    REP_HM_PIECE_TREND A
                    JOIN REP_HM_PROC_SIGNAL B
                    ON ((B.SIGNAL_ID = A.SIGNAL_ID) AND (B.SIGNAL_TYPE=0)) 
                WHERE
                    (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_ID = :SignalId";


            var data = ctx.GetEntity<RepHmPieceTrendListItemDto>(query1, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                         ctx.CreateParameter("SignalId", signalId)
            );
            if (data != null)
            {

                if (data.CompressionLevel != 0)
                {
                    data.SampleData = DataCompressionUtility.Decompress(data.SampleData);
                }

                var floatArray = new float[data.SampleData.Length / 4];
                Buffer.BlockCopy(data.SampleData, 0, floatArray, 0, data.SampleData.Length);

                string centerId = data.CenterId.PadRight(32, ' ');

                string query2 =
                    $@" SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalIdPartial,
                    TRIM(B.DESCRIPTION)         AS Description,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS				AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) 
                    AND (B.CENTER_ID = :CenterId)) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_NO = :OffsetSignalNo ";

                var data2 = ctx.GetEntity<RepHmPieceTrendListItemDto>(query2, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                              ctx.CreateParameter("OffsetSignalNo", data.OffsetSignalNo),
                                                                              ctx.CreateParameter("CenterId", centerId));

                if (data2.CompressionLevel != 0)
                {
                    data2.SampleData = DataCompressionUtility.Decompress(data2.SampleData);
                }

                var floatArray2 = new float[data2.SampleData.Length / 4];
                Buffer.BlockCopy(data2.SampleData, 0, floatArray2, 0, data2.SampleData.Length);

                
                data.ChartDataY = floatArray.Skip(1).ToArray();
                data.ChartDataX = floatArray2.Skip(1).ToArray();
                data.ChartType = "cartesian";
            }

            RepHmPieceTrendListItemDto[] dtoArray = new RepHmPieceTrendListItemDto[1];
            if(data == null){ data = new RepHmPieceTrendListItemDto();}
            dtoArray[0] = data;

            return new ListResultDto<RepHmPieceTrendListItemDto>
            {
                Data = dtoArray,
                Total = dtoArray.Length
            };
        }

        private ListResultDto<RepHmPieceTrendListItemDto> getDataSignalType1(ListRequestDto<RepHmPieceTrendListFilterDto> listRequest)
        {
            return getDataSignalType2(listRequest);
        }

        private ListResultDto<RepHmPieceTrendListItemDto> getDataSignalType2(ListRequestDto<RepHmPieceTrendListFilterDto> listRequest)
        {
            string signalId = listRequest.Filter.SampleIdEq.PadRight(32, ' ');
            RepHmPieceTrendListItemDto dto = new RepHmPieceTrendListItemDto();
            RepHmPieceTrendListItemDto[] dtoArray = new RepHmPieceTrendListItemDto[1];
            dtoArray[0] = dto; 

            string query1 =
               $@" SELECT DISTINCT
                    TRIM(A.SIGNAL_ID)           AS SignalId,
                    TRIM(M.DESCRIPTION )        AS Description,
                    ''                			AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    'FM'           				AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS 				AS NumSignals,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo 
                FROM
                    REP_HM_PIECE_TREND A, REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                            WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) AND (B.CENTER_ID = 'FM'))
 	                         AND(M.MAP_NO =B.MAP_NO)
                     AND B.SIGNAL_TYPE = 2  
                     AND A.SIGNAL_ID = :SignalId
                     AND A.OUT_PIECE_NO = :OutPieceNo ";

            var data1 = ctx.GetEntity<RepHmPieceTrendListItemDto>(query1, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                          ctx.CreateParameter("SignalId", signalId));

            if (data1 != null)
            {

                if (data1.NumSignals == 1)
                {
                    string query2 =
                       $@" SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalId,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS				AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) AND (B.CENTER_ID = 'FM')) AND(M.MAP_NO=B.MAP_NO)
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND B.SIGNAL_TYPE = 2
                    AND A.SIGNAL_ID = :SignalId
                    ORDER BY A.SIGNAL_ID ";

                    var data2 = ctx.GetEntities<RepHmPieceTrendListItemDto>(query2, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                                   ctx.CreateParameter("SignalId", signalId)).ToArray();


                    string query3 = @"SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalId,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS				AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) AND (B.CENTER_ID = 'FM')) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_NO = :OffsetSignalNo
                    ORDER BY A.SIGNAL_ID ";

                    var data3 = ctx.GetEntity<RepHmPieceTrendListItemDto>(query3, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                                   ctx.CreateParameter("OffsetSignalNo", data1.OffsetSignalNo)
                    );

                    if (data3.CompressionLevel != 0)
                    {
                        data3.SampleData = DataCompressionUtility.Decompress(data3.SampleData);
                    }

                    int i = 0;
                    foreach (var item in data2)
                    {
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
                        floatArrayX = floatArrayX.Skip(1).ToArray();
                        dto.ChartDataX = floatArrayX;

                    }
                }
                else if (data1.NumSignals > 1)
                {
                    string query2 =
                        $@" SELECT 
                    TRIM(A.SIGNAL_ID)           AS SignalId,
                    TRIM(M.DESCRIPTION )        AS Description,
                    ''                			AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    'FM'           				AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS 				AS NumSignals,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo,
                    SAMPLE_DATA                 AS SampleData,
                    A.NUM_SAMPLES               AS NumSamples
                FROM
                    REP_HM_PIECE_TREND A, REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) AND (B.CENTER_ID = 'FM'))
 	                AND(M.MAP_NO =B.MAP_NO)
                    AND B.SIGNAL_TYPE = 2
                    AND NUM_SIGNALS > 1
                    AND OUT_PIECE_NO = :OutPieceNo
                    AND B.SIGNAL_ID = :SignalId ";

                    var data2 = ctx.GetEntity<RepHmPieceTrendListItemDto>(query2, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
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
                        dto.ChartDataZ[i] = new float[x];
                    }

                    for (int i = 0; i < y; i++)
                    {
                        dto.ChartDataY[i] = floatArray[i * x];
                        for (int j = 1; j < x; j++)
                        {
                            dto.ChartDataZ[i][j - 1] = floatArray[i * x + j];
                        }
                    }

                    string query3 = @"SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalIdPartial,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS				AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) AND (B.CENTER_ID = 'FM')) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_NO = :OffsetSignalNo ";

                    var dataX = ctx.GetEntity<RepHmPieceTrendListItemDto>(query3, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                                   ctx.CreateParameter("OffsetSignalNo", data2.OffsetSignalNo));

                    if (dataX.CompressionLevel != 0)
                    {
                        dataX.SampleData = DataCompressionUtility.Decompress(dataX.SampleData);
                    }

                    int sampleLength = dataX.SampleData.Length / 4;

                    var floatArrayX = new float[sampleLength];
                    Buffer.BlockCopy(dataX.SampleData, 0, floatArrayX, 0, dataX.SampleData.Length);
                    floatArrayX = floatArrayX.Skip(1).ToArray();
                    dto.ChartDataX = floatArrayX;

                }


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

            dtoArray[0] = dto;

            return new ListResultDto<RepHmPieceTrendListItemDto>
            {
                Data = dtoArray,
                Total = dtoArray.Length
            };
        }

        private ListResultDto<RepHmPieceTrendListItemDto> getDataSignalType3(ListRequestDto<RepHmPieceTrendListFilterDto> listRequest)
        {
            string signalId = listRequest.Filter.SampleIdEq.PadRight(32, ' ');
            RepHmPieceTrendListItemDto dto = new RepHmPieceTrendListItemDto();
            RepHmPieceTrendListItemDto[] dtoArray = new RepHmPieceTrendListItemDto[1];
            dtoArray[0] = dto;

            string query1 =
                   $@" SELECT
                        TRIM(A.SIGNAL_ID)           AS SignalId,
                        TRIM(B.DESCRIPTION)         AS Description,
                        TRIM(B.UNIT)                AS MeasUnit,
                        A.OUT_PIECE_NO              AS OutPieceNo,
                        TRIM(B.CENTER_ID)           AS CenterId,
                        A.COMPRESSION_LEVEL         AS CompressionLevel,
                        SIGNAL_TYPE                 AS SignalType,
                        A.PASS_NO                   AS PassNo,
                        A.NUM_SIGNALS				AS NumSignals,
                        B.DISPLAY_MODE              AS DisplayMode,
                        A.NUM_SAMPLES               AS NumSamples,
                        B.OFFSET_SIGNAL_NO          AS OffsetSignalNo,
                        B.DISPL_SIGNAL_NO           AS DisplSignalNo,
                        A.SAMPLE_DATA               AS SampleData
                    FROM
                        REP_HM_PIECE_TREND A,
                        REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) AND (B.CENTER_ID = 'FM')) AND (M.MAP_NO=B.MAP_NO)
                        AND (A.OUT_PIECE_NO = :OutPieceNo)
                        AND B.SIGNAL_TYPE = 3
                        AND NUM_SIGNALS > 1
                        AND M.MAP_ID = :SignalId ";


            var data1 = ctx.GetEntity<RepHmPieceTrendListItemDto>(query1, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                          ctx.CreateParameter("SignalId", signalId));

            if (data1 == null)
            {
                return new ListResultDto<RepHmPieceTrendListItemDto>
                {
                    Data = new RepHmPieceTrendListItemDto[0],
                    Total = 0
                };
            }


            if (data1.NumSignals == 1)
            {


            }
            else if (data1.NumSignals > 1)
            {

                string queryY = @"SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalIdPartial,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS				AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) AND (B.CENTER_ID = 'FM')) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_NO = :DisplSignalNo ";

                var dataY = ctx.GetEntity<RepHmPieceTrendListItemDto>(queryY, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                               ctx.CreateParameter("DisplSignalNo", data1.DisplSignalNo)
                );
                if (dataY.CompressionLevel != 0)
                {
                    dataY.SampleData = DataCompressionUtility.Decompress(dataY.SampleData);
                }


                int x = (dataY.NumSamples) / (dataY.NumSignals);
                int y = dataY.NumSignals;
                dto.ChartDataY = new float[y];

                int num = dataY.SampleData.Length / 4;
                var floatArray = new float[num];
                Buffer.BlockCopy(dataY.SampleData, 0, floatArray, 0, dataY.SampleData.Length);

                dto.ChartDataZ = new float[y][];
                for (int i = 0; i < y; i++)
                {
                    dto.ChartDataZ[i] = new float[x];
                }

                for (int i = 0; i < y; i++)
                {
                    dto.ChartDataY[i] = floatArray[i * x];
                    for (int j = 1; j < x; j++)
                    {
                        dto.ChartDataZ[i][j - 1] = floatArray[i * x + j];
                    }
                }



                string queryX = @"SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalIdPartial,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    SIGNAL_TYPE                 AS SignalType,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS				AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    B.DISPLAY_MODE              AS DisplayMode,
                    A.NUM_SAMPLES               AS NumSamples,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE ((B.SIGNAL_ID = A.SIGNAL_ID) AND (B.CENTER_ID = 'FM')) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_NO = :OffsetSignalNo ";

                var dataX = ctx.GetEntity<RepHmPieceTrendListItemDto>(queryX, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                               ctx.CreateParameter("OffsetSignalNo", data1.OffsetSignalNo)
                );

                if (dataX.CompressionLevel != 0)
                {
                    dataX.SampleData = DataCompressionUtility.Decompress(dataX.SampleData);
                }

                int sampleLength2 = dataX.SampleData.Length / 4;
                var floatArrayX = new float[sampleLength2];
                Buffer.BlockCopy(dataX.SampleData, 0, floatArrayX, 0, dataX.SampleData.Length);
                floatArrayX = floatArrayX.Skip(1).ToArray();
                dto.ChartDataX = floatArrayX;

            }


            dto.Description = data1.Description;
            dto.NumSignals = data1.NumSignals;
            dto.OffsetSignalNo = data1.OffsetSignalNo;
            dto.SignalId = data1.SignalId;
            dto.MeasUnit = data1.MeasUnit;
            dto.OutPieceNo = data1.OutPieceNo;
            dto.PassNo = data1.PassNo;
            dto.SignalType = data1.SignalType;
            dto.DisplayMode = data1.DisplayMode;

            if (data1.DisplayMode == 1)
            {
                dto.ChartType = "heatmap";
            }
            else
            {
                dto.ChartType = "surface";
            }

            dtoArray[0] = dto;

            return new ListResultDto<RepHmPieceTrendListItemDto>
            {
                Data = dtoArray,
                Total = dtoArray.Length
            };

        }

    }
}
