import { Component, OnInit, OnDestroy, EventEmitter, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { debounceTime, tap, map } from 'rxjs/operators';

import { ListRequest, LogService } from '@app/core';
import { BaseEntityListFilterComponent } from '@app/shared';
import { City, CityListFilter, CityApiService } from '../../city';
import { MeasurementListFiltering } from '../measurement.model';

@Component({
    selector: 'app-measurement-list-filter',
    templateUrl: './measurement-list-filter.component.html'
})
export class MeasurementListFilterComponent
    extends BaseEntityListFilterComponent<MeasurementListFiltering>
    implements OnInit, OnDestroy, OnChanges {

    @Input() filterCityId: string;

    cities: City[] = [];
    showCityFilter: boolean;

    constructor(log: LogService, private cityApi: CityApiService) {
        super(log);
    }

    ngOnInit(): void {
        this.form = new FormGroup(
            {
                cityName: new FormControl(null),
                from: new FormControl(null),
                to: new FormControl(null)
            }
        );

        this.subscribe(
            this.form
                .valueChanges
                .pipe(
                    debounceTime(500),
                    // tslint:disable-next-line: no-console
                    tap(_ => this.log.debug('MeasurementListFilterComponent valueChanges', _)),
                    map(_ => this.mapToFilter(_))
                )
                .subscribe(_ => this.filterChange.emit(_))
        );
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.showCityFilter = (this.filterCityId === null);
    }

    private mapToFilter(formValues: any): MeasurementListFiltering {
        let cityId: number; // Filter forced from input parameter
        if (this.filterCityId == null) {
            const city = formValues.cityName && this.cities
                ? this.cities.find(_ => _.name === formValues.cityName)
                : null;
            cityId = city && city.id;
        } else {
            cityId = +this.filterCityId;
        }

        const from = formValues.from && new Date(formValues.from);
        const to = formValues.to && new Date(formValues.to);

        return <MeasurementListFiltering>{
            cityId: cityId,
            from: from,
            to: to
        };
    }

    onFilterChange(searchCityName: string) {
        const cityFilter: ListRequest<CityListFilter> = {
            filter: { searchCityName: searchCityName },
            page: { skip: 0, take: 5 },
            sort: { items: [] }
        };
        this.cityApi.readList(cityFilter)
            .pipe(map(res => res.data))
            .subscribe(res => this.cities = res);
    }

}
