// coil-general-report-list-item.class.ts
import { Type, Exclude } from 'class-transformer';

import { CoilGeneralReport } from './coil-general-report.class';

export class CoilGeneralReportListItem extends CoilGeneralReport {
    public calculatedWeight: number;
    public crewId: string;
    public customerId: string;
    public entryThk: number;
    public exitThk: number;
    public exitWidth: number;
    public externalDiameter: number;
    @Type(() => Date)
    public filterDate: Date;
    public heatId: string;
    public inPieceId: string;
    public inPieceNo: number;
    public jobId: string;
    public length: number;
    public materialGradeId: string;
    public measuredWeight: number;
    public nominalWidth: number;
    public outPieceId: string;
    public outPieceNo: number;
    public prodCoilNum: number;
    public productionShiftLabel: string;
    @Type(() => Date)
    public productionStartDate: Date;
    @Type(() => Date)
    public productionStopDate: Date;
    public shiftId: number;
    public status: number;
    public targetColdThickness: number;
    public targetColdWidth: number;
    public targetThickness: number;
    public trialFlag: string;
    public weight: number;

    @Exclude()
    public get producedPieceId() {
        return this.outPieceId;
    }

    @Exclude()
    public get inputSlabNo() {
        return this.inPieceNo;
    }

    @Exclude()
    public get heatNo() {
        return this.heatId;
    }

    @Exclude()
    public get steelGradeId() {
        return this.materialGradeId;
    }

    @Exclude()
    public get exitThickness() {
        return this.exitThk;
    }

    @Exclude()
    public get entryThickness() {
        return this.entryThk;
    }

    @Exclude()
    public get delayTime() {
        // FIXME which field?
        return new Date();
    }
}
