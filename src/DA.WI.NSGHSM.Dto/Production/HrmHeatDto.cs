using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class HrmHeatBaseDto
    {
        public string HeatId { get; set; }
        public int HeatNo { get; set; }
    }

    public class HrmHeatDto : HrmHeatBaseDto
    {
    }

    public class HrmHeatListItemDto : HrmHeatDto
    {
    }

    public class HrmHeatForInsertDto : HrmHeatBaseDto
    {
    }

    public class HrmHeatForUpdateDto : HrmHeatBaseDto
    {
    }

    public class HrmHeatDetailDto : HrmHeatDto
    {
    }

     public class HrmHeatListFilterDto 
    {
        public string SearchJobId { get; set; }
    }
}