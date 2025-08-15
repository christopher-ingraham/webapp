import { AppError } from './app-error.class';
import { AppErrorType } from './app-error-type.enum';

export class ApiCallFailError extends AppError {
    constructor(message?: string) {
        super(AppErrorType.API_CALL_FAIL, message);
    }
}
