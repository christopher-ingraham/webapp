import { HrmInputPieceDetail } from '../service/hrm-input-piece-api/models/hrm-input-piece-detail.class';
import { HrmJobDetail } from '../service/hrm-job-api/models/hrm-job-detail.class';
import { ProducedCoilDetail } from '../service/produced-coil-api/model/produced-coil-detail.class';
import { UsedSetupDetail } from '../service/used-setup-api/model/used-setup-detail.class';

const MasterType = 'master';
const HrmJobDetailType = HrmJobDetail['name'];
const HrmInputPieceDetailType = HrmInputPieceDetail['name'];
const ProducedCoilDetailType = ProducedCoilDetail['name'];
const UsedSetupDetailType = UsedSetupDetail['name'];

export type TabstripTabType =
    typeof MasterType |
    typeof HrmJobDetailType |
    typeof HrmInputPieceDetailType |
    typeof ProducedCoilDetailType |
    typeof UsedSetupDetailType;

export class TabstripTabOptions<TEntityId> {
    disabled?: boolean;
    title?: string;
    type?: TabstripTabType;
    id?: TEntityId;
    isNew?: boolean;
    copyFromId?: any;

    constructor(options?: Partial<TabstripTabOptions<TEntityId>>) {
        this.disabled = false;
        this.title = 'No title';
        this.type = MasterType;
        this.isNew = false;
        if (options) {
            Object.assign(this, options);
        }
    }
}
