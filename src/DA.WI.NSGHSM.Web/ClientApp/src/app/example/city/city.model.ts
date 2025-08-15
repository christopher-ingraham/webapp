export class CityBase {
    name: string;
    countryName: string;
    population: number;

    public get fullName() {
        return `${this.name} - ${this.countryName}`;
    }
}

export class City extends CityBase {
    id: number;
}

export class CityDetail extends City {
    // nothing
}

export class CityForInsert extends City {
    // nothing
}

export class CityForUpdate extends City {
    // nothing
}

export class CityListItem extends City {
    // nothing
}

export interface CityListFilter {
    searchText?: string;
    searchCityName?: string;
}
