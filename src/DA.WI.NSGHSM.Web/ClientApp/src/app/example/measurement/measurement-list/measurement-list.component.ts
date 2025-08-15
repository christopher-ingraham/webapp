import { Component, OnInit, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs/operators';

import { PageChangeEvent } from '@progress/kendo-angular-grid';
import { IntlService } from '@progress/kendo-angular-intl';

import { ListRequest, PagingRequest, LogService, AuthService } from '@app/core';
import { BaseEntityListComponent } from '@app/shared';
import { CityApiService } from '../../city';
import { MeasurementApiService } from '../measurement-api.service';
import {
    Measurement,
    MeasurementBase,
    MeasurementDetail,
    MeasurementForInsert,
    MeasurementForUpdate,
    MeasurementListFiltering,
    MeasurementListItem,
    WeatherType,
} from '../measurement.model';

@Component({
    selector: 'app-measurement-list',
    templateUrl: './measurement-list.component.html'
})

export class MeasurementListComponent
    extends BaseEntityListComponent<
        MeasurementBase,
        Measurement,
        MeasurementDetail,
        MeasurementForInsert,
        MeasurementForUpdate,
        MeasurementListItem,
        MeasurementListFiltering,
        MeasurementApiService>
    implements OnInit, OnDestroy, OnChanges {

    WeatherType = WeatherType;
    cityIdSubscription: any;
    cityId: string;
    cityName: string;

    constructor(
        log: LogService,
        authService: AuthService,
        private measurementApiService: MeasurementApiService,
        private cityApiService: CityApiService,
        public intl: IntlService,
        public media: MediaObserver,
        public activatedRoute: ActivatedRoute,
    ) {
        super(log, authService);
    }

    get service() {
        return this.measurementApiService;
    }

    ngOnInit(): void {
        this.setupSecurityRoleFlags();
        this.isMobile = this.media.asObservable().pipe(map(() => this.media.isActive('xs')));
        this.cityIdSubscription = this.activatedRoute.queryParamMap.subscribe(
            (params) => {
                this.cityId = params.get('cityId');
                this.dataRequest = this.initDataRequest();
                this.refreshData();
                if (this.cityId) {
                    this.cityApiService.read(+this.cityId).subscribe(
                        (city) => {
                            this.cityName = city.name;
                        }
                    );
                }
            }
        );
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.dataRequest = this.initDataRequest();
    }

    ngOnDestroy(): void {
        this.cityIdSubscription.unsubscribe();
    }

    onFilterChange(filter: MeasurementListFiltering) {
        this.dataRequest.filter = filter;
        this.dataRequest.page.skip = 0; // reset paging
        this.refreshData();
    }

    private initDataRequest(): ListRequest<MeasurementListFiltering> {
        const returnedDataRequest: ListRequest<MeasurementListFiltering> = {
            filter: {},
            page: { skip: 0, take: this.pageSize },
            sort: { items: [] }
        };
        if (this.cityId) {
            returnedDataRequest.filter = {
                cityId: +this.cityId
            };
        }
        return returnedDataRequest;
    }

    public getWeatherTypeImage(weatherTypeId: number) {
        return `assets/example/${WeatherType[weatherTypeId].toLowerCase()}.png`;
    }
}
