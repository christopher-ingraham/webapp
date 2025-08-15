import { ProducedCoilDetail } from './produced-coil-detail.class';
import { RepHmRollDetail } from './produced-coil-RepHmRollDetail.class';

export class RollDataForStands extends ProducedCoilDetail {
    public stands: RepHmRollDetail[]; // RepHmRollDetailDto[];
}
