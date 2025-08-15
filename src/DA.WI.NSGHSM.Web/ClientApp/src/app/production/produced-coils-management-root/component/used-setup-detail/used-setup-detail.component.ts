
// used-setup-detail.component.ts
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy, Input, } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';

import { classToClass } from 'class-transformer';

import { TranslateService } from '@ngx-translate/core';

import {
    AppUserInfoService,
    AuthService,
    LogService,
    ValidationsDecoderService,
} from '@app/core';
import {
    BaseEntityDetailComponent,
    UsedSetupApiService,
    UsedSetup,
    UsedSetupBase,
    UsedSetupDetail,
    UsedSetupForInsert,
    UsedSetupForUpdate,
    UsedSetupListFilter,
    UsedSetupListItem,
    FormGroupUomConverterMap,
    UomValuePipe,
    FormGroupEasyBuilder,
    TabstripTabMetadata,
} from '@app/shared';
import { ConfirmService } from '@app/widget';

import { UsedSetupCustomChildState } from '../../model/used-setup-management-child-state.interface.class';

@Component({
    selector: 'app-used-setup-detail',
    templateUrl: './used-setup-detail.component.html',
    styleUrls: ['./used-setup-detail.component.scss']
})
export class UsedSetupDetailComponent
    extends BaseEntityDetailComponent<
    UsedSetupBase,
    UsedSetup,
    UsedSetupDetail,
    UsedSetupForInsert,
    UsedSetupForUpdate,
    UsedSetupListItem,
    UsedSetupListFilter,
    UsedSetupApiService>
    implements OnInit, OnDestroy {

    public readonly modelOptions = {
        standalone: true,
    };
    @Input() public parentTab: TabstripTabMetadata<UsedSetupListItem>;

    public childState: UsedSetupCustomChildState;
    public generalSettings = [];
    public ancillarySetup = [];
    public generalData = [];
    public enabledDisabledCombo = [{ label: '0:DISABLED', value: 0 }, { label: '1:ENABLED', value: 1 }];

    public chartSeriesReductionData = [];
    public chartSeriesMillSpeedData = [];
    public chartSeriesForceData = [];
    public chartSeriesTemperatureData = [];

    constructor(
        formBuilder: FormBuilder,
        appUserInfoService: AppUserInfoService,
        confirmService: ConfirmService,
        log: LogService,
        validationsDecoder: ValidationsDecoderService,
        authService: AuthService,
        private usedSetupApiService: UsedSetupApiService,
        private translateService: TranslateService,
        uomValuePipe: UomValuePipe,
    ) {
        super(formBuilder, log, authService, appUserInfoService,
            confirmService, validationsDecoder, 'EXAMPLE', 'UsedSetup', uomValuePipe);
    }

    get service() {
        return this.usedSetupApiService;
    }

    get formGroup(): FormGroup {
        const f = new FormGroupEasyBuilder(this.formValidationMetadata, this.isReadOnly);
        f.addNumbers(
            'millMode',
            'density',
            'entryWdt',
            'transferBarThk',
            'thermalExpCoeff',
            'targetWdt',
            'transferBarWdt',
            'pieceLength',
            'entryThk',
            'transferBarTemp',
            'entryTemp',
            'pieceWeight',
            'targetThk',
            'descalerHeadOffset',
            'icHeadOffset',
            'rollBiteOilPerc',
            'tailCropLength',
        );

        f.addStrings(
            'operator',
            'practiceId',
            'centerId',
            'inPieceId',
            'materialGradeId',
            'gradeGroupLabel',
            'descalerMode',
            'enabledStandF1',
            'enabledStandF2',
            'enabledStandF3',
            'enabledStandF4',
            'enabledStandF5',
            'enabledStandF6'
        );

        f.addDates(
            'revision'
        );

        return f.formGroup;
    }

    get formGroupConverters(): FormGroupUomConverterMap {
        const cm = new FormGroupUomConverterMap(this.form, this.uomValuePipe);

        cm.map = [
            { key: 'pieceWeight', uomSI: 'kg', uomUSCS: 'lb' },
            { key: 'entryThk', uomSI: 'mm', uomUSCS: 'in' },

        ];

        return cm;
    }

    protected dataToForm(data: UsedSetupDetail) {
        this.generalSettings = data.generalSettings;
        const chartSeriesReductionData = [];
        const chartSeriesMillSpeedData = [];
        const chartSeriesForceData = [];
        const chartSeriesTemperatureData = [];
        const ancillarySetup = [
            { 'label': 'Oil In Water [%]' },
            { 'label': 'Crop Shear Head Cut length' },
            { 'label': 'Crop Shear Tail Cut length [m]' }
        ];

        data['enabledStandF1'] = this.generalSettings[0].enabledStand;
        data['enabledStandF2'] = this.generalSettings[1].enabledStand;
        data['enabledStandF3'] = this.generalSettings[2].enabledStand;
        data['enabledStandF4'] = this.generalSettings[3].enabledStand;
        data['enabledStandF5'] = this.generalSettings[4].enabledStand;
        data['enabledStandF6'] = this.generalSettings[5].enabledStand;

        const generalData = [
            { 'label': 'Entry Thickness [mm]' }, { 'label': 'Exit Thickness [mm]' },
            { 'label': 'Draft [mm]' },
            { 'label': 'Reduction [%]' },
            { 'label': 'Entry Width [mm]' }, { 'label': 'Exit Width [mm]' },
            { 'label': 'Entry Temp [°C]' }, { 'label': 'Exit Temp [°C]' },
            { 'label': 'Entry Tension [MPa]' }, { 'label': 'Exit Tension [MPa]' },
            { 'label': 'Entry Tension [kN]' }, { 'label': 'Exit Tension [kN]' },
            { 'label': 'Threading Speed [m/s]' },
            { 'label': 'Head Force [kN]' },
            { 'label': 'WR Bending [kN]' },
            { 'label': 'WR Shifting [mm]' }

        ];
        let count = 1;
        this.generalSettings.forEach(function (item) {
            const row = 'F' + count;
            ancillarySetup[0][row] = item.rollBiteOilPerc;
            ancillarySetup[1][row] = item.headCropLength;
            ancillarySetup[2][row] = item.tailCropLength;

            generalData[0][row] = item.entryThk;
            generalData[1][row] = item.exitThk;
            generalData[2][row] = item.draft;
            generalData[3][row] = item.reduction;
            generalData[4][row] = item.entryWidth;
            generalData[5][row] = item.exitWidth;
            generalData[6][row] = item.entryTemp;
            generalData[7][row] = item.exitTemp;
            generalData[8][row] = item.hEntryTemp;
            generalData[9][row] = item.hExitTemp;
            generalData[10][row] = item.specEntryTension;
            generalData[11][row] = item.specExitTension;
            generalData[12][row] = item.threadingSpeed;
            generalData[13][row] = item.hForce;
            generalData[14][row] = item.wrBend;
            generalData[15][row] = item.wrShift;

            chartSeriesReductionData.push(item.reduction);
            chartSeriesMillSpeedData.push(item.millSpeed);
            chartSeriesForceData.push(item.force);
            chartSeriesTemperatureData.push(item.exitTemp);

            count++;

        });

        this.chartSeriesReductionData = chartSeriesReductionData;
        this.chartSeriesMillSpeedData = chartSeriesMillSpeedData;
        this.chartSeriesForceData = chartSeriesForceData;
        this.chartSeriesTemperatureData = chartSeriesTemperatureData;
        this.ancillarySetup = ancillarySetup;
        this.generalData = generalData;


        super.dataToForm(data);
    }

    get formToEntityForInsert(): UsedSetupForInsert {
        const entityForInsert = classToClass<UsedSetupForInsert>(this.form.value);
        // TODO classToClass() performs just a raw conversion
        return entityForInsert;
    }
    get formToEntityForUpdate(): UsedSetupForUpdate {
        const entityForUpdate = classToClass<UsedSetupForUpdate>(this.form.value);
        // TODO classToClass() performs just a raw conversion
        return entityForUpdate;
    }
    get newEntityDetail(): Observable<Partial<UsedSetupDetail>> {
        return this.newEntityDetailConstant({
            // define constant attributes here
        });
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
        if (this.isNew) {
            // create new entity
            const entityForInsert = this.formToEntityForInsert;
            this.sanitizeId();
            // entityForInsert.id = this.id; // FIXME verify entity's key
            this.usedSetupApiService.create(entityForInsert).subscribe(
                (result) => this.goBack()
            );
        } else {
            // update existing entity
            const entityForUpdate = this.formToEntityForUpdate;
            this.sanitizeId();
            // entityForUpdate.id = this.id; // FIXME verify entity's key
            this.usedSetupApiService.update(this.id, entityForUpdate).subscribe(
                (result) => this.goBack()
            );
        }
    }

    onDeleteClick() {
        if (!this.id || this.isReadOnly === true) {
            return;
        }

        if (!confirm(this.translateService.instant(this.areYouSureToDeleteThisItem))) {
            return;
        }

        this.usedSetupApiService.delete(this.id).subscribe(
            (result) => this.goBack()
        );
    }

    private sanitizeId() {
        // if id = NaN backend side the controller crashes
        if (isNaN(this.id)) {
            this.id = undefined;
        }
    }

    ngOnDestroy() {
        this.unsubscribeAll();
    }

    // Chart series

    public get chartSeriesReduction(): number[] {

        // TODO get from DTO
        return this.chartSeriesReductionData;
    }
    public get chartSeriesMillSpeed(): number[] {
        // TODO get from DTO
        return this.chartSeriesMillSpeedData;
    }
    public get chartSeriesForce(): number[] {
        // TODO get from DTO
        return this.chartSeriesForceData;
    }
    public get chartSeriesTemperature(): number[] {
        // TODO get from DTO
        return this.chartSeriesTemperatureData;
    }

}
