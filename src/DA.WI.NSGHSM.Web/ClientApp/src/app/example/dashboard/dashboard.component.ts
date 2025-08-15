import { Component, OnInit } from '@angular/core';

import { ListRequest } from '@app/core';

import { CityListFilter, CityApiService, CityListItem } from '../city';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {

    cities: CityListItem[] = [];

    constructor(private cityDataApi: CityApiService) { }

    ngOnInit() {
        const listFilter: ListRequest<CityListFilter> = {
            filter: {},
            page: {
                take: 3,
                skip: 0
            },
            sort: {
                items: []
            }
        };

        this.cityDataApi.readList(listFilter).subscribe(_ => this.cities = _.data);
    }

}
