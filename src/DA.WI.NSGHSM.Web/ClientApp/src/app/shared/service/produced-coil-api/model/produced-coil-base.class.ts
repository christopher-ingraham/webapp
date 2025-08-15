import { Type, Transform, Exclude } from 'class-transformer';

// ProducedCoil-base.class.ts
export class ProducedCoilBase {

    public outPieceNo: number;
    public outPieceId: string;  // Produced Coild ID
    public outPieceCnt: number | null;
    public outPieceSeq: number;    // Coil Sequence
    public inPieceNo: number | null;
    public inPieceId: string;  // Input Slab ID
    public jobId: string;   // String No
    @Type(() => Date)
    public productionStartDate: Date;   // Start Production Time
    @Type(() => Date)
    public productionStopDate: Date;    // Stop Production Time
    public calculatedWeight: number;
    public measuredWeight: number;  // Measured Weight
    public length: number;  // Exit Length
    public exitThk: number;  // Exit Thickness
    public targetWidth: number;
    public targetThickness: number;    // Target Cold Thickness
    public innerDiameter: number;   // Internal Diameter
    public outerDiameter: number;    // External Diameter
    public endOfInGotFlag: number;   // Last Ingot Flag
    public testCut: number;     // Test Cut
    public trialFlag: number;    // Trial Coil
    public crewId: string;   // Production Crew ---->  popolare tendina con lookup 'cboCrew' (vedi postman)
    public shiftId: number;     // Production Shift ---->  popolare tendina con lookup 'cboShift' (vedi postman)
    public remark: string | null;  // Note
    public soakTime: number;  // Soak Time
    public status: number;  // Coil Status ---->  popolare tendina con lookup 'cboExCoilStatus' (vedi postman)
    public gapTime: number;   // Gap Time
    public rollingTime: number;   // Rolling Time
    public stripCrownIntolFlag: number;  // [TAB Measured Data] Strip Profile [mm] ---> colonna 5 ("In Tolerance")
    public exitThkIntolFlag: number;     // [TAB Measured Data] Exit Thickness [mm] ---> colonna 5 ("In Tolerance")
    public exitWidthIntolFlag: number;      // [TAB Measured Data] Exit Width [mm] ---> colonna 5 ("In Tolerance")
    public exitTempIntolFlag: number;         // [TAB Measured Data] Finishing Temperature [^C] ---> colonna 5 ("In Tolerance")
    public downcoilTempIntolFlag: number;     // [TAB Measured Data] Coiling Temperature [^C] ---> colonna 5 ("In Tolerance")
    public interstandCoolingBitmask: number;   // Mappatura per campi
    public operator: string;    // Operator
    public exitWidth: number;
    @Type(() => Date)
    public revision: Date;    // Revision
    public dispositionCodesForHold: boolean[];  // TAB 'DISPOSITION DATA'

    @Exclude()
    public get interstandCooling(): boolean[] {
        const bitmask = this.interstandCoolingBitmask;
        // tslint:disable-next-line
        const f1f2 = ((bitmask & 1) ? true : false);
        // tslint:disable-next-line
        const f2f3 = ((bitmask & 2) ? true : false);
        // tslint:disable-next-line
        const f3f4 = ((bitmask & 4) ? true : false);
        // tslint:disable-next-line
        const f4f5 = ((bitmask & 8) ? true : false);
        // tslint:disable-next-line
        const f5f6 = ((bitmask & 16) ? true : false);

        return [f1f2, f2f3, f3f4, f4f5, f5f6];
    }


    public set interstandCooling(value: boolean[]) {
        let bitmask = 0;
        let weight = 1;

        for (let offset = 0; offset < value.length; offset++) {
            bitmask += value[offset] ? weight : 0;
            weight *= 2;
        }

        this.interstandCoolingBitmask = bitmask;
    }
}
