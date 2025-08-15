import { Injectable, ErrorHandler, Injector } from '@angular/core';

import { environment } from '../../../environments/environment';


import { LogService } from '../log';
import { NotifierService } from '../notifier';

import { ErrorDecoderService } from './error-decoder.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
    private _log: LogService;
    private get log(): LogService {
        if (!this._log) {
            this._log = this.injector.get(LogService);
        }
        return this._log;
    }

    private _notifier: NotifierService;
    private get notifier(): NotifierService {
        if (!this._notifier) {
            this._notifier = this.injector.get(NotifierService);
        }
        return this._notifier;
    }

    private _errorDecoder: ErrorDecoderService;
    private get errorDecoder(): ErrorDecoderService {
        if (!this._errorDecoder) {
            this._errorDecoder = this.injector.get(ErrorDecoderService);
        }
        return this._errorDecoder;
    }

    constructor(private injector: Injector) { }

    handleError(error: any): void {

        const decodedError: string = this.errorDecoder.decode(error);

        this.log.error(error);

        if (!environment.production && (error instanceof Error)) {
            // tslint:disable-next-line
            console.log(error.stack);
        }

        this.notifier.error(decodedError);
    }
}
