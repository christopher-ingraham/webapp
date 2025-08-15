using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class RepHmRollBaseDto
    {

    }

    public class RepHmRollDto : RepHmRollBaseDto
    {

    }

    public class RepHmRollListItemDto : RepHmRollDto
    {

    }

    public class RepHmRollForInsertDto : RepHmRollBaseDto
    {
    }

    public class RepHmRollForUpdateDto : RepHmRollBaseDto
    {
        
    }

    public class RepHmRollDetailDto : RepHmRollDto
    {
        public string AreaId { get; set; }
        public string CenterId { get; set; }
        public int StandNo { get; set; }
        public string WrLoId { get; set; }      // WR Button Roll
        public double WrLoDiameter { get; set; }
        public double WrLoRolledLen { get; set; }
        public string WrUpId { get; set; }       // WR Top Roll ID
        public double WrUpDiameter { get; set; }
        public double WrUpRolledLen { get; set; }   // Rolled Length [m]
        public string BrLoId { get; set; }      // BUR Button Roll
        public double BrLoDiameter { get; set; }
        public double BrLoRolledLen { get; set; }
        public string BrUpId { get; set; }      // BUR Top Roll ID
        public double BrUpDiameter { get; set; }
        public double BrUpRolledLen { get; set; }
    }

    public class RepHmRollListFilterDto
    {
    }
}