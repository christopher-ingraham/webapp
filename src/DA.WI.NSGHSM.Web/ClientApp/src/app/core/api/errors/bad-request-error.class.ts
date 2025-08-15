import { AppError } from './app-error.class';
import { AppErrorType } from './app-error-type.enum';

export class BadRequestError extends AppError {
    constructor(key: string, data?: any, message?: string) {
        super(AppErrorType.BAD_REQUEST, message);

        this.key = key;
        this.data = data;
    }

    public readonly key: string;
    public readonly data: any;
}
