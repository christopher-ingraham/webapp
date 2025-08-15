// ProducedCoil-for-update.class.ts
import { ProducedCoilBase } from './produced-coil-base.class';

export class ProducedCoilForUpdate extends ProducedCoilBase {

    constructor(options?: Partial<ProducedCoilForUpdate>) {
        super();
        if (options) {
            Object.assign(this, options);
        }
    }
}
