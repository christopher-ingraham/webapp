import {
    FilterItemFromToDate,
    FilterItemNumber,
    FilterItemString,
} from '@app/shared';

// From Produced Coils Management root to ProducedCoilListComponent
export class ProducedCoilListFilterFromPcmRoot {
    productionStopDate: FilterItemFromToDate;
    producedPieceId: FilterItemString;
    inputSlabNumber: FilterItemString;
    heatNumber: FilterItemString;
    coilStatus: FilterItemNumber;

    constructor() {
        this.productionStopDate = new FilterItemFromToDate();
        this.producedPieceId = new FilterItemString();
        this.inputSlabNumber = new FilterItemString();
        this.heatNumber = new FilterItemString();
        this.coilStatus = new FilterItemNumber();
    }
}
