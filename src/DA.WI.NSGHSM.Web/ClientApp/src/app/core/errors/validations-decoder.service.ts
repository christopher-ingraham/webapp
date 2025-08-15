import { Injectable } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
    providedIn: 'root'
})
export class ValidationsDecoderService {

    constructor(private translator: TranslateService) { }

    decode(
        fieldPrefix: string,
        fieldName: string,
        errors: ValidationErrors
    ): string {

        const decodedErrors = Object.keys(errors).map((key: string) => {

            const error = errors[key];
            const resourceKey = this.dotNormalize(fieldPrefix, 'VALIDATIONS', fieldName, key);
            const fieldNameTranslation = this.translator.instant(this.dotNormalize(fieldPrefix, 'FIELDS', fieldName));
            const errorParam = Object.assign({ fieldName: fieldNameTranslation }, error);

            const translation = this.translator.instant(resourceKey, errorParam);

            if (translation !== resourceKey) {
                // Translation succesful
                return translation;
            }

            // No translation available, we build an alternative one
            if (key == 'min') {
                return this.translator.instant(key + ' value: ' + errorParam.min);
            } else if (key == 'max') {
                return this.translator.instant(key + ' value: ' + errorParam.max);
            } else {
                return this.translator.instant(key + ' value');
            }
        });

        return decodedErrors.join('\n');
    }

    private dotNormalize(...items: string[]): string {
        return [...items].join('.').toUpperCase();
    }
}
