import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AppError, AppErrorType, BadRequestError } from '../api/errors';

@Injectable({
  providedIn: 'root'
})
export class ErrorDecoderService {

  constructor(private translateService: TranslateService) { }

  decode(error: AppError): string {

    const resourceKey =  `CORE.ERRORS.${AppErrorType[error.type]}`;

    switch (error.type) {

      case AppErrorType.NOT_FOUND:
      case AppErrorType.UNAUTHORIZED:
      case AppErrorType.FORBIDDEN:
      case AppErrorType.API_CALL_FAIL:
      case AppErrorType.INTERNAL_SERVER_ERROR:
        return this.translateService.instant(resourceKey);

      case AppErrorType.BAD_REQUEST: {
        const badRequestError = error as BadRequestError;
        const badRequestErrorResourceKey = `${resourceKey}.${(badRequestError.key || 'UNKNOWN').toUpperCase()}`;

        return this.translateService.instant(badRequestErrorResourceKey, badRequestError.data);
      }
    }

    return this.translateService.instant(`CORE.ERRORS.UNKNOWN`);
  }
}
