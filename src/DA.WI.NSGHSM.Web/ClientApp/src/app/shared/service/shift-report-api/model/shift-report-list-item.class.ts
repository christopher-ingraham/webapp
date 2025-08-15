// shift-report-list-item.class.ts
import { Exclude, Type } from 'class-transformer';

import { ShiftReport } from './shift-report.class';

export class ShiftReportListItem extends ShiftReport {
    crewId: string;
    @Type(() => Date)
    currentMonthStart: Date;
    @Type(() => Date)
    currentMonthStop: Date;
    @Type(() => Date)
    filterDate: Date;
    lastPieceNo: number;
    mtdProduced: number;
    optWeight: number;
    productionShiftLabel: string;
    shiftCnt: number;
    @Type(() => Date)
    shiftEndDate: Date;
    shiftId: number;
    @Type(() => Date)
    shiftStartDate: Date;
    totInWeight: number;
    totOutPiecesNum: number;
    totOutWeight: number;
    utilization: number;

    @Exclude()
    public get fiscalMonthStart(): Date {
        return this.currentMonthStart;
    }

    @Exclude()
    public get fiscalMonthStop(): Date {
        return this.currentMonthStop;
    }

    @Exclude()
    public get shiftSlabsWeight(): number {
        return this.totOutWeight;
    }

    @Exclude()
    public get shiftCoilsWeight(): number {
        return this.totInWeight;
    }

    @Exclude()
    public get optimalWeight(): number {
        return this.optWeight;
    }

    @Exclude()
    public get shiftUtilization(): number {
        return this.utilization;
    }

    @Exclude()
    public get lastPieceNumber(): number {
        return this.lastPieceNo;
    }

    @Exclude()
    public get shiftNumberOfCoils(): number {
        return this.totOutPiecesNum;
    }

    @Exclude()
    public get shiftLabel() {
        return this.productionShiftLabel;
    }

    @Exclude()
    public get crewLabel() {
        return this.crewId;
    }

}
