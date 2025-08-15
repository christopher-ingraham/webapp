import { ComboBoxItemNumberString, ChildState } from '@app/shared';

export interface StringsListManagementChildState
    extends ChildState {
    jobStatusList?: ComboBoxItemNumberString[];
    pieceStatusList?: ComboBoxItemNumberString[];
}
