using DA.DB.Utils;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo._Core;

namespace DA.WI.NSGHSM.Repo.QualityControlSystem
{
    internal class TdbAlloyRepo<TDataSource> : ITdbAlloyRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public TdbAlloyRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        protected TdbChemCompDto GetTdbChemComp(int ChemCompCnt)
        {
            string query = @" SELECT A.CHEM_ALUMINIUM AS Aluminium
                      , A.CHEM_ANTIMONY         AS Antimony
                      , A.CHEM_ARSENIC          AS Arsenic
                      , A.CHEM_BARIUM           AS Barium
                      , A.CHEM_BERYLLIUM        AS Beryllium
                      , A.CHEM_BISMUTH          AS Bismuth
                      , A.CHEM_BORON            AS Boron
                      , A.CHEM_CADMIUM          AS Cadmium
                      , A.CHEM_CAESIUM          AS Caesium
                      , A.CHEM_CALCIUM          AS Calcium
                      , A.CHEM_CARBON           AS Carbon
                      , A.CHEM_CERIUM           AS Cerium
                      , A.CHEM_CHROMIUM         AS Chromium
                      , A.CHEM_COBALT           AS Cobalt
                      , A.CHEM_COPPER           AS Copper
                      , A.CHEM_GALLIUM          AS Gallium
                      , A.CHEM_GERMANIUM        AS Germanium
                      , A.CHEM_HYDROGEN         AS Hydrogen
                      , A.CHEM_INDIUM           AS Indium
                      , A.CHEM_IRON             AS Iron
                      , A.CHEM_LANTHANUM        AS Lanthanum
                      , A.CHEM_LEAD             AS Lead
                      , A.CHEM_LITHIUM          AS Lithium
                      , A.CHEM_MAGNESIUM        AS Magnesium
                      , A.CHEM_MANGANESE        AS Manganese
                      , A.CHEM_MERCURY          AS Mercury
                      , A.CHEM_MOLYBDENUM       AS Molybdenum
                      , A.CHEM_NICKEL           AS Nickel
                      , A.CHEM_NIOBIUM          AS Niobium
                      , A.CHEM_NITROGEN         AS Nitrogen
                      , A.CHEM_OXYGEN           AS Oxygen
                      , A.CHEM_PHOSPHORUS       AS Phosphorus
                      , A.CHEM_POTASSIUM        AS Potassium
                      , A.CHEM_RHENIUM          AS Rhenium
                      , A.CHEM_SCANDIUM         AS Scandium
                      , A.CHEM_SELENIUM         AS Selenium
                      , A.CHEM_SILICON          AS Silicon
                      , A.CHEM_SILVER           AS Silver
                      , A.CHEM_SODIUM           AS Sodium
                      , A.CHEM_STRONTIUM        AS Strontium
                      , A.CHEM_SULPHUR          AS Sulphur
                      , A.CHEM_TANTALUM         AS Tantalum
                      , A.CHEM_TELLURIUM        AS Tellurium
                      , A.CHEM_THALLIUM         AS Thallium
                      , A.CHEM_THORIUM          AS Thorium
                      , A.CHEM_TIN              AS Tin
                      , A.CHEM_TITANIUM         AS Titanium
                      , A.CHEM_TUNGSTEN         AS Tungsten
                      , A.CHEM_URANIUM          AS Uranium
                      , A.CHEM_VANADIUM         AS Vanadium
                      , A.CHEM_YTTRIUM          AS Yttrium
                      , A.CHEM_ZINC             AS Zinc
                      , A.CHEM_ZIRCONIUM        AS Zirconium
                  FROM TDB_CHEM_COMP  A 
                  WHERE A.CHEM_COMP_CNT = :ChemCompCnt";

            var result = ctx.GetEntity<TdbChemCompDto>(query, ctx.CreateParameter("ChemCompCnt", ChemCompCnt));

            return result;
        }

        public TdbAlloyDto GetCurrentTdbAlloy(int AlloyCode)
        {

            string query = @"SELECT 
                        TRIM(A.ALLOY_ID)            AS AlloyId
                      , TRIM(A.ALLOY_DESCRIPTION)   AS AlloyDescription
                      , A.CHEM_COMP_NOM             AS ChemCompNom
                      , A.CHEM_COMP_MIN             AS ChemCompMin
                      , A.CHEM_COMP_MAX             AS ChemCompMax
                  FROM TDB_ALLOY  A 
                  WHERE A.ALLOY_CODE = :AlloyCode ";

            var tdbAlloy = ctx.GetEntity<TdbAlloyDto>(query, ctx.CreateParameter("AlloyCode", AlloyCode));

            if (tdbAlloy != null)
            {

                TdbChemCompDto[] chemCompArray = new TdbChemCompDto[3];
                chemCompArray[0] = GetTdbChemComp(tdbAlloy.ChemCompNom);
                chemCompArray[1] = GetTdbChemComp(tdbAlloy.ChemCompMin);
                chemCompArray[2] = GetTdbChemComp(tdbAlloy.ChemCompMax);


                return new TdbAlloyDto
                {
                    AlloyId = tdbAlloy.AlloyId,
                    AlloyDescription = tdbAlloy.AlloyDescription,
                    ChemCompNom = tdbAlloy.ChemCompNom,
                    ChemCompMin = tdbAlloy.ChemCompMin,
                    ChemCompMax = tdbAlloy.ChemCompMax,
                    ChemComp = chemCompArray
                };
            }
            else return new TdbAlloyDto { };
        }
    }
}