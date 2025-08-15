import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl, ValidatorFn, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

import { TranslateService } from '@ngx-translate/core';
import { classToClass } from 'class-transformer';

import { ListRequest, ValidationsDecoderService, AppUserInfoService, LogService, AuthService } from '@app/core';
import { BaseEntityDetailComponent, ChildState, FormGroupUomConverterMap, UomValuePipe, TabstripTabMetadata } from '@app/shared';
import { ConfirmService } from '@app/widget';

import { CityApiService, CityListFilter, CityListItem } from '../../city';
import { MeasurementApiService } from '../measurement-api.service';
import {
    Measurement,
    MeasurementBase,
    MeasurementDetail,
    MeasurementForInsert,
    MeasurementForUpdate,
    MeasurementListFiltering,
    WeatherType,
    MeasurementListItem,
} from '../measurement.model';

@Component({
    selector: 'app-measurement-detail',
    templateUrl: './measurement-detail.component.html'
})
export class MeasurementDetailComponent
    extends BaseEntityDetailComponent<
    MeasurementBase,
    Measurement,
    MeasurementDetail,
    MeasurementForInsert,
    MeasurementForUpdate,
    MeasurementListItem,
    MeasurementListFiltering,
    MeasurementApiService>
    implements OnInit, OnDestroy {

    public childState: ChildState;
    @Input() public parentTab: TabstripTabMetadata<MeasurementListItem>;

    cities: CityListItem[] = [];
    weatherTypes: Array<{ text: string, value: number }> = [];
    selectedCityName: string;
    cityId: number;

    constructor(
        formBuilder: FormBuilder,
        appUserInfoService: AppUserInfoService,
        confirmService: ConfirmService,
        log: LogService,
        validationsDecoder: ValidationsDecoderService,
        authService: AuthService,
        private activatedRoute: ActivatedRoute,
        private measurementApiService: MeasurementApiService,
        private cityApiService: CityApiService,
        private translateService: TranslateService,
        uomValuePipe: UomValuePipe,
    ) {
        super(formBuilder, log, authService, appUserInfoService,
            confirmService, validationsDecoder, 'EXAMPLE', 'Measurement', uomValuePipe);
    }

    get service() {
        return this.measurementApiService;
    }

    get formGroup(): FormGroup {
        return this.formBuilder.group({
            cityName: new FormControl(this.formControlDefaultStringState, [Validators.required, this.CityValidator()]),
            measuredAt: new FormControl(this.formControlDefaultDateState, [Validators.required]),
            pressureMB: new FormControl(this.formControlDefaultNumberState, [Validators.required]),
            temperatureC: new FormControl(this.formControlDefaultNumberState, [Validators.required]),
            weather: new FormControl(this.formControlDefaultStringState, [Validators.required]),
        });
    }
    get formGroupConverters(): FormGroupUomConverterMap {
        const cm = new FormGroupUomConverterMap(this.form, this.uomValuePipe);
        // TODO
        return cm;
    }


    get formToEntityForInsert(): MeasurementForInsert {
        const measurement = classToClass<MeasurementForInsert>(this.form.value);
        // FIXME
        return measurement;
    }
    get formToEntityForUpdate(): MeasurementForUpdate {

        const measurement = classToClass<MeasurementForUpdate>(this.form.value);
        const cityName = this.form.value.cityName;
        const city = cityName && this.cities
            ? this.cities.find(_ => _.name === cityName)
            : null;

        if (this.cityId) {
            measurement.cityId = this.cityId;
        } else {
            measurement.cityId = city && city.id;
        }

        return measurement;
    }
    get newEntityDetail(): Observable<Partial<MeasurementDetail>> {
        return this.newEntityDetailConstant({});
    }

    ngOnInit(): void {
        this.initWeatherTypes();
        this.detailInit().subscribe((ok) => {
            if (ok) {
                /* anything after form+valid+convertes initialization */
            }
        });
        this.subscribe(
            this.activatedRoute.queryParamMap.subscribe(
                params => {
                    this.cityId = +params.get('cityId');

                    if (this.cityId) {
                        this.cityApiService.read(this.cityId).subscribe(_ => this.form.patchValue({ 'cityName': _.name }));
                    }
                })
        );
    }

    onCityFilterChange(searchCityName: string) {

        const cityFilter: ListRequest<CityListFilter> = {
            filter: { searchCityName },
            page: { skip: 0, take: 5 },
            sort: { items: [] }
        };

        this.cityApiService.readList(cityFilter)
            .pipe(map(res => res.data))
            .subscribe(res => this.cities = res);
    }

    onSaveClick() {
        if (!this.form.valid) {
            return;
        }

        const data = this.isNew ? this.formToEntityForInsert : this.formToEntityForUpdate;

        const apiAction = this.isNew
            ? this.measurementApiService.create(data)
            : this.measurementApiService.update(this.id, data);

        apiAction.subscribe(result => this.goBack());
    }

    onDeleteClick() {
        if (!this.id) {
            return;
        }

        if (!confirm(this.translateService.instant(this.areYouSureToDeleteThisItem))) {
            return;
        }

        this.measurementApiService.delete(this.id).subscribe(result => this.goBack());
    }

    private initWeatherTypes() {

        this.weatherTypes = [];

        for (const weatherType in WeatherType) {
            if (WeatherType.hasOwnProperty(weatherType)) {
                const value = WeatherType[weatherType];
                if (typeof value === 'number') {
                    this.weatherTypes.push({ value: value, text: weatherType });
                }
            }
        }
    }

    private CityValidator(): ValidatorFn {
        return (c: AbstractControl): { [key: string]: Boolean } | null => {

            if (this.cityId) {
                return null;
            }

            if (!c.value) {
                return null;
            }

            if (this.cities && this.cities.find(_ => _.name === c.value)) {
                return null;
            }

            return { valid: true };
        };
    }

}
