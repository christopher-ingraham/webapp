using DA.WI.NSGHSM.Dto._Core;
using System;
using System.Text;

namespace DA.WI.NSGHSM.Dto.Production
{

    public class HrmAnalysisDataBaseDto
    {
        public string SampleId { get; set; }
        public int AnalysisCnt { get; set; }

    }

    public class HrmAnalysisDataDto : HrmAnalysisDataBaseDto
    {
    }

    public class HrmAnalysisDataListItemDto : HrmAnalysisDataDto
    {
    }

    public class HrmAnalysisDataForInsertDto : HrmAnalysisDataBaseDto
    {
    }

    public class HrmAnalysisDataForUpdateDto : HrmAnalysisDataBaseDto
    {
    }

    public class HrmAnalysisDataDetailDto : HrmAnalysisDataDto
    {
        public double Aluminium { get; set; }
        public double Silver { get; set; }
        public double Arsenic { get; set; }
        public double Boron { get; set; }
        public double Barium { get; set; }
        public double Beryllium { get; set; }
        public double Bismuth { get; set; }
        public double Carbon { get; set; }
        public double Calcium { get; set; }
        public double Cadmium { get; set; }
        public double Cerium { get; set; }
        public double Cobalt { get; set; }
        public double Chromium { get; set; }
        public double Caesium { get; set; }
        public double Copper { get; set; }
        public double Iron { get; set; }
        public double Gallium { get; set; }
        public double Germanium { get; set; }
        public double Hydrogen { get; set; }
        public double Mercury { get; set; }
        public double Indium { get; set; }
        public double Potassium { get; set; }
        public double Lanthanum { get; set; }
        public double Lithium { get; set; }
        public double Magnesium { get; set; }
        public double Manganese { get; set; }
        public double Molybdenum { get; set; }
        public double Nitrogen { get; set; }
        public double Sodium { get; set; }
        public double Niobium { get; set; }
        public double Nickel { get; set; }
        public double Oxygen { get; set; }
        public double Phosphorus { get; set; }
        public double Lead { get; set; }
        public double Rhenium { get; set; }
        public double Sulphur { get; set; }
        public double Antimony { get; set; }
        public double Scandium { get; set; }
        public double Selenium { get; set; }
        public double Silicon { get; set; }
        public double Tin { get; set; }
        public double Strontium { get; set; }
        public double Tantalum { get; set; }
        public double Tellurium { get; set; }
        public double Thorium { get; set; }
        public double Titanium { get; set; }
        public double Thallium { get; set; }
        public double Uranium { get; set; }
        public double Vanadium { get; set; }
        public double Tungsten { get; set; }
        public double Yttrium { get; set; }
        public double Zinc { get; set; }
        public double Zirconium { get; set; }
        public double AluminumSol { get; set; }
        public double AluminiumTot { get; set; }
        public double TitaniumEff { get; set; }

    }

     public class HrmAnalysisDataListFilterDto
    {
        public int SearchHeatNo { get; set; }
    }
}
