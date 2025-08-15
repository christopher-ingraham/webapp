using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.QualityControlSystem
{

    public class TdbAlloyBaseDto
    {
        public string AlloyId { get; set; }
        public string AlloyDescription { get; set; }
        public int ChemCompNom { get; set; }
        public int ChemCompMin { get; set; }
        public int ChemCompMax { get; set; }
        public TdbChemCompBaseDto[] ChemComp { get; set; }
    }

    public class TdbAlloyDto : TdbAlloyBaseDto
    {
    }

    public class TdbAlloyListItemDto : TdbAlloyDto
    {
    }

    public class TdbAlloyForInsertDto : TdbAlloyBaseDto
    {
    }

    public class TdbAlloyForUpdateDto : TdbAlloyBaseDto
    {
    }

    public class TdbAlloyDetailDto : TdbAlloyDto
    {
    }

    public class TdbAlloyListFilterDto
    {
        public string SearchAlloyId { get; set; }
    }
}