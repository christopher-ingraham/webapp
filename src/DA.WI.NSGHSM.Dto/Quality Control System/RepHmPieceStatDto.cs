using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.QualityControlSystem
{

    public class RepHmPieceStatBaseDto
    {
        public string SignalId { get; set; }
        public string Description { get; set; }
        public string MeasUnit { get; set; }
        public int OutPieceNo { get; set; }
        public string OutPieceArea { get; set; }
        public int PassNo { get; set; }
        public byte[] SampleData { get; set; }
        public float[] ChartDataX { get; set; }
        public float[] ChartDataY { get; set; }
        
    }

    public class RepHmPieceStatDto : RepHmPieceStatBaseDto
    {
    }

    public class RepHmPieceStatListItemDto : RepHmPieceStatDto
    {
    }

    public class RepHmPieceStatDetailDto : RepHmPieceStatDto
    {
    }

     public class RepHmPieceStatListFilterDto
    {
        public int? OutPieceNoMoreThan { get; set; }
        public int? OutPieceNoLessThan { get; set; }
        public int? OutPieceNoEq { get; set; }
        public string OutPieceAreaEq { get; set; }
        public int? PassNoEq { get; set; }
        public int? PassNoMoreThan { get; set; }
        public int? PassNoLessThan { get; set; }
        public string SampleIdEq { get; set; }
        public string SampleIdNe { get;set; }
        public int? ChartData { get; set; }

    }
}