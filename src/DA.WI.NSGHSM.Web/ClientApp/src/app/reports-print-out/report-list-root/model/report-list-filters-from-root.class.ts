import {
    FilterItemFromToDate,
    FilterItemFromToNumber,
    FilterItemNumber,
    FilterItemString,
} from '@app/shared';

import { ReportType } from './report-type.enum';


export class ReportListFiltersFromRoot {
    // Union of all filters in all reports
    public readonly centerId: FilterItemString;
    public readonly coilStatus: FilterItemNumber;
    public readonly endDelayDate: FilterItemFromToDate;
    public readonly entryThickness: FilterItemFromToNumber;
    public readonly exitThickness: FilterItemFromToNumber;
    public readonly exitWidth: FilterItemFromToNumber;
    public readonly heatNumber: FilterItemString;
    public readonly inputSlabNumber: FilterItemString;
    public readonly materialGradeId: FilterItemString;
    public readonly millMode: FilterItemNumber;
    public readonly practiceId: FilterItemString;
    public readonly producedPieceId: FilterItemString;
    public readonly productionStopDate: FilterItemFromToDate;
    public readonly stoppageDate: FilterItemFromToDate;
    public readonly revisionDate: FilterItemFromToDate;
    public readonly shiftDate: FilterItemFromToDate;
    public readonly shiftId: FilterItemNumber;
    public readonly shiftLabel: FilterItemString;

    // Selective constructor: create instances for a specific report's
    // filters (all remaining undefined).
    private constructor(reportType: ReportType) {
        switch (reportType) {
            case ReportType.CoilGeneralReport:
                this.productionStopDate = new FilterItemFromToDate();
                this.producedPieceId = new FilterItemString();
                this.inputSlabNumber = new FilterItemString();
                this.heatNumber = new FilterItemString();
                this.materialGradeId = new FilterItemString();
                this.coilStatus = new FilterItemNumber;
                this.exitThickness = new FilterItemFromToNumber();
                this.exitWidth = new FilterItemFromToNumber();
                this.shiftId = new FilterItemNumber();
                break;
            case ReportType.TimeOrientedReport:
                this.productionStopDate = new FilterItemFromToDate();
                this.shiftId = new FilterItemNumber();
                this.shiftLabel = new FilterItemString();
                break;
            case ReportType.ShiftReport:
                this.shiftDate = new FilterItemFromToDate();
                this.shiftId = new FilterItemNumber();
                break;
            case ReportType.CobbleReport:
                this.productionStopDate = new FilterItemFromToDate();
                this.producedPieceId = new FilterItemString();
                this.inputSlabNumber = new FilterItemString();
                this.heatNumber = new FilterItemString();
                this.materialGradeId = new FilterItemString();
                this.coilStatus = new FilterItemNumber;
                this.exitThickness = new FilterItemFromToNumber();
                this.exitWidth = new FilterItemFromToNumber();
                this.shiftId = new FilterItemNumber();
                break;
            case ReportType.StoppageReport:
                this.stoppageDate = new FilterItemFromToDate();
                this.shiftId = new FilterItemNumber();
                this.shiftLabel = new FilterItemString();
                break;
            case ReportType.PracticeReport:
                this.centerId = new FilterItemString();
                this.practiceId = new FilterItemString();
                this.millMode = new FilterItemNumber();
                this.materialGradeId = new FilterItemString();
                this.entryThickness = new FilterItemFromToNumber();
                this.revisionDate = new FilterItemFromToDate();
                break;
            default:
                throw new Error(`unknown ReportType ${reportType}`);
        }
    }

    public static newForCoilGeneralReport(): ReportListFiltersFromRoot {
        return new ReportListFiltersFromRoot(ReportType.CoilGeneralReport);
    }
    public static newForTimeOrientedReport(): ReportListFiltersFromRoot {
        return new ReportListFiltersFromRoot(ReportType.TimeOrientedReport);
    }
    public static newForShiftReport(): ReportListFiltersFromRoot {
        return new ReportListFiltersFromRoot(ReportType.ShiftReport);
    }
    public static newForCobbleReport(): ReportListFiltersFromRoot {
        return new ReportListFiltersFromRoot(ReportType.CobbleReport);
    }
    public static newForStoppageReport(): ReportListFiltersFromRoot {
        return new ReportListFiltersFromRoot(ReportType.StoppageReport);
    }
    public static newForPracticeReport(): ReportListFiltersFromRoot {
        return new ReportListFiltersFromRoot(ReportType.PracticeReport);
    }
}
