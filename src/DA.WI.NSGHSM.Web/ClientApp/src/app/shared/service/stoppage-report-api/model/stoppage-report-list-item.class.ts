// stoppage-report-list-item.class.ts
import { StoppageReport } from './stoppage-report.class';
import { Type, Exclude } from 'class-transformer';

export class StoppageReportListItem extends StoppageReport {
    public crewId: string;
    public delayTime: string;
    public duration_HHMM: string;
    @Type(() => Date)
    public filterDate: Date;
    public productionShiftLabel: string;
    public shiftId: number;
    @Type(() => Date)
    public startDelay: Date;
    public stpCounter: number;
    public stpGroupLabel: string;
    public stpReasonLabel: string;

    @Exclude()
    public get stoppageBeginDate() {
        return this.startDelay;
    }

    @Exclude()
    public get stoppageEndDate() {
        return new Date(); // FIXME
    }

    @Exclude()
    public get duration() {
        return this.duration_HHMM;
    }

    @Exclude()
    public get stoppageType() {
        return this.delayTime;
    }

    @Exclude()
    public get stoppageGroup() {
        return this.stpGroupLabel;
    }

    @Exclude()
    public get stoppageReason() {
        return this.stpReasonLabel;
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
