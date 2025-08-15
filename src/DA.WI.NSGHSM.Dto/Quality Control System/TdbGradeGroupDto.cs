using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.QualityControlSystem
{

    public class TdbGradeGroupBaseDto
    {
        public string GradeGroupLabel { get; set; }
        public int GradeGroupId { get; set; }
    }

    public class TdbGradeGroupDto : TdbGradeGroupBaseDto
    {
    }

    public class TdbGradeGroupListItemDto : TdbGradeGroupDto
    {
    }

    public class TdbGradeGroupForInsertDto : TdbGradeGroupBaseDto
    {
    }

    public class TdbGradeGroupForUpdateDto : TdbGradeGroupBaseDto
    {
    }

    public class TdbGradeGroupDetailDto : TdbGradeGroupDto
    {
    }

     public class TdbGradeGroupListFilterDto 
    {
    }
}