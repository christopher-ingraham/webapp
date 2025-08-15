using DA.DB.Utils;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.QualityControlSystem;
using DA.WI.NSGHSM.IRepo.QualityControlSystem;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.QualityControlSystem
{
    internal class TdbAlloySpecRepo<TDataSource> : ITdbAlloySpecRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public TdbAlloySpecRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<TdbAlloySpecListItemDto> SelTdbAlloySpec(ListRequestDto<TdbAlloySpecListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT ALLOY_SPEC_CNT      AS AlloySpecCnt
                            ,TRIM(ALLOY_SPEC_CODE)      AS AlloySpecCode
                            ,ALLOY_SPEC_VERSION         AS AlloySpecVersion
                            FROM TDB_ALLOY_SPEC ";


            string queryCount = @"SELECT COUNT(ALLOY_SPEC_CODE)
                                FROM  TDB_ALLOY_SPEC ";

            query += "ORDER BY ALLOY_SPEC_CNT ";

            var data = ctx.GetEntities<TdbAlloySpecListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<TdbAlloySpecListItemDto>
            {
                Data = data,
                Total = total
            };
        }


        public TdbAlloySpecDetailDto GetTdbAlloySpec(int AlloySpecCnt)
        {

            string query = @"SELECT 
                        TRIM(A.ALLOY_SPEC_CODE)     AS AlloySpecCode
                      , A.ALLOY_SPEC_VERSION        AS AlloySpecVersion
                      , A.ALLOY_SPEC_CNT            AS AlloySpecCnt
                      , A.CHEM_COMP_MIN             AS ChemCompMin
                      , A.CHEM_COMP_MAX             AS ChemCompMax
                    FROM  TDB_ALLOY_SPEC  A
                    WHERE A.ALLOY_SPEC_CNT = :AlloySpecCnt ";

            var tdbSpecAlloy = ctx.GetEntity<TdbAlloySpecDetailDto>(query, ctx.CreateParameter("AlloySpecCnt", AlloySpecCnt));

            if (tdbSpecAlloy != null)
            {

                TdbChemCompDto[] chemCompArray = new TdbChemCompDto[2];
                chemCompArray[0] = GetTdbChemComp(tdbSpecAlloy.ChemCompMin);
                chemCompArray[1] = GetTdbChemComp(tdbSpecAlloy.ChemCompMax);


                return new TdbAlloySpecDetailDto
                {
                    AlloySpecCnt = tdbSpecAlloy.AlloySpecCnt,
                    AlloySpecCode = tdbSpecAlloy.AlloySpecCode,
                    AlloySpecVersion = tdbSpecAlloy.AlloySpecVersion,
                    ChemCompMin = tdbSpecAlloy.ChemCompMin,
                    ChemCompMax = tdbSpecAlloy.ChemCompMax,
                    ChemComp = chemCompArray
                };
            }
            else return new TdbAlloySpecDetailDto { };
        }

        protected TdbChemCompDto GetTdbChemComp(int ChemCompCnt)
        {
            string query = @" SELECT A.CHEM_ALUMINIUM       AS Aluminium
                      , A.CHEM_ANTIMONY                     AS Antimony
                      , A.CHEM_ARSENIC                      AS Arsenic
                      , A.CHEM_BARIUM                       AS Barium
                      , A.CHEM_BERYLLIUM                    AS Beryllium
                      , A.CHEM_BISMUTH                      AS Bismuth
                      , A.CHEM_BORON                        AS Boron
                      , A.CHEM_CADMIUM                      AS Cadmium
                      , A.CHEM_CAESIUM                      AS Caesium
                      , A.CHEM_CALCIUM                      AS Calcium
                      , A.CHEM_CARBON                       AS Carbon
                      , A.CHEM_CERIUM                       AS Cerium
                      , A.CHEM_CHROMIUM                     AS Chromium
                      , A.CHEM_COBALT                       AS Cobalt
                      , A.CHEM_COPPER                       AS Copper
                      , A.CHEM_GALLIUM                      AS Gallium
                      , A.CHEM_GERMANIUM                    AS Germanium
                      , A.CHEM_HYDROGEN                     AS Hydrogen
                      , A.CHEM_INDIUM                       AS Indium
                      , A.CHEM_IRON                         AS Iron
                      , A.CHEM_LANTHANUM                    AS Lanthanum
                      , A.CHEM_LEAD                         AS Lead
                      , A.CHEM_LITHIUM                      AS Lithium
                      , A.CHEM_MAGNESIUM                    AS Magnesium
                      , A.CHEM_MANGANESE                    AS Manganese
                      , A.CHEM_MERCURY                      AS Mercury
                      , A.CHEM_MOLYBDENUM                   AS Molybdenum
                      , A.CHEM_NICKEL                       AS Nickel
                      , A.CHEM_NIOBIUM                      AS Niobium
                      , A.CHEM_NITROGEN                     AS Nitrogen
                      , A.CHEM_OXYGEN                       AS Oxygen
                      , A.CHEM_PHOSPHORUS                   AS Phosphorus
                      , A.CHEM_POTASSIUM                    AS Potassium
                      , A.CHEM_RHENIUM                      AS Rhenium
                      , A.CHEM_SCANDIUM                     AS Scandium
                      , A.CHEM_SELENIUM                     AS Selenium
                      , A.CHEM_SILICON                      AS Silicon
                      , A.CHEM_SILVER                       AS Silver
                      , A.CHEM_SODIUM                       AS Sodium
                      , A.CHEM_STRONTIUM                    AS Strontium
                      , A.CHEM_SULPHUR                      AS Sulphur
                      , A.CHEM_TANTALUM                     AS Tantalum
                      , A.CHEM_TELLURIUM                    AS Tellurium
                      , A.CHEM_THALLIUM                     AS Thallium
                      , A.CHEM_THORIUM                      AS Thorium
                      , A.CHEM_TIN                          AS Tin
                      , A.CHEM_TITANIUM                     AS Titanium
                      , A.CHEM_TUNGSTEN                     AS Tungsten
                      , A.CHEM_URANIUM                      AS Uranium
                      , A.CHEM_VANADIUM                     AS Vanadium
                      , A.CHEM_YTTRIUM                      AS Yttrium
                      , A.CHEM_ZINC                         AS Zinc
                      , A.CHEM_ZIRCONIUM                    AS Zirconium
                  FROM TDB_CHEM_COMP  A 
                  WHERE A.CHEM_COMP_CNT = :ChemCompCnt";

            var result = ctx.GetEntity<TdbChemCompDto>(query, ctx.CreateParameter("ChemCompCnt", ChemCompCnt));

            return result;
        }
    }
}