using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.QualityControlSystem
{

    public class TdbProcessCodeBaseDto
    {
        public string ProcessCodeLabel { get; set; }
        public int CodeId { get; set; }
    }

    public class TdbProcessCodeDto : TdbProcessCodeBaseDto
    {
    }

    public class TdbProcessCodeListItemDto : TdbProcessCodeDto
    {
    }

    public class TdbProcessCodeForInsertDto : TdbProcessCodeBaseDto
    {
    }

    public class TdbProcessCodeForUpdateDto : TdbProcessCodeBaseDto
    {
    }

    public class TdbProcessCodeDetailDto : TdbProcessCodeDto
    {
    }

     public class TdbProcessCodeListFilterDto 
    {
        public int CodeType { get; set; }
    }
}