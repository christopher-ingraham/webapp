import { RollDataForStands } from './produced-coil-RollDataForStandsDto.class';

export class RepHmRollDetail extends RollDataForStands {
    public areaId: string;
    public centerId: string;
    public standNo: number;
    public wrLoId: string;      // WR Button Roll
    public wrLoDiameter: number;
    public wrLoRolledLen: number;
    public wrUpId: string;       // WR Top Roll ID
    public wrUpDiameter: number;
    public wrUpRolledLen: number;   // Rolled Length
    public brLoId: string;      // BUR Button Roll
    public brLoDiameter: number;
    public brLoRolledLen: number;
    public brUpId: string;      // BUR Top Roll ID
    public brUpDiameter: number;
    public brUpRolledLen: number;
}
