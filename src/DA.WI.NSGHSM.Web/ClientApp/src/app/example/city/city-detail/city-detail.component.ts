import { ActivatedRoute, Router, Route } from '@angular/router';
import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl, FormBuilder } from '@angular/forms';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';

import { classToClass } from 'class-transformer';

import { TranslateService } from '@ngx-translate/core';

import { AuthService, ValidationsDecoderService, AppUserInfoService, LogService } from '@app/core';
import { BaseEntityDetailComponent, ChildState, FormGroupUomConverterMap, UomValuePipe, TabstripTabMetadata } from '@app/shared';
import { ConfirmService } from '@app/widget';
import { CityApiService } from '../city-api.service';
import { City, CityDetail, CityBase, CityListFilter, CityForInsert, CityForUpdate, CityListItem } from '../city.model';


@Component({
    selector: 'app-city-detail',
    templateUrl: './city-detail.component.html'
})
export class CityDetailComponent
    extends BaseEntityDetailComponent<
    CityBase,
    City,
    CityDetail,
    CityForInsert,
    CityForUpdate,
    CityListItem,
    CityListFilter,
    CityApiService>
    implements OnInit, OnDestroy {

    public childState: ChildState;
    @Input() public parentTab: TabstripTabMetadata<CityListItem>;

    constructor(
        formBuilder: FormBuilder,
        appUserInfoService: AppUserInfoService,
        confirmService: ConfirmService,
        log: LogService,
        validationsDecoder: ValidationsDecoderService,
        authService: AuthService,
        private cityApiService: CityApiService,
        private translateService: TranslateService,
        uomValuePipe: UomValuePipe,
    ) {
        super(formBuilder, log, authService, appUserInfoService,
            confirmService, validationsDecoder, 'EXAMPLE', 'City', uomValuePipe);
    }

    get service() {
        return this.cityApiService;
    }

    get formGroup(): FormGroup {
        return this.formBuilder.group({
            countryName: new FormControl(this.formControlDefaultStringState, [Validators.required, Validators.maxLength(20)]),
            name: new FormControl(this.formControlDefaultStringState, [Validators.required, Validators.maxLength(20)]),
            population: new FormControl(this.formControlDefaultNumberState, [Validators.required, Validators.min(1)]),
        });
    }
    get formGroupConverters(): FormGroupUomConverterMap {
        const cm = new FormGroupUomConverterMap(this.form, this.uomValuePipe);
        // TODO
        return cm;
    }

    get formToEntityForInsert(): CityForInsert {
        return classToClass<CityForInsert>(this.form.value);
    }
    get formToEntityForUpdate(): CityForUpdate {
        return classToClass<CityForUpdate>(this.form.value);
    }
    get newEntityDetail(): Observable<Partial<CityDetail>> {
        return this.newEntityDetailConstant({});
    }

    ngOnInit(): void {
        this.detailInit().subscribe((ok) => {
            if (ok) {
                /* anything after form+valid+convertes initialization */
            }
        });
    }

    onSaveClick() {
        if (!this.form.valid || this.isReadOnly === true) {
            return;
        }
        const data = this.isNew ? this.formToEntityForInsert : this.formToEntityForUpdate;
        // if id = NaN backend side the controller crashes
        if (isNaN(this.id)) {
            this.id = undefined;
        }
        data.id = this.id;
        const apiAction = this.isNew
            ? this.cityApiService.create(data)
            : this.cityApiService.update(this.id, data);

        apiAction.subscribe(
            (result) => this.goBack()
        );
    }

    onDeleteClick() {
        if (!this.id || this.isReadOnly === true) {
            return;
        }

        if (!confirm(this.translateService.instant(this.areYouSureToDeleteThisItem))) {
            return;
        }

        this.cityApiService.delete(this.id).subscribe(
            (result) => this.goBack()
        );
    }

}
