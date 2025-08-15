import { AppErrorType } from './app-error-type.enum';

export class AppError extends Error {
    public readonly type: AppErrorType;

    // for reference:
    // Support for new.target pseudo-property as per ES2015 specification
    // https://www.typescriptlang.org/docs/handbook/release-notes/typescript-2-2.html
    constructor(type: AppErrorType, message?: string) {
        // this undefined here

        super(message); // 'Error' breaks prototype chain here
        // this.prototype here is Error, because direct ancestor of AppError is Error
        // --> (this instanceof Error) holds

        // restore prototype chain
        Object.setPrototypeOf(this, new.target.prototype);
        // this.prototype here is again AppError

        this.type = type;
    }
}
