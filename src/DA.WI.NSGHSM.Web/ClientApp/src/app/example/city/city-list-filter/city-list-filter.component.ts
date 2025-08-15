import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { map, tap, debounceTime } from 'rxjs/operators';

import { BaseEntityListFilterComponent } from '@app/shared';
import { CityListFilter } from '../city.model';
import { LogService } from '@app/core';


@Component({
    selector: 'app-city-list-filter',
    templateUrl: './city-list-filter.component.html'
})
export class CityListFilterComponent
    extends BaseEntityListFilterComponent<CityListFilter>
    implements OnInit, OnDestroy {

    constructor(log: LogService) {
        super(log);
    }

    ngOnInit(): void {

        this.form = new FormGroup({ searchText: new FormControl(null) });

        this.subscribe(
            this.form
                .valueChanges
                .pipe(
                    debounceTime(500),
                    tap(_ => this.log.debug('CityListFilterComponent valueChanges', _)),
                    map(_ => this.mapToFilter(_))
                )
                .subscribe(_ => this.filterChange.emit(_))
        );
    }

    private mapToFilter(formValues: any): CityListFilter {
        return <CityListFilter>{
            searchText: formValues.searchText
        };
    }
}
