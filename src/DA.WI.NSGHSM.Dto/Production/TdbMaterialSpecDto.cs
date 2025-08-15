using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class TdbMaterialSpecBaseDto
    {
    }

    public class TdbMaterialSpecDto : TdbMaterialSpecBaseDto
    {
    }

    public class TdbMaterialSpecListItemDto : TdbMaterialSpecDto
    {
    }

    public class TdbMaterialSpecForInsertDto : TdbMaterialSpecBaseDto
    {
    }
    public class TdbMaterialSpecLookupDto
    {
        public string Display { get; set; }
        public int Value { get; set; }
    }

    public class TdbMaterialSpecForUpdateDto : TdbMaterialSpecBaseDto
    {
    }

    public class TdbMaterialSpecDetailDto : TdbMaterialSpecDto
    {
    }

    public class TdbMaterialSpecListFilterDto 
    {
    }
}