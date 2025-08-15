using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace DA.WI.NSGHSM.Dto.QualityControlSystem
{

    [DataContract]
    public class RepHmTrendsViewBaseDto
    {
        [DataMember]
        public string SignalId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string MeasUnit { get; set; }
        [DataMember]
        public int OutPieceNo { get; set; }
        
        public string CenterId { get; set; }
        [DataMember]
        public int PassNo { get; set; }
        [DataMember]
        public int NumSignals { get; set; }
        [DataMember]
        public int SignalType { get; set; } 
        public int CompressionLevel { get; set; } 
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

    public class RepHmTrendsViewDto : RepHmTrendsViewBaseDto
    {
    }

    public class RepHmTrendsViewListItemDto : RepHmTrendsViewDto
    {
    }

    public class RepHmTrendsViewDetailDto : RepHmTrendsViewDto
    {
    }

     public class RepHmTrendsViewListFilterDto
    {
        public int? OutPieceNoEq { get; set; }

    }
}