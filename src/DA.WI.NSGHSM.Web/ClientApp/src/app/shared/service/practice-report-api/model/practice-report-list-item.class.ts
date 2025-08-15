// practice-report-list-item.class.ts
import { Type, Exclude } from 'class-transformer';

import { PracticeReport } from './practice-report.class';

export class PracticeReportListItem extends PracticeReport {
    public centerId: string;
    public density: number;
    public entryTemp: number;
    public entryThk: number;
    public entryWdt: number;
    @Type(() => Date)
    public filterDate: Date;
    public gradeGroupId: number;
    public gradeGroupLabel: string;
    public materialGradeId: string;
    public millMode: number;
    public millModeLabel: string;
    public operator: string;
    public pieceLength: number;
    public practiceId: string;
    @Type(() => Date)
    public revision: Date;
    public targetColdThk: number;
    public targetColdWdt: number;
    public targetThk: number;
    public targetWdt: number;
    public nDisabledStand: number;

    @Exclude()
    public get steelGradeId() {
        return this.materialGradeId;
    }

    @Exclude()
    public get entryThickness() {
        return this.entryThk;
    }

    @Exclude()
    public get targetThickness() {
        return this.targetThk;
    }

    @Exclude()
    public get entryWidth() {
        return this.entryWdt;
    }

    @Exclude()
    public get entryTemperature() {
        return this.entryTemp;
    }

    @Exclude()
    public get targetColdWidth() {
        return this.targetColdWdt;
    }

    @Exclude()
    public get targetColdThickness() {
        return this.targetColdThk;
    }

}
