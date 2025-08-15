using DA.DB.Utils;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.AuxValue;
using DA.WI.NSGHSM.Dto.Production;
using DA.WI.NSGHSM.IRepo.AuxValue;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DA.WI.NSGHSM.Repo.AuxValue
{
    internal class AuxValueRepo<TDataSource> : IAuxValueRepo<TDataSource>
          where TDataSource : DataSource, new()
    {
        private IDbClient ctx;

        public AuxValueRepo(TDataSource dataSource)
        {
            this.ctx = dataSource.GetDbClient();
        }

        public ListResultDto<AuxValueListItemDto> SelAuxIntegerValue(ListRequestDto<AuxValueListFilterDto> listRequest)
        {
            List<IDbDataParameter> queryParam = new List<IDbDataParameter>();
            List<IDbDataParameter> queryCountParam = new List<IDbDataParameter>();

            string query = @"SELECT
                            RTRIM(CAST(INTEGER_VALUE AS CHAR(10))) || ': ' || RTRIM(VALUE_LABEL) AS ValueLabel
                          , INTEGER_VALUE           AS IntegerValue
                          , RTRIM(CHAR_VALUE)       AS CharValue
                          , RTRIM(VALUE_NAME)       AS ValueName
                            FROM AUX_VALUE ";


            string queryCount = @"SELECT COUNT(VARIABLE_ID) 
                                  FROM AUX_VALUE ";


            var searchVariableId = listRequest.Filter?.SearchVariableId;
            if (searchVariableId.IsNullOrEmpty() == false && searchVariableId == "AREA_TYPE")
            {
                query += @"WHERE (VARIABLE_ID = rpad(:SearchVariableId, 32, ' '))
                        AND VALUE_TYPE  > 0 ";
                queryCount += @"WHERE (VARIABLE_ID = rpad(:SearchVariableId, 32, ' '))
                        AND VALUE_TYPE  > 0  ";
                queryParam.Add(ctx.CreateParameter("SearchVariableId", searchVariableId));
                queryCountParam.Add(ctx.CreateParameter("SearchVariableId", searchVariableId));
            } else {
                query += "WHERE VALUE_TYPE = 10 AND (VARIABLE_ID = rpad(:SearchVariableId, 32, ' ')) ";
                queryCount += "WHERE VALUE_TYPE = 10 AND (VARIABLE_ID = rpad(:SearchVariableId, 32, ' ')) ";
                queryParam.Add(ctx.CreateParameter("SearchVariableId", searchVariableId));
                queryCountParam.Add(ctx.CreateParameter("SearchVariableId", searchVariableId));
            }

            query += "ORDER BY  VALUE_SEQ ASC ";

            var data = ctx.GetEntities<AuxValueListItemDto>(ctx.PaginateSqlStatement(query, Convert.ToInt32(listRequest.Page?.Take ?? 0), Convert.ToInt32(listRequest.Page?.Skip ?? 0)), queryParam.ToArray()).ToArray();
            var total = ctx.GetEntity<long>(queryCount, queryCountParam.ToArray());

            return new ListResultDto<AuxValueListItemDto>
            {
                Data = data,
                Total = total
            };
        }

        public CustomValidationDTO getRangeValidationDTO(string variableId, Dictionary<KeyValuePair<string, string>, KeyValuePair<string, string>> fieldDtoMap){
            variableId = variableId.PadRight(32, ' ');  
            Dictionary<string, ValidationRangeDTO> range = new Dictionary<string, ValidationRangeDTO>();

            int i = 0;
            foreach (KeyValuePair<KeyValuePair<string, string>, KeyValuePair<string, string>> entry in fieldDtoMap)
            {

                string value = entry.Key.Value.PadRight(80, ' ');  

                string query = $@"SELECT
                          TRIM(VALUE_NAME)    AS valueName
                        , MIN_VALUE           AS min
                        , MAX_VALUE           AS max
                  FROM    AUX_VALUE
                  WHERE   VARIABLE_ID = :VariableId
                  AND     VALUE_NAME = :Value ";

                var result = ctx.GetEntity<ValidationRangeDTO>(query, ctx.CreateParameter("VariableId", variableId),
                                                                      ctx.CreateParameter("Value", value));

                result.unitSI = entry.Value.Key;
                 result.unitUSCS = entry.Value.Value;

                range.Add(entry.Key.Key, result);
                i++;
            }

            CustomValidationDTO validationDto = new CustomValidationDTO();

            validationDto.rangeValidation = range;
            
            return validationDto;

        }
    }
}