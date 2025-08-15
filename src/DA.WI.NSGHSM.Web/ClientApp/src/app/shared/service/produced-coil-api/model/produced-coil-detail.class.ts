// ProducedCoil-detail.class.ts
import { ProducedCoil } from './produced-coil.class';
import { InputCoilTrgMeasDetailDto } from './produced-coil-InputCoilTrgMeasDetailDto.class';
import { OutCoilSetupIntermediateTempDetailDto } from './produced-coil-OutCoilSetupIntermediateTempDetailDto.class';
import { RollDataForStands } from './produced-coil-RollDataForStandsDto.class';
import { CurrentOutCoilMeas } from './produced-coil-CurrentOutCoilMeas.class';

export class ProducedCoilDetail extends ProducedCoil {
    // TODO
    public jobPieceSeq: string;  // Line Piece No
    public entryThickness: number;
    public orderNumber: string;  // Customer Order No
    public orderPosition: string;   // Order Line No
    public customerId: string;   // Customer ID
    public inputCoilTrgMeas: InputCoilTrgMeasDetailDto;   // TAB 'MEASURED DATA'
    public outCoilSetupIntermediateTemp: OutCoilSetupIntermediateTempDetailDto;  // TAB 'MEASURED DATA'  (Intermediate temperature)
    public rollDataForStands: RollDataForStands;  // TAB 'ROLLS DATA'
    public currentOutCoilMeas: CurrentOutCoilMeas[];  // TAB 'MEASURED DATA' ("Tabella" di destra")
    public entryLength: number;  // Entry Length
    public totalReduction: number;   // Total Reduction
    public heatId: string;  // Heat No
    public materialGradeId: string;    // Material Grade ID
    public gradeGroupId: string;   // Grade Group ID
    public targetColdWidth: number;   // Target Cold Width
    public exitWidth: number;   // Exit Width
    public targetColdThk: number;
    public exitTemp: number;
    public entryHeadWidth: number;     // Entry Head Width
    public entryHeadThickness: number;     // Entry Head Thickness
    public usedDefaultChemComp: number;   // Default Chemistry Used
}
