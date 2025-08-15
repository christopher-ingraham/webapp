import {
    FilterItemFromToDate,
    FilterItemFromToNumber,
    FilterItemNumber,
    FilterItemString,
} from '@app/shared';

// From Production Input-Slabs-List-Management root to HrmJobListComponent
export class HrmInputPieceListFilterFromIslmRoot {
    public creationDateTime: FilterItemFromToDate;
    public slabNumber: FilterItemString;
    public heatNumber: FilterItemString;
    public stringNumber: FilterItemString;
    public customerOrderNumber: FilterItemString;
    public customerName: FilterItemString;
    public pieceStatus: FilterItemNumber;
    public productionStatus: FilterItemFromToNumber;

    constructor() {
        this.creationDateTime = new FilterItemFromToDate();
        this.slabNumber = new FilterItemString();
        this.heatNumber = new FilterItemString();
        this.stringNumber = new FilterItemString();
        this.customerOrderNumber = new FilterItemString();
        this.customerName = new FilterItemString();
        this.pieceStatus = new FilterItemNumber();
        this.productionStatus = new FilterItemFromToNumber();
    }
}
