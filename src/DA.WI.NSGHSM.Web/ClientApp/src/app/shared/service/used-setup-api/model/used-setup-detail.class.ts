// used-setup-detail.class.ts
import { UsedSetup } from './used-setup.class';
import { UsedAllStepsListItem } from './used-setup-usedAllStepsListItem.class';
import { Type } from 'class-transformer';

export class UsedSetupDetail extends UsedSetup {

    public areaId: string;
    public centerId: string;     // Center ID
    public inPieceNo: number;
    public inPieceId: string;      // Input Piece ID
    public heatId: string;
    public jobId: string;
    public practiceId: string;      // Practice ID
    public millMode: number;           // Mill Mode  --->  Vedi lookups ---> cboMillMode  (Postman)
    public disabledStandBitmask: number;
    public materialGradeId: string;     // Material Grade ID
    public gradeGroupLabel: string;     // Grade Groud ID
    public entryWdt: number;              // Entry Width
    public entryThk: number;
    public entryTemp: number;       // Entry Temperature
    public pieceLength: number;        // Entry Length
    public pieceWeight: number;        // Entry Weight
    public density: number;            // Density
    public thermalExpCoeff: number;     //  Thermal Exp Coeff
    public transferBarThk: number;        // Thickness  (Transfer Bar)
    public transferBarWdt: number;        // Width  (Transfer Bar)
    public transferBarTemp: number;       // Temperature  (Transfer Bar)
    public targetWdt: number;          // Target Cold Width
    public targetWdtUpTol: number;
    public targetWdtLoTol: number;
    public targetThk: number;         // Target Cold Thickness
    public targetThkUpTol: number;
    public targetThkLoTol: number;
    public descalerMode: number;         // Descaler Mode
    public descalerHeadOffset: number;   // Descaler Head Offset Delay          [Ancillary Setup]
    public icHeadOffset: number;         // Intensive Cooling Head Offset       [Ancillary Setup]
    public setupMode: number;
    public setupModelLabel: string;
    public operator: string;         //  Operator
    @Type(() => Date)
    public revision: Date;
    public generalSettings: UsedAllStepsListItem [];    //  TAB [General Data] + alcuni [Ancillary Setup]
}
