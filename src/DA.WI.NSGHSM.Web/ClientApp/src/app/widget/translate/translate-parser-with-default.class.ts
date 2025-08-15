import { TranslateDefaultParser } from '@ngx-translate/core';

import { LogService } from '@app/core';

import { TranslateHelperService } from '../helpers';

export class TranslateParserWithDefault extends TranslateDefaultParser {
    constructor(
        // private log: LogService,
        private translateHelperService: TranslateHelperService) {
        super();
    }

    public getValue(target: any, key: string): any {
        // this.log.debug(`TranslateParserWithDefault.getValue: key="${key}"`);
        const keyWithDefault = this.translateHelperService.parseKeyWithDefault(key);

        const result = super.getValue(target, keyWithDefault.key);
        if (result !== undefined) {
            return result;
        }

        if (keyWithDefault.default !== null) {
            return keyWithDefault.default;
        }

        return undefined;
    }

}

