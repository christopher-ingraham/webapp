import { ProducedCoilDetail } from './produced-coil-detail.class';

export class OutCoilSetupIntermediateTempDetailDto extends ProducedCoilDetail {
    public targetTempInterm: number;
    public targetTempIntermUpTol: number;
    public targetTempIntermLoTol: number;
}
