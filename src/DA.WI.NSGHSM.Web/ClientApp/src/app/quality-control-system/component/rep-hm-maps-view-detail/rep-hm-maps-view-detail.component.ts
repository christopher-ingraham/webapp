
// rep-hm-piece-Trend-detail.component.ts
import { ActivatedRoute, Router, Route } from '@angular/router';
import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl, FormBuilder } from '@angular/forms';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';

import { TranslateService } from '@ngx-translate/core';

import { AuthService, ValidationsDecoderService, AppUserInfoService, LogService } from '@app/core';
import {
    BaseEntityDetailComponent,
    ChildState,
    FormGroupUomConverterMap,
    UomValuePipe,
    TabstripTabMetadata,
} from '@app/shared';
import { ConfirmService } from '@app/widget';
import { classToClass } from 'class-transformer';
import {
    RepHmMapsView,
    RepHmMapsViewBase,
    RepHmMapsViewDetail,
    RepHmMapsViewForInsert,
    RepHmMapsViewForUpdate,
    RepHmMapsViewListItem,
    RepHmMapsViewListFilter,
    RepHmMapsViewApiService
} from 'src/app/shared/service/rep-hm-maps-view-api';


@Component({
    selector: 'app-rep-hm-maps-view-detail',
    templateUrl: './rep-hm-maps-view-detail.component.html',
    styleUrls: ['./rep-hm-maps-view-detail.component.scss']
})
export class RepHmMapsViewDetailComponent
    extends BaseEntityDetailComponent<
    RepHmMapsViewBase,
    RepHmMapsView,
    RepHmMapsViewDetail,
    RepHmMapsViewForInsert,
    RepHmMapsViewForUpdate,
    RepHmMapsViewListItem,
    RepHmMapsViewListFilter,
    RepHmMapsViewApiService>
    implements OnInit, OnDestroy {

    public childState: ChildState;

    @Input() public parentTab: TabstripTabMetadata<RepHmMapsViewListItem>;

    constructor(
        formBuilder: FormBuilder,
        appUserInfoService: AppUserInfoService,
        confirmService: ConfirmService,
        log: LogService,
        validationsDecoder: ValidationsDecoderService,
        authService: AuthService,
        private repHmMapsViewApiService: RepHmMapsViewApiService,
        private translateService: TranslateService,
        uomValuePipe: UomValuePipe,
    ) {
        super(formBuilder, log, authService, appUserInfoService,
            confirmService, validationsDecoder, 'EXAMPLE', 'RepHmMapsView', uomValuePipe);
    }

    get service() {
        return this.repHmMapsViewApiService;
    }

    get formGroup(): FormGroup {
        return new FormGroup({
            description: new FormControl(this.formControlDefaultStringState, []),
            measUnit: new FormControl('', []),
            centerId: new FormControl('', []),
            outPieceNo: new FormControl('', []),
            passNo: new FormControl('', []),
            signalId: new FormControl('', []),
        });
    }
    get formGroupConverters(): FormGroupUomConverterMap {
        const cm = new FormGroupUomConverterMap(this.form, this.uomValuePipe);
        // TODO
        return cm;
    }


    get formToEntityForInsert(): RepHmMapsViewForInsert {
        return classToClass<RepHmMapsViewForInsert>(this.form.value);
    }
    get formToEntityForUpdate(): RepHmMapsViewForUpdate {
        return classToClass<RepHmMapsViewForUpdate>(this.form.value);
    }
    get newEntityDetail() {
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
            ? this.repHmMapsViewApiService.create(data as RepHmMapsViewForInsert)
            : this.repHmMapsViewApiService.update(this.id, data as RepHmMapsViewForUpdate);

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

        this.repHmMapsViewApiService.delete(this.id).subscribe(
            (result) => this.goBack()
        );
    }

}

