using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.QualityControlSystem
{

    public class RepHmPieceBaseDto
    {
    }

    public class RepHmPieceDto : RepHmPieceBaseDto
    {
        
    }

    public class RepHmPieceListItemDto : RepHmPieceDto
    {
        public long OutPieceNo { get; set; }
        public string ProducedCoilId { get; set; }
        public string InputCoilId { get; set; }
        public int HeatNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public string CustomerLineNo { get; set; }
        public DateTime ProducionStopDate { get; set; }
        public string SteelGradeId { get; set; }
        public int MillMode { get; set; }
        public double Width { get; set; }
        public double Thickness { get; set; }
        public int Weight { get; set; }
        public int ExternalDiameter { get; set; }
        
        //public int GhostRollingFlag { get; set; }
    }

    public class RepHmPieceForUpdateDto : RepHmPieceBaseDto
    {
    }

    public class RepHmPieceDetailDto : RepHmPieceDto
    {
    }

     public class RepHmPieceListFilterDto
    {
        public string SearchProducedCoil { get; set; }
        public string SearchInputCoil { get; set; }
        public DateTimeOffset? SearchDataFrom { get; set; }
        public DateTimeOffset? SearchDataTo { get; set; }
    }
}