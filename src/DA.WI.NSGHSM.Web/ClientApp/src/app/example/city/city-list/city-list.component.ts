import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { MediaObserver } from '@angular/flex-layout';

import { ListRequest, AuthService, LogService  } from '@app/core';
import { BaseEntityListComponent } from '@app/shared';
import { CityApiService } from '../city-api.service';
import { CityListFilter, City, CityBase, CityDetail, CityForInsert, CityForUpdate, CityListItem } from '../city.model';


@Component({
    selector: 'app-city-list',
    templateUrl: './city-list.component.html'
})
export class CityListComponent
    extends BaseEntityListComponent<
        CityBase,
        City,
        CityDetail,
        CityForInsert,
        CityForUpdate,
        CityListItem,
        CityListFilter,
        CityApiService>
    implements OnInit {

    isReadOnly = false;

    constructor(
        log: LogService,
        authService: AuthService,
        private cityApiService: CityApiService,
        public media: MediaObserver,
    ) {
        super(log, authService);
        this.dataRequest = this.initDataRequest();
    }

    get service() {
        return this.cityApiService;
    }

    ngOnInit(): void {
        this.refreshData();
        this.setupSecurityRoleFlags();
        this.isMobile = this.media.asObservable().pipe(map(() => this.media.isActive('xs')));
    }

    onFilterChange(filter: CityListFilter) {
        this.dataRequest.filter = filter;
        // transforming in like search
        this.dataRequest.filter.searchCityName = filter.searchCityName;
        this.dataRequest.filter.searchText = filter.searchText;
        this.dataRequest.page.skip = 0; // reset paging
        this.refreshData();
    }

    private initDataRequest(): ListRequest<CityListFilter> {
        return {
            filter: { searchText: null },
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] }
        };
    }
}
