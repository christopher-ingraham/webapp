import { AppError } from './app-error.class';
import { AppErrorType } from './app-error-type.enum';

export class InternalServerError extends AppError {
    // for reference:
    // Support for new.target
    // https://www.typescriptlang.org/docs/handbook/release-notes/typescript-2-2.html
    constructor(message?: string) {
        super(AppErrorType.INTERNAL_SERVER_ERROR, message);
    }
}
