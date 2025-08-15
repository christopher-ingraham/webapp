import {
    FilterItemFromToDate,
    FilterItemNumber,
    FilterItemString,
} from '@app/shared';

// From Production Strings-List-Management root to HrmJobListComponent
export class HrmJobListFilterFromSlmRoot {
    plannedProductionDate: FilterItemFromToDate;
    stringNumber: FilterItemString;
    productionStatus: FilterItemNumber;

    constructor() {
        this.plannedProductionDate = new FilterItemFromToDate();
        this.stringNumber = new FilterItemString();
        this.productionStatus = new FilterItemNumber();
    }
}
