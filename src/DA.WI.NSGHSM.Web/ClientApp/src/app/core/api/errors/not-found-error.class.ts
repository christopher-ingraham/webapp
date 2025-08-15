import { AppError } from './app-error.class';
import { AppErrorType } from './app-error-type.enum';

export class NotFoundError extends AppError {
    constructor(message?: string) {
        super(AppErrorType.NOT_FOUND, message);
    }
}
