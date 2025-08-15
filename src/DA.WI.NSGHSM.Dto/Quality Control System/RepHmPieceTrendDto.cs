using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace DA.WI.NSGHSM.Dto.QualityControlSystem
{

    [DataContract]
    public class RepHmPieceTrendBaseDto
    {
        [DataMember]
        public int SignalNo { get; set; }
        [DataMember]
        public string SignalId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string MeasUnit { get; set; }
        public int CompressionLevel { get; set; }
        [DataMember]
        public int OutPieceNo { get; set; }
        [DataMember]
        public string CenterId { get; set; }
        [DataMember]
        public int PassNo { get; set; }
        [DataMember]
        public int NumSignals { get; set; }
        [DataMember]
        public int SignalType { get; set; } 
        [DataMember]
        public int DisplayMode { get; set; } 
        [DataMember]
        public int NumSamples { get; set; }
        public int DisplSignalNo { get; set; }
        [DataMember]
        public string ChartType { get; set; }
        public int OffsetSignalNo { get; set; } 
        public byte[] SampleData { get; set; }
        [DataMember]
        public float[] ChartDataX { get; set; }
        [DataMember]
        public float[] ChartDataY { get; set; }   
        [DataMember]
        public float[][] ChartDataZ { get; set; }

    }

    public class RepHmPieceTrendDto : RepHmPieceTrendBaseDto
    {
    }

    public class RepHmPieceTrendListItemDto : RepHmPieceTrendDto
    {
    }

    public class RepHmPieceTrendDetailDto : RepHmPieceTrendDto
    {
    }

     public class RepHmPieceTrendListFilterDto
    {
        public int? OutPieceNoMoreThan { get; set; }
        public int? OutPieceNoLessThan { get; set; }
        public int OutPieceNoEq { get; set; }
        public string CenterIdEq { get; set; }
        public int? PassNoEq { get; set; }
        public int? PassNoMoreThan { get; set; }
        public int? PassNoLessThan { get; set; }
        public string SampleIdEq { get; set; }
        public string SampleIdNe { get;set; }
        public int? ChartData { get; set; }
        public int? SignalTypeEq { get; set; }

    }
}