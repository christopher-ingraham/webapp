export class MeasurementBase {
    cityId: number;
    cityName: string;
    measuredAt: Date;
    weather: WeatherType;
    temperatureC: number;
    pressureMB: number;
}

export class Measurement extends MeasurementBase {
    id: number;
}

export class MeasurementDetail extends Measurement {
    city: CityLookupDto;
}

export class MeasurementForInsert extends MeasurementBase {
    // nothing
}

export class MeasurementForUpdate extends MeasurementBase {
    // nothing
}

export class MeasurementListItem extends MeasurementBase {
    // nothing
}

export interface MeasurementListFilter {
    cityId?: number;
    from?: Date;
    to?: Date;
}

interface CityLookupDto {
    id: number;
    name: string;
}

export enum WeatherType {
    Sunny = 0,
    Cloudy = 1,
    Rain = 2
}

// export interface Measurement {

//     id: number;
//     cityId: number;
//     cityName: string;
//     measuredAt: Date;
//     weather: WeatherType;
//     temperatureC: number;
//     pressureMB: number;
// }

export interface MeasurementListFiltering {
    cityId?: number;
    from?: Date;
    to?: Date;
}
