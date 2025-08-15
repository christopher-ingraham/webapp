using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.QualityControlSystem
{

    public class TdbAlloySpecBaseDto
    {
        public int AlloySpecCnt { get; set; }
        public string AlloySpecCode { get; set; }
        public int AlloySpecVersion { get; set; }
    }

    public class TdbAlloySpecDto : TdbAlloySpecBaseDto
    {
    }

    public class TdbAlloySpecListItemDto : TdbAlloySpecDto
    {
    }

    public class TdbAlloySpecForInsertDto : TdbAlloySpecBaseDto
    {
    }

    public class TdbAlloySpecForUpdateDto : TdbAlloySpecBaseDto
    {
    }

    public class TdbAlloySpecDetailDto : TdbAlloySpecDto
    {
        public int ChemCompMin { get; set; }
        public int ChemCompMax { get; set; }
        public TdbChemCompBaseDto[] ChemComp { get; set; }
    }

    public class TdbAlloySpecListFilterDto
    {
    }
}