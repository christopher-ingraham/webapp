using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.QualityControlSystem
{

    public class TdbMaterialGradeBaseDto
    {
        public string MaterialGradeId { get; set; }
        public int AlloyCodeCore { get; set; }
    }

    public class TdbMaterialGradeDto : TdbMaterialGradeBaseDto
    {
    }

    public class TdbMaterialGradeListItemDto : TdbMaterialGradeDto
    {
    }

    public class TdbMaterialGradeForInsertDto : TdbMaterialGradeBaseDto
    {
    }

    public class TdbMaterialGradeForUpdateDto : TdbMaterialGradeBaseDto
    {
    }

    public class TdbMaterialGradeDetailDto : TdbMaterialGradeDto
    {
    }

    public class TdbMaterialGradeLookupDto
    {
        public string Display { get; set; }
        public string Value { get; set; }
    }

    public class TdbMaterialGradeListFilterDto
    {
    }
}