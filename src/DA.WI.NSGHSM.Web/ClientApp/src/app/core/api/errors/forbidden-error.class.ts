import { AppError } from './app-error.class';
import { AppErrorType } from './app-error-type.enum';

export class ForbiddenError extends AppError {
    constructor(message?: string) {
        super(AppErrorType.FORBIDDEN, message);
    }
}
