// rep-hm-setup.class.ts
import { Type } from 'class-transformer';

import { RepHmSetupBase } from './rep-hm-setup-base.class';
import { RepHmSetupAllStepsListItem } from './rep-hm-setup-all-steps-list-item.class';

export class RepHmSetup extends RepHmSetupBase {
    public areaId: string;
    public centerId: string;                // Center ID
    public inPieceNo: number;
    public inPieceId: string;               // Input Piece ID
    public heatId: string;
    public jobId: string;
    public practiceId: string;              // Practice ID
    public millMode: number;                // Mill Mode  --->  Vedi lookups ---> cboMillMode  (Postman)
    public disabledStandBitmask: number;
    public materialGradeId: string;         // Material Grade ID
    public gradeGroupLabel: string;         // Grade Groud ID
    public entryWdt: number;                // Entry Width
    public entryThk: number;
    public entryTemp: number;               // Entry Temperature
    public pieceLength: number;             // Entry Length
    public pieceWeight: number;             // Entry Weight
    public density: number;                 // Density
    public thermalExpCoeff: number;         //  Thermal Exp Coeff
    public transferBarThk: number;          // Thickness  (Transfer Bar)
    public transferBarWdt: number;          // Width  (Transfer Bar)
    public transferBarTemp: number;         // Temperature  (Transfer Bar)
    public targetWdt: number;               // Target Cold Width
    public targetWdtUpTol: number;
    public targetWdtLoTol: number;
    public targetThk: number;               // Target Cold Thickness
    public targetThkUpTol: number;
    public targetThkLoTol: number;
    public descalerMode: number;            // Descaler Mode --->  Vedi lookups ---> cboDescalerMode  (Postman)   [Ancillary Setup]
    public descalerHeadOffset: number;      // Descaler Head Offset Delay          [Ancillary Setup]
    public icHeadOffset: number;            // Intensive Cooling Head Offset       [Ancillary Setup]
    public setupMode: number;
    public setupModelLabel: string;
    public operator: string;                //  Operator
    @Type(() => Date)
    public revision: Date;                  // Revision
    @Type(() => RepHmSetupAllStepsListItem)
    public generalSettings: RepHmSetupAllStepsListItem[];    //  TAB [General Data] + alcuni [Ancillary Setup]
}
