import { DialogAction } from '@progress/kendo-angular-dialog';
import { ConfirmResultType } from './confirm-result-type.enum';

export class ConfirmResult {

    constructor(
        public readonly type: ConfirmResultType,
        public readonly action?: DialogAction,
        public readonly error?: Error,
    ) {}

    public static createAction(action: DialogAction) {
        return new ConfirmResult(ConfirmResultType.action, action);
    }
    public static createClose() {
        return new ConfirmResult(ConfirmResultType.close);
    }
    public static createError(error: Error) {
        return new ConfirmResult(ConfirmResultType.error, undefined, error);
    }

    public get isAction(): boolean {
        return (this.type === ConfirmResultType.action);
    }
    public get isClose(): boolean {
        return (this.type === ConfirmResultType.close);
    }
    public get isError(): boolean {
        return (this.type === ConfirmResultType.error);
    }
}
