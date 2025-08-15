// ProducedCoil-for-insert.class.ts
import { ProducedCoilBase } from './produced-coil-base.class';
import { NumericFilterCellComponent } from '@progress/kendo-angular-grid';

export class ProducedCoilForInsert extends ProducedCoilBase {
    public outPieceArea: string;
    public destCodeId: number;
    public combinationNo: number;
    public stripCrownTolPerc: number;   // [TAB Measured Data] Strip Profile [mm] ---> colonna 4 ("In Tol[%]")
    public exitThkTolPerc: number;      // [TAB Measured Data] Exit Thickness [mm] ---> colonna 4 ("In Tol[%]")
    public exitWidthTolPerc: number;  // [TAB Measured Data] Exit Width [mm] ---> colonna 4 ("In Tol[%]")
    public exitTempTolPerc: number;     // [TAB Measured Data] Finishing Temperature [^C] ---> colonna 4 ("In Tol[%]")
    public downcoilTempTolPerc: number;   // [TAB Measured Data] Coiling Temperature [^C] ---> colonna 4 ("In Tol[%]")


    constructor(options?: Partial<ProducedCoilForInsert>) {
        super();
        if (options) {
            Object.assign(this, options);
        }
    }
}
