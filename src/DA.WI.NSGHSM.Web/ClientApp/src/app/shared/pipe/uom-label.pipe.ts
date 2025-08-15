import { Pipe, PipeTransform } from '@angular/core';

import { AppStatusStoreService } from '../service/app-status-store/app-status-store.service';

@Pipe({
    name: 'uomLabel',
    pure: false,
})
export class UomLabelPipe implements PipeTransform {

    constructor(private appStatus: AppStatusStoreService) { }

    // @example
    // {{'Head Thickness [mm]' | translate | uomLabel:'mm':'in' }}
    // {{ 'mm' | uomLabel:'!mm':'in' }}
    transform(value: any, ...args: any[]): any {

        // Check user setting for system of units
        if (this.appStatus.isUomSI) {
            // Label contains SI units already: done.
            return value;
        }

        // Make sure value is a string
        const label = '' + value;

        // Check args:
        // [0] SI UoM
        // [1] USCS UoM
        if (!args || (args.length !== 2)) {
            const length = args ? args.length : 0;
            throw new Error(`${UomLabelPipe['name']}: two unit of measurement expected, but found ${length}`);
        }

        const [searchUom, replaceUom] = args;

        if (searchUom[0] === '!') {
            // VERBATIM SEARCH
            const actualSearchUom = searchUom.substr(1);
            return label.replace(actualSearchUom, replaceUom);
        } else {
            // UoM to use
            const uomToUse = `[${this.appStatus.isUomSI ? searchUom : replaceUom}]`;
            // UoM to search for
            const searchForString = `[${searchUom}]`;
            // Replace UoM
            return label.replace(searchForString, uomToUse);
        }
    }

}
