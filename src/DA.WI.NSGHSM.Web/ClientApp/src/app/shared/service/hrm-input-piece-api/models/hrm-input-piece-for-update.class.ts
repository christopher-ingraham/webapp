import { HrmInputPieceBase } from './hrm-input-piece-base.class';

export class HrmInputPieceForUpdate extends HrmInputPieceBase
{
    public destCodeId: number;
    public sourceCodeId: number;
    public heatNo: number;
    public preliminaryThk: number;
    public preliminaryThkHead: number;
    public preliminaryThkTail: number;
    public preliminaryWdt: number;
    public preliminaryWdtHead: number;
    public preliminaryWdtTail: number;
    public preliminaryWdtChg: number;
    public preliminaryLen: number;
    public entryTemp: number;
    public measuredTemp: number;
    public useMeasTemp: number;
    public useMeasWidth: number;
    public useBaseGrade: number;
    public remark: string;
    public gradeGroupId: string;
    public baseGradeId: string;
    public furnaceDischargeTemp: number;
    public measuredWidthHead: number;
    public measuredWidthTail: number;
    public targetExitTemp: number;
    public targetFlatness: number;
    public targetFlatnessNtol: number;
    public targetFlatnessCustomertol: number;
    public targetFlatnessPtol: number;
    public alloySpecCode: string;
    public materialSpecId: number;
    public trialFlag: number;
    public customerId: string;
    public customerName: string;
    public customerContactName: string;
    public carrierMode: string;
    public endUse: string;
    public enduseSurfaceRating: number;
    public areaType: string;
    public targetTempFmCustomertol: number;
    public targetExitTempPtol: number;
    public targetTempDC: number;
    public targetTempDCNtol: number;
    public targetTempDCCustomertol: number;
    public targetTempDCPtol: number;
    public targetProfile: number;
    public targetProfileNtol: number;
    public targetProfileCustomertol: number;
    public targetProfilePtol: number;
    public targetThicknessNtol: number;
    public targetThicknessPtol: number;
    public targetWidthNtol: number;
    public targetWidthPtol: number;
    public targetWeight: number;
    public targetWeightNtol: number;
    public targetWeightPtol: number;
    public targetExitTempNtol: number;
    public targetThickness: number;

    constructor(options?: Partial<HrmInputPieceForUpdate>) {
        super();
        if (options) {
            Object.assign(this, options);
        }
    }

}

