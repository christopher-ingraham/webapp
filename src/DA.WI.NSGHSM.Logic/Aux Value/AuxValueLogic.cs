using System.Collections.Generic;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.AuxValue;
using DA.WI.NSGHSM.IRepo.AuxValue;
using DA.WI.NSGHSM.Repo;

namespace DA.WI.NSGHSM.Logic.AuxValue
{

    public class AuxValueLogic
    {
        private IAuxValueRepo<ReportDataSource> auxValueRepo;


        public AuxValueLogic(IAuxValueRepo<ReportDataSource> auxValueRepo)
        {
            this.auxValueRepo = auxValueRepo;
        }

        public ListResultDto<AuxValueListItemDto> SelAuxIntegerValue(ListRequestDto<AuxValueListFilterDto> listRequest)
        {
            return auxValueRepo.SelAuxIntegerValue(listRequest);
        }

        public CustomValidationDTO getCustomValidation(string variableId, Dictionary<KeyValuePair<string, string>, KeyValuePair<string, string>> valueList)
        {
            return auxValueRepo.getRangeValidationDTO(variableId, valueList);
        }

    }
}