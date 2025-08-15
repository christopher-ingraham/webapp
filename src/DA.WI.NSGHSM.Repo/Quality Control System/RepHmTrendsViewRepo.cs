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
    internal class RepHmTrendsViewRepo<TDataSource> : IRepHmTrendsViewRepo<TDataSource>
         where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public RepHmTrendsViewRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<RepHmTrendsViewListItemDto> ReadList(ListRequestDto<RepHmTrendsViewListFilterDto> listRequest)
        {
            RepHmTrendsViewListItemDto[] dtoArray = new RepHmTrendsViewListItemDto[2];
            ListResultDto<RepHmTrendsViewListItemDto> result = new ListResultDto<RepHmTrendsViewListItemDto>();
            RepHmTrendsViewListItemDto dataDisplayMode1 = getDataDisplayMode1(listRequest);
            RepHmTrendsViewListItemDto dataDisplayMode2 = getDataDisplayMode2(listRequest);

            if (dataDisplayMode1 == null && dataDisplayMode2 == null)
            {
                RepHmTrendsViewListItemDto[] dtoArray2 = new RepHmTrendsViewListItemDto[0];
                result.Data = dtoArray2;
                result.Total = dtoArray2.Length;
                return result;
            }

            dtoArray[0] = dataDisplayMode1;
            dtoArray[1] = dataDisplayMode2;
            result.Data = dtoArray;
            result.Total = dtoArray.Length;

            return result;

        }

        private RepHmTrendsViewListItemDto getDataDisplayMode1(ListRequestDto<RepHmTrendsViewListFilterDto> listRequest)
        {

            string query1 =
                $@" SELECT
                    TRIM(A.SIGNAL_ID)           AS SignalId,
                    TRIM(B.DESCRIPTION)         AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    A.NUM_SIGNALS               AS NumSignals,
                    SAMPLE_DATA                 AS SampleData,
                    A.COMPRESSION_LEVEL         AS CompressionLevel,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo,
                    A.COMPRESSION_LEVEL         AS CompressionLevel
                FROM
                    REP_HM_PIECE_TREND A
                    JOIN REP_HM_PROC_SIGNAL B
                    ON (B.SIGNAL_ID = A.SIGNAL_ID)
                WHERE
                    (A.OUT_PIECE_NO = :OutPieceNo)
                    AND B.DISPLAY_MODE=1
                    AND B.SIGNAL_TYPE=0 ";


            var data = ctx.GetEntity<RepHmTrendsViewListItemDto>(query1, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq));
            if (data == null) return null;
            if (data.CompressionLevel != 0)
            {
                data.SampleData = DataCompressionUtility.Decompress(data.SampleData);
            }

            var floatArray = new float[data.SampleData.Length / 4];

            Buffer.BlockCopy(data.SampleData, 0, floatArray, 0, data.SampleData.Length);

            string query2 =
                $@"SELECT
                            SAMPLE_DATA                 AS SampleData,
                            A.COMPRESSION_LEVEL         AS CompressionLevel
                        FROM
                            REP_HM_PIECE_TREND A
                            JOIN REP_HM_PROC_SIGNAL B
                            ON (
                                (B.SIGNAL_ID = A.SIGNAL_ID)
                            )
                        WHERE
                        	A.OUT_PIECE_NO = :OutPieceNo AND 
                            A.SIGNAL_NO = :OffsetSignalNo
                            ORDER BY a.COMPRESSION_LEVEL ASC ";

            var data2 = ctx.GetEntity<RepHmTrendsViewListItemDto>(query2, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                          ctx.CreateParameter("OffsetSignalNo", data.OffsetSignalNo)
                );
            if (data2.CompressionLevel != 0)
            {
                data2.SampleData = DataCompressionUtility.Decompress(data2.SampleData);
            }

            var floatArray2 = new float[data2.SampleData.Length / 4];
            Buffer.BlockCopy(data2.SampleData, 0, floatArray2, 0, data2.SampleData.Length);

            data.ChartDataY = floatArray.Skip(1).ToArray();
            var a = floatArray2.Skip(1).ToArray();
            data.ChartDataX = a.Skip(1).ToArray();
            data.ChartType = "cartesian";

            return data;
        }

        private RepHmTrendsViewListItemDto getDataDisplayMode2(ListRequestDto<RepHmTrendsViewListFilterDto> listRequest)
        {
            RepHmTrendsViewListItemDto dto = new RepHmTrendsViewListItemDto();

            string query1 =
                       $@" SELECT DISTINCT
                    TRIM(M.MAP_ID )             AS SignalId,
                    TRIM(M.DESCRIPTION )        AS Description,
                    TRIM(B.UNIT)                AS MeasUnit,
                    A.OUT_PIECE_NO              AS OutPieceNo,
                    SIGNAL_TYPE                 AS SignalType,
                    A.PASS_NO                   AS PassNo,
                    TRIM(B.CENTER_ID)           AS CenterId,
                    A.NUM_SIGNALS               AS NumSignals,
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo,
                    B.DISPL_SIGNAL_NO           AS DisplSignalNo
                FROM
                    REP_HM_PIECE_TREND A, REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                            WHERE (B.SIGNAL_ID = A.SIGNAL_ID)
 	                         AND(M.MAP_NO =B.MAP_NO)
                     AND B.DISPLAY_MODE = 2
                     AND A.OUT_PIECE_NO = :OutPieceNo ";

            var data1 = ctx.GetEntity<RepHmTrendsViewListItemDto>(query1, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq));
            if (data1 == null) return null;
            if (data1.SignalType == 2)
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
                    AND B.DISPLAY_MODE = 2 ";

                    var data2 = ctx.GetEntities<RepHmTrendsViewListItemDto>(query2, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq)).ToArray();

                    string centerId = data1.CenterId.PadRight(32, ' ');

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
                    AND B.CENTER_ID = :CenterId  
                    ORDER BY A.SIGNAL_ID ";

                    var data3 = ctx.GetEntity<RepHmTrendsViewListItemDto>(query3, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                                   ctx.CreateParameter("OffsetSignalNo", data1.OffsetSignalNo),
                                                                                    ctx.CreateParameter("CenterId", centerId)
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
                        floatArrayX = floatArrayX.Skip(1).ToArray();
                        dto.ChartDataX = floatArrayX;

                        dto.Description = data1.Description;
                        dto.NumSignals = data1.NumSignals;
                        dto.OffsetSignalNo = data1.OffsetSignalNo;
                        dto.SignalId = data1.SignalId;
                        dto.MeasUnit = data1.MeasUnit;
                        dto.OutPieceNo = data1.OutPieceNo;
                        dto.PassNo = data1.PassNo;
                        dto.SignalType = data1.SignalType;
                        dto.DisplayMode = data1.DisplayMode;
                        dto.CenterId = data3.CenterId;
                        dto.ChartType = "surface";


                    }
                }
                else
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
                    A.NUM_SAMPLES               AS NumSamples,
                    A.COMPRESSION_LEVEL         AS CompressionLevel
                FROM
                    REP_HM_PIECE_TREND A, REP_HM_PROC_SIGNAL B, REP_HM_PROC_SIGNAL_MAP M
                    WHERE (B.SIGNAL_ID = A.SIGNAL_ID)
 	                AND(M.MAP_NO =B.MAP_NO)
                    AND NUM_SIGNALS > 1
                    AND OUT_PIECE_NO = :OutPieceNo
                    AND B.DISPLAY_MODE = 2
                    ORDER BY A.NUM_SAMPLES DESC ";

                    var data2 = ctx.GetEntity<RepHmTrendsViewListItemDto>(query2, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq));

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
                        dto.ChartDataZ[i] = new float[x - 1];
                    }

                    for (int i = 0; i < y; i++)
                    {
                        dto.ChartDataY[i] = floatArray[i * x];
                        for (int j = 1; j < x; j++)
                        {
                            dto.ChartDataZ[i][j - 1] = floatArray[i * x + j];
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
                    B.OFFSET_SIGNAL_NO          AS OffsetSignalNo,
                    A.COMPRESSION_LEVEL         AS CompressionLevel
                FROM
                    REP_HM_PIECE_TREND A,
                    REP_HM_PROC_SIGNAL B
                    WHERE (B.SIGNAL_ID = A.SIGNAL_ID) 
                    AND (A.OUT_PIECE_NO = :OutPieceNo)
                    AND A.SIGNAL_NO = :OffsetSignalNo
                    AND B.CENTER_ID = :CenterId  ";

                    var dataX = ctx.GetEntity<RepHmTrendsViewListItemDto>(query3, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                                   ctx.CreateParameter("OffsetSignalNo", data2.OffsetSignalNo),
                                                                                   ctx.CreateParameter("CenterId", data2.CenterId)
                    );

                    if (dataX.CompressionLevel != 0)
                    {
                        dataX.SampleData = DataCompressionUtility.Decompress(dataX.SampleData);
                    }

                    int sampleLength = dataX.SampleData.Length / 4;

                    var floatArrayX = new float[sampleLength];
                    Buffer.BlockCopy(dataX.SampleData, 0, floatArrayX, 0, dataX.SampleData.Length);
                    dto.ChartDataX = floatArrayX.Skip(1).ToArray(); ;

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
                dto.ChartType = "surface";

                return dto;
            }
            else
            {
                 string query0 =
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
                        AND B.DISPLAY_MODE = 2 ";

                var data0 = ctx.GetEntity<RepHmMapsViewListItemDto>(query0, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq));

                if (data0.CompressionLevel != 0)
                {
                    data0.SampleData = DataCompressionUtility.Decompress(data0.SampleData);
                }

                int x = (data0.NumSamples) / (data0.NumSignals);
                int y = data0.NumSignals;
                dto.ChartDataY = new float[y];

                int num = data0.SampleData.Length / 4;
                var floatArray = new float[num];
                Buffer.BlockCopy(data0.SampleData, 0, floatArray, 0, data0.SampleData.Length);

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
                    AND B.DISPL_SIGNAL_NO = :DisplSignalNo
                    AND B.CENTER_ID = :CenterId ";

                var dataY = ctx.GetEntity<RepHmMapsViewListItemDto>(queryY, ctx.CreateParameter("OutPieceNo", listRequest.Filter.OutPieceNoEq),
                                                                               ctx.CreateParameter("DisplSignalNo", data0.DisplSignalNo),
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

                dto.Description = data0.Description;
                dto.NumSignals = data0.NumSignals;
                dto.OffsetSignalNo = data0.OffsetSignalNo;
                dto.SignalId = data0.SignalId;
                dto.MeasUnit = data0.MeasUnit;
                dto.OutPieceNo = data0.OutPieceNo;
                dto.PassNo = data0.PassNo;
                dto.SignalType = data0.SignalType;
                dto.DisplayMode = data0.DisplayMode;
                dto.CenterId = data0.CenterId;
                dto.ChartType = "surface";

                return dto;

            }
        }
    }
}