import { ProducedCoilDetail } from './produced-coil-detail.class';

export class InputCoilTrgMeasDetailDto extends ProducedCoilDetail {
    public targetWidth: number;
    public targetWidthPtol: number;
    public targetWidthNtol: number;
    public targetThickness: number;
    public targetThicknessPtol: number;
    public targetThicknessNtol: number;
    public targetTempFm: number;
    public targetTempFmPtol: number;
    public targetTempFmNtol: number;
    public targetTempDc: number;
    public targetTempDcPtol: number;
    public targetTempDcNtol: number;
    public targetProfile: number;
    public targetProfilePtol: number;
    public targetProfileNtol: number;
}
