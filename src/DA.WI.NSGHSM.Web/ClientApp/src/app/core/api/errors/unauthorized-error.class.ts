import { AppError } from './app-error.class';
import { AppErrorType } from './app-error-type.enum';

export class UnauthorizedError extends AppError {
    constructor(message?: string) {
        super(AppErrorType.UNAUTHORIZED, message);
    }
}
