using DA.DB.Utils;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.Production;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.Production
{
    internal class HrmAnalysisDataRepo<TDataSource> : IHrmAnalysisDataRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public HrmAnalysisDataRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<HrmAnalysisDataListItemDto> LoadHrmSampleId(ListRequestDto<HrmAnalysisDataListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = $@" SELECT TRIM(SAMPLE_ID)       AS SampleId,
                                ANALYSIS_CNT                AS AnalysisCnt
                                FROM HRM_ANALYSIS_DATA ";

            string queryCount = $@" SELECT COUNT(SAMPLE_ID)
                  FROM HRM_ANALYSIS_DATA ";

            var searchHeatNo = listRequest.Filter?.SearchHeatNo;
            if (searchHeatNo != null)
            {
                query += "WHERE (HEAT_NO LIKE :SearchHeatNo||'%') ";
                queryCount += "WHERE (HEAT_NO LIKE :SearchHeatNo||'%') ";
                queryParam.Add(ctx.CreateParameter("SearchHeatNo", searchHeatNo));
                queryCountParam.Add(ctx.CreateParameter("SearchHeatNo", searchHeatNo));
            }


            var data = ctx.GetEntities<HrmAnalysisDataListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<HrmAnalysisDataListItemDto>
            {
                Data = data,
                Total = total
            };
        }


        public ListResultDto<HrmAnalysisDataDetailDto> LoadHrmSampleIdList(ListRequestDto<HrmAnalysisDataDetailDto> listRequest, int analysisCnt)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT
                TRIM(SAMPLE_ID)     AS SampleId,
                ANALYSIS_CNT        AS AnalysisCnt,
                AL_ALUMINUM         AS Aluminium,
                AG_SILVER           AS Silver,
                AS_ARSENIC          AS Arsenic,
                B_BORON             AS Boron,
                BA_BARIUM           AS Barium,
                BE_BERYLLIUM        AS Beryllium,
                BI_BISMUTH          AS Bismuth,
                C_CARBON            AS Carbon,
                CA_CALCIUM          AS Calcium,
                CD_CADMIUM          AS Cadmium,
                CE_CERIUM           AS Cerium,
                CO_COBALT           AS Cobalt,
                CR_CHROMIUM         AS Chromium,
                CS_CAESIUM          AS Caesium,
                CU_COPPER           AS Copper,
                FE_IRON             AS Iron,
                GA_GALLIUM          AS Gallium,
                GE_GERMANIUM        AS Germanium,
                H_HYDROGEN          AS Hydrogen,
                HG_MERCURY          AS Mercury,
                IN_INDIUM           AS Indium,
                K_POTASSIUM         AS Potassium,
                LA_LANTHANUM        AS Lanthanum,
                LI_LITHIUM          AS Lithium,
                MG_MAGNESIUM        AS Magnesium,
                MN_MANGANESE        AS Manganese,
                MO_MOLYBDENUM       AS Molybdenum,
                N_NITROGEN          AS Nitrogen,
                NA_SODIUM           AS Sodium,
                NB_NIOBIUM          AS Niobium,
                NI_NICKEL           AS Nickel,
                O_OXYGEN            AS Oxygen,
                P_PHOSPHOROUS       AS Phosphorus,
                PB_LEAD             AS Lead,
                RE_RHENIUM          AS Rhenium,
                S_SULPHUR           AS Sulphur,
                SB_ANTIMONY         AS Antimony,
                SC_SCANDIUM         AS Scandium,
                SE_SELENIUM         AS Selenium,
                SI_SILICON          AS Silicon,
                SN_TIN              AS Tin,
                SR_STRONTIUM        AS Strontium,
                TA_TANTALUM         AS Tantalum,
                TE_TELLURIUM        AS Tellurium,
                TH_THORIUM          AS Thorium,
                TI_TITANIUM         AS Titanium,
                TL_THALLIUM         AS Thallium,
                U_URANIUM           AS Uranium,
                V_VANADIUM          AS Vanadium,
                W_TUNGSTEN          AS Tungsten,
                Y_YTTRIUM           AS Yttrium,
                ZN_ZINC             AS Zinc,
                ZR_ZIRCONIUM        AS Zirconium,
                AL_ALUMINIUM_SOL    AS AluminiumSol,
                AL_ALUMINIUM_TOT    AS AluminiumTot,
                TI_TITANIUM_EFF     AS TitaniumEff
            FROM
                HRM_ANALYSIS_DATA 
            WHERE ANALYSIS_CNT = :AnalysisCnt ";

            string queryCount = $@" SELECT COUNT(SAMPLE_ID)
                  FROM HRM_ANALYSIS_DATA
                  WHERE ANALYSIS_CNT = :AnalysisCnt ";


            var data = ctx.GetEntities<HrmAnalysisDataDetailDto>(
                            ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0),
                            Convert.ToInt32(listRequest.Page?.Skip ?? 0)),
                            ctx.CreateParameter("AnalysisCnt", analysisCnt)
                        ).ToArray();

            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<HrmAnalysisDataDetailDto>
            {
                Data = data,
                Total = total
            };

        }

        public HrmAnalysisDataDetailDto Read(int id)
        {
            string query = @"SELECT
                TRIM(SAMPLE_ID)     AS SampleId,
                ANALYSIS_CNT        AS AnalysisCnt,
                AL_ALUMINUM         AS Aluminium,
                AG_SILVER           AS Silver,
                AS_ARSENIC          AS Arsenic,
                B_BORON             AS Boron,
                BA_BARIUM           AS Barium,
                BE_BERYLLIUM        AS Beryllium,
                BI_BISMUTH          AS Bismuth,
                C_CARBON            AS Carbon,
                CA_CALCIUM          AS Calcium,
                CD_CADMIUM          AS Cadmium,
                CE_CERIUM           AS Cerium,
                CO_COBALT           AS Cobalt,
                CR_CHROMIUM         AS Chromium,
                CS_CAESIUM          AS Caesium,
                CU_COPPER           AS Copper,
                FE_IRON             AS Iron,
                GA_GALLIUM          AS Gallium,
                GE_GERMANIUM        AS Germanium,
                H_HYDROGEN          AS Hydrogen,
                HG_MERCURY          AS Mercury,
                IN_INDIUM           AS Indium,
                K_POTASSIUM         AS Potassium,
                LA_LANTHANUM        AS Lanthanum,
                LI_LITHIUM          AS Lithium,
                MG_MAGNESIUM        AS Magnesium,
                MN_MANGANESE        AS Manganese,
                MO_MOLYBDENUM       AS Molybdenum,
                N_NITROGEN          AS Nitrogen,
                NA_SODIUM           AS Sodium,
                NB_NIOBIUM          AS Niobium,
                NI_NICKEL           AS Nickel,
                O_OXYGEN            AS Oxygen,
                P_PHOSPHOROUS       AS Phosphorus,
                PB_LEAD             AS Lead,
                RE_RHENIUM          AS Rhenium,
                S_SULPHUR           AS Sulphur,
                SB_ANTIMONY         AS Antimony,
                SC_SCANDIUM         AS Scandium,
                SE_SELENIUM         AS Selenium,
                SI_SILICON          AS Silicon,
                SN_TIN              AS Tin,
                SR_STRONTIUM        AS Strontium,
                TA_TANTALUM         AS Tantalum,
                TE_TELLURIUM        AS Tellurium,
                TH_THORIUM          AS Thorium,
                TI_TITANIUM         AS Titanium,
                TL_THALLIUM         AS Thallium,
                U_URANIUM           AS Uranium,
                V_VANADIUM          AS Vanadium,
                W_TUNGSTEN          AS Tungsten,
                Y_YTTRIUM           AS Yttrium,
                ZN_ZINC             AS Zinc,
                ZR_ZIRCONIUM        AS Zirconium,
                AL_ALUMINIUM_SOL    AS AluminiumSol,
                AL_ALUMINIUM_TOT    AS AluminiumTot,
                TI_TITANIUM_EFF     AS TitaniumEff
            FROM
                HRM_ANALYSIS_DATA 
            WHERE ANALYSIS_CNT = :AnalysisCnt ";

            var data = ctx.GetEntity<HrmAnalysisDataDetailDto>(query, ctx.CreateParameter("AnalysisCnt", id));

            return data;
        }

    }
}
