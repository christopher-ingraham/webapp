import { Pipe, PipeTransform } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { isNumeric } from 'rxjs/util/isNumeric';

import { AppStatusStoreService } from '../service/app-status-store/app-status-store.service';

import { SourceTargetConverterMap, SourceTargetConverterInfo } from './model';

@Pipe({
    name: 'uomValue',
    pure: false,
})
export class UomValuePipe implements PipeTransform {

    private readonly stcMap: SourceTargetConverterMap = {
        C: { F: { converter: UomValuePipe.convertCelsiusToFahrenheit, format: { digitsInfo: '1.1-1', }, } },
        F: { C: { converter: UomValuePipe.convertFahrenheitToCelsius, format: { digitsInfo: '1.1-1', }, } },
        ft: { m: { converter: UomValuePipe.convertFootToMetre, format: { digitsInfo: '1.1-1', }, } },
        in: { mm: { converter: UomValuePipe.convertInchToMm, format: { digitsInfo: '1.3-3', }, } },
        kg: { lb: { converter: UomValuePipe.convertKgToPound, format: { digitsInfo: '1.1-1', }, } },
        lb: { kg: { converter: UomValuePipe.convertPoundToKg, format: { digitsInfo: '1.1-1', }, } },
        m: { ft: { converter: UomValuePipe.convertMetreToFoot, format: { digitsInfo: '1.1-1', }, } },
        mm: { in: { converter: UomValuePipe.convertMmToInch, format: { digitsInfo: '1.3-3', }, } },
        kN: { ton: { converter: UomValuePipe.convertkNToTon, format: { digitsInfo: '1.1-1', }, } },
        ton: { kN: { converter: UomValuePipe.convertTonToKn, format: { digitsInfo: '1.1-1', }, } },
        'bool': { 'int': { converter: UomValuePipe.convertBooleanToInt } },
        'int': { 'bool': { converter: UomValuePipe.convertIntToBoolean } },
        'm/s': { fpm: { converter: UomValuePipe.convertMetrePerSecondToFpm, format: { digitsInfo: '1.2-2', }, } },
        fpm: { 'm/s': { converter: UomValuePipe.convertFpmToMetrePerSecond, format: { digitsInfo: '1.2-2', }, } },
        psi: { MPa: { converter: UomValuePipe.convertPsiToMPa, format: { digitsInfo: '1.1-1', }, } },
        MPa: { psi: { converter: UomValuePipe.convertMPaToPsi, format: { digitsInfo: '1.1-1', }, } },
        'm³/h': { gpm: { converter: UomValuePipe.convertm3hToGpm, format: { digitsInfo: '1.1-1', }, } },
        gpm: { 'm³/h': { converter: UomValuePipe.convertGpmTom3h, format: { digitsInfo: '1.1-1', }, } },
        'µm': { mils:  { converter: UomValuePipe.convertMicroMetreToMils, format: { digitsInfo: '1.1-1', }, } },
        mils: { 'µm':  { converter: UomValuePipe.convertMilsToMicroMetre, format: { digitsInfo: '1.1-1', }, } },
    };

    public static convertMmToInch(value: number): number {
        if (isNumeric(value)) {
            return value / 25.4;
        }
        return value;
    }
    public static convertInchToMm(value: number): number {
        if (isNumeric(value)) {
            return value * 25.4;
        }
        return value;
    }

    public static convertMicroMetreToMils(value: number): number {
        if (isNumeric(value)) {
            return value / 25.4;
        }
        return value;
    }
    public static convertMilsToMicroMetre(value: number): number {
        if (isNumeric(value)) {
            return value * 25.4;
        }
        return value;
    }

    public static convertMetreToFoot(value: number): number {
        if (isNumeric(value)) {
            return value * 3.28084;
        }
        return value;
    }
    public static convertFootToMetre(value: number): number {
        if (isNumeric(value)) {
            return value / 3.28084;
        }
        return value;
    }

    public static convertKgToPound(value: number): number {
        if (isNumeric(value)) {
            return value * 1000 / 453.59237;
        }
        return value;
    }
    public static convertPoundToKg(value: number): number {
        if (isNumeric(value)) {
            return value * 453.59237 / 1000;
        }
        return value;
    }

    public static convertCelsiusToFahrenheit(value: number): number {
        if (isNumeric(value)) {
            return (value * 1.8) + 32;
        }
        return value;
    }
    public static convertFahrenheitToCelsius(value: number): number {
        if (isNumeric(value)) {
            return (value - 32) * 0.5556;
        }
        return value;
    }

    public static convertkNToTon(value: number): number {
        if (isNumeric(value)) {
            return value * 9.964016384;
        }
        return value;
    }
    public static convertTonToKn(value: number): number {
        if (isNumeric(value)) {
            return value / 9.964016384;
        }
        return value;
    }

    public static convertBooleanToInt(value: boolean): number {
        return value ? 1 : 0;
    }
    public static convertIntToBoolean(value: number): boolean {
        return (value !== 0);
    }

    public static convertMetrePerSecondToFpm(value: number): number {
        if (isNumeric(value)) {
            return value * 196.85;
        }
        return value;
    }
    public static convertFpmToMetrePerSecond(value: number): number {
        if (isNumeric(value)) {
            return value / 196.85;
        }
        return value;
    }

    public static convertPsiToMPa(value: number): number {
        if (isNumeric(value)) {
            return value / 145.037737797;
        }
        return value;
    }
    public static convertMPaToPsi(value: number): number {
        if (isNumeric(value)) {
            return value * 145.037737797;
        }
        return value;
    }

    public static convertm3hToGpm(value: number): number {
        if (isNumeric(value)) {
            return value * 4.402868;
        }
        return value;
    }
    public static convertGpmTom3h(value: number): number {
        if (isNumeric(value)) {
            return value / 0.227125;
        }
        return value;
    }

    constructor(
        private decimalPipe: DecimalPipe,
        private appStatus: AppStatusStoreService,
    ) { }


    public get getAppStatus(): AppStatusStoreService {
        return this.appStatus;
    }



    transform(value: number | boolean, ...args: any[]): any {

        if ((value === undefined) || (value === null)) {
            // GIGO
            return value;
        }

        // Check args:
        // [0] source UoM
        // [1] target UoM
        // [2] format flag (OPTIONAL: default true)
        if (!args || (args.length < 2)) {
            const argc = args ? args.length : 0;
            throw new Error(`${UomValuePipe['name']}: two unit of measurement expected, but found ${argc}`);
        }

        const [source, target, userFormatFlag] = args;
        const conversionInfo: SourceTargetConverterInfo = this.stcMap[source] ? this.stcMap[source][target] : undefined;

        // default is true
        const formatFlag = (args.length < 3) ? true : userFormatFlag;

        if (conversionInfo) {
            // Conversion information present: convert and format.
            return this.convertAndFormat(value, conversionInfo, formatFlag);
        } else {
            // conversion info does not exist: return raw input value.
            return value;
        }
    }

    private convertAndFormat(value: number | boolean, conversionInfo: SourceTargetConverterInfo, formatFlag: boolean): any {
        let convertedValue: number | boolean;

        // Check user setting for system of units
        if (this.appStatus.isUomSI) {
            // Backend-provided values are already in SI units: done.
            convertedValue = value;
        } else {
            // Conversion required
            convertedValue = conversionInfo.converter(value);
        }

        // To be formatted?
        if (formatFlag) {
            const format = conversionInfo.format;
            if (format) {
                return this.decimalPipe.transform(convertedValue,
                    format.digitsInfo,
                    format.locale);
            }
        }

        // All done!
        return convertedValue;
    }

}
