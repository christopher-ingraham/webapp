
// produced-coil-detail.component.ts
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy, Input, } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';

import { classToClass } from 'class-transformer';

import { TranslateService } from '@ngx-translate/core';
import { RmlCrewApiService } from '../../../../shared/service/produced-coil-api/rml-crew-api.service';

import {
    AppUserInfoService,
    AuthService,
    LogService,
    ValidationsDecoderService,
} from '@app/core';
import {
    AuxValueService,
    BaseEntityDetailComponent,
    ProducedCoilApiService,
    ProducedCoil,
    ProducedCoilBase,
    ProducedCoilDetail,
    ProducedCoilForInsert,
    ProducedCoilForUpdate,
    ProducedCoilListFilter,
    ProducedCoilListItem,
    ComboBoxItemNumberString,
    ComboBoxItemStringString,
    FormGroupUomConverterMap,
    UomValuePipe,
    FormGroupEasyBuilder,
    TabstripTabMetadata,
} from '@app/shared';
import { ConfirmService } from '@app/widget';

import { ProducedCoilsManagementChildState } from '../../model/produced-coils-management-child-state.interface';


@Component({
    selector: 'app-produced-coil-detail',
    templateUrl: './produced-coil-detail.component.html',
    styleUrls: ['./produced-coil-detail.component.scss']
})
export class ProducedCoilDetailComponent
    extends BaseEntityDetailComponent<
    ProducedCoilBase,
    ProducedCoil,
    ProducedCoilDetail,
    ProducedCoilForInsert,
    ProducedCoilForUpdate,
    ProducedCoilListItem,
    ProducedCoilListFilter,
    ProducedCoilApiService>
    implements OnInit, OnDestroy {

    public readonly modelOptions = {
        standalone: true,
    };
    @Input() public parentTab: TabstripTabMetadata<ProducedCoilListItem>;

    public childState: ProducedCoilsManagementChildState;
    public interstandCoolingBitmaskList = [{ label: '0:OFF', value: false }, { label: '1:ON', value: true }];
    public readonly inputSlabList: Observable<ComboBoxItemNumberString[]>;
    public readonly coilStatusList: Observable<ComboBoxItemNumberString[]>;
    public readonly shiftIdList: Observable<ComboBoxItemNumberString[]>;
    public readonly innerDiameterList: Observable<ComboBoxItemNumberString[]>;
    public readonly crewIdList: Observable<ComboBoxItemNumberString[]>;

    constructor(
        formBuilder: FormBuilder,
        appUserInfoService: AppUserInfoService,
        confirmService: ConfirmService,
        log: LogService,
        validationsDecoder: ValidationsDecoderService,
        authService: AuthService,
        private producedCoilApiService: ProducedCoilApiService,
        private translateService: TranslateService,
        private auxValueApiService: AuxValueService,
        private rmlCrewApiService: RmlCrewApiService,
        uomValuePipe: UomValuePipe,
    ) {
        super(formBuilder, log, authService, appUserInfoService,
            confirmService, validationsDecoder, 'EXAMPLE', 'ProducedCoil', uomValuePipe );


        this.inputSlabList = this.createinputSlabList();
        this.coilStatusList = this.createCoilStatusList();
        this.shiftIdList = this.createShiftIdList();
        this.innerDiameterList = this.createInnerDiameterList();
        this.crewIdList = this.createCrewIdList();
    }

    get service() {
        return this.producedCoilApiService;
    }

    protected dataToForm(data: ProducedCoilDetail): void {

        const dispositionCodes = data.dispositionCodesForHold;
        if (dispositionCodes) {
            data['biteSprayPressureTooLowInF1'] = dispositionCodes[0];
            data['aimGaugeChanged'] = dispositionCodes[1];
            data['cobble'] = dispositionCodes[2];
            data['cobblePrevention'] = dispositionCodes[3];
            data['coilingTempOutsideCustomer'] = dispositionCodes[4];
            data['casterWedgeNotOk'] = dispositionCodes[5];
            data['damagedTail'] = dispositionCodes[6];
            data['foldedHead'] = dispositionCodes[7];
            data['finishingTempOutsideCustomer'] = dispositionCodes[8];
            data['thicknessOutsideCustomerTol'] = dispositionCodes[9];
            data['gradeChanged'] = dispositionCodes[10];
            data['coilHeadTooHot'] = dispositionCodes[11];
            data['descaleHighPressureNotOk'] = dispositionCodes[12];
            data['heatRidge'] = dispositionCodes[13];
            data['coilingPyrometerNotWorking'] = dispositionCodes[14];
            data['finishingPyrometerNotWorking'] = dispositionCodes[15];
            data['widthGaugeNotWorking'] = dispositionCodes[16];
            data['thicknessGaugeNotWorking'] = dispositionCodes[17];
            data['orderNumberChanged'] = dispositionCodes[18];
            data['edgeThicknessBelowCustomer'] = dispositionCodes[19];
            data['processorWidthChanged'] = dispositionCodes[20];
            data['coilSlippedInF1'] = dispositionCodes[21];
            data['slipPrevention'] = dispositionCodes[22];
            data['coilingTemperatureChanged'] = dispositionCodes[23];
            data['slabInTunnelFurnaceTooLong'] = dispositionCodes[24];
            data['transitionWidth'] = dispositionCodes[25];
            data['widthDips'] = dispositionCodes[26];
            data['widthOutsideCustomerTol'] = dispositionCodes[27];
            data['coilWeightOutsideCustomerTol'] = dispositionCodes[28];
            data['widthWarning'] = dispositionCodes[29];
        }


        const inputCoilTrgMeas = data.inputCoilTrgMeas;
        if (inputCoilTrgMeas) {
            data['targetWidthMeas'] = inputCoilTrgMeas.targetWidth;
            data['targetWidthPtol'] = inputCoilTrgMeas.targetWidthPtol;
            data['targetWidthNtol'] = inputCoilTrgMeas.targetWidthNtol;
            data['targetThicknessMeas'] = inputCoilTrgMeas.targetThickness;
            data['targetThicknessPtol'] = inputCoilTrgMeas.targetThicknessPtol;
            data['targetThicknessNtol'] = inputCoilTrgMeas.targetThicknessNtol;
            data['targetTempFmMeas'] = inputCoilTrgMeas.targetTempFm;
            data['targetTempFmPtol'] = inputCoilTrgMeas.targetTempFmPtol;
            data['targetTempFmNtol'] = inputCoilTrgMeas.targetTempFmNtol;
            data['targetTempDcMeas'] = inputCoilTrgMeas.targetTempDc;
            data['targetTempDcPtol'] = inputCoilTrgMeas.targetTempDcPtol;
            data['targetTempDcNtol'] = inputCoilTrgMeas.targetTempDcNtol;
            data['targetProfileMeas'] = inputCoilTrgMeas.targetProfile;
            data['targetProfilePtol'] = inputCoilTrgMeas.targetProfilePtol;
            data['targetProfileNtol'] = inputCoilTrgMeas.targetProfileNtol;
        }

        const outCoilSetupIntermediateTemp = data.outCoilSetupIntermediateTemp;
        if (outCoilSetupIntermediateTemp) {
            data['targetTempInterm'] = outCoilSetupIntermediateTemp.targetTempInterm;
            data['targetTempIntermUpTol'] = outCoilSetupIntermediateTemp.targetTempIntermUpTol;
            data['targetTempIntermLoTol'] = outCoilSetupIntermediateTemp.targetTempIntermLoTol;
        }


        const currentOutCoilMeas = data.currentOutCoilMeas;
        if (currentOutCoilMeas) {
            if (currentOutCoilMeas[0]) {
                data['exitThkAvg'] = currentOutCoilMeas[0].exitThk;
                data['exitWdtAvg'] = currentOutCoilMeas[0].exitWdt;
                data['exitStripTempAvg'] = currentOutCoilMeas[0].exitStripTemp;
                data['downcoilTempAvg'] = currentOutCoilMeas[0].downcoilTemp;
                data['profileAvg'] = currentOutCoilMeas[0].profile;
                data['stripEdgeDropAvg'] = currentOutCoilMeas[0].stripEdgeDrop;
                data['stripQbFlatnessAvg'] = currentOutCoilMeas[0].stripQbFlatness;
                data['intermediateTempAvg'] = currentOutCoilMeas[0].intermediateTemp;
            }

            if (currentOutCoilMeas[1]) {
                data['exitThkMin'] = currentOutCoilMeas[1].exitThk;
                data['exitWdtMin'] = currentOutCoilMeas[1].exitWdt;
                data['exitStripTempMin'] = currentOutCoilMeas[1].exitStripTemp;
                data['downcoilTempMin'] = currentOutCoilMeas[1].downcoilTemp;
                data['profileMin'] = currentOutCoilMeas[1].profile;
                data['stripEdgeDropMin'] = currentOutCoilMeas[1].stripEdgeDrop;
                data['stripQbFlatnessMin'] = currentOutCoilMeas[1].stripQbFlatness;
                data['intermediateTempMin'] = currentOutCoilMeas[1].intermediateTemp;
            }

            if (currentOutCoilMeas[2]) {
                data['exitThkMax'] = currentOutCoilMeas[2].exitThk;
                data['exitWdtMax'] = currentOutCoilMeas[2].exitWdt;
                data['exitStripTempMax'] = currentOutCoilMeas[2].exitStripTemp;
                data['downcoilTempMax'] = currentOutCoilMeas[2].downcoilTemp;
                data['profileMax'] = currentOutCoilMeas[2].profile;
                data['stripEdgeDropMax'] = currentOutCoilMeas[2].stripEdgeDrop;
                data['stripQbFlatnessMax'] = currentOutCoilMeas[2].stripQbFlatness;
                data['intermediateTempMax'] = currentOutCoilMeas[2].intermediateTemp;
            }

            if (currentOutCoilMeas[3]) {
                data['exitThkStd'] = currentOutCoilMeas[3].exitThk;
                data['exitWdtStd'] = currentOutCoilMeas[3].exitWdt;
                data['exitStripTempStd'] = currentOutCoilMeas[3].exitStripTemp;
                data['downcoilTempStd'] = currentOutCoilMeas[3].downcoilTemp;
                data['profileStd'] = currentOutCoilMeas[3].profile;
                data['stripEdgeDropStd'] = currentOutCoilMeas[3].stripEdgeDrop;
                data['stripQbFlatnessStd'] = currentOutCoilMeas[3].stripQbFlatness;
                data['intermediateTempStd'] = currentOutCoilMeas[3].intermediateTemp;
            }
        }


        const rollsDataForStands = data.rollDataForStands;
        if (rollsDataForStands) {
            if (rollsDataForStands[0]) {
                data['brUpIdF1'] = rollsDataForStands[0].brUpId;
                data['wrUpIdF1'] = rollsDataForStands[0].wrUpId;
                data['wrLoIdF1'] = rollsDataForStands[0].wrLoId;
                data['brLoIdF1'] = rollsDataForStands[0].brLoId;
                data['wrUpRolledLenF1'] = rollsDataForStands[0].wrUpRolledLen;
            }

            if (rollsDataForStands[1]) {
                data['brUpIdF2'] = rollsDataForStands[1].brUpId;
                data['wrUpIdF2'] = rollsDataForStands[1].wrUpId;
                data['wrLoIdF2'] = rollsDataForStands[1].wrLoId;
                data['brLoIdF2'] = rollsDataForStands[1].brLoId;
                data['wrUpRolledLenF2'] = rollsDataForStands[1].wrUpRolledLen;
            }

            if (rollsDataForStands[2]) {
                data['brUpIdF3'] = rollsDataForStands[2].brUpId;
                data['wrUpIdF3'] = rollsDataForStands[2].wrUpId;
                data['wrLoIdF3'] = rollsDataForStands[2].wrLoId;
                data['brLoIdF3'] = rollsDataForStands[2].brLoId;
                data['wrUpRolledLenF3'] = rollsDataForStands[2].wrUpRolledLen;
            }
            if (rollsDataForStands[3]) {
                data['brUpIdF4'] = rollsDataForStands[3].brUpId;
                data['wrUpIdF4'] = rollsDataForStands[3].wrUpId;
                data['wrLoIdF4'] = rollsDataForStands[3].wrLoId;
                data['brLoIdF4'] = rollsDataForStands[3].brLoId;
                data['wrUpRolledLenF4'] = rollsDataForStands[3].wrUpRolledLen;
            }
            if (rollsDataForStands[4]) {
                data['brUpIdF5'] = rollsDataForStands[4].brUpId;
                data['wrUpIdF5'] = rollsDataForStands[4].wrUpId;
                data['wrLoIdF5'] = rollsDataForStands[4].wrLoId;
                data['brLoIdF5'] = rollsDataForStands[4].brLoId;
                data['wrUpRolledLenF5'] = rollsDataForStands[4].wrUpRolledLen;
            }
            if (rollsDataForStands[5]) {
                data['brUpIdF6'] = rollsDataForStands[5].brUpId;
                data['wrUpIdF6'] = rollsDataForStands[5].wrUpId;
                data['wrLoIdF6'] = rollsDataForStands[5].wrLoId;
                data['brLoIdF6'] = rollsDataForStands[5].brLoId;
                data['wrUpRolledLenF6'] = rollsDataForStands[5].wrUpRolledLen;
            }
        }

        const interstandCoolingBitmask = data.interstandCooling;
        if (interstandCoolingBitmask) {
            data['interstandCoolingBitmaskF1F2'] = interstandCoolingBitmask[0];
            data['interstandCoolingBitmaskF2F3'] = interstandCoolingBitmask[1];
            data['interstandCoolingBitmaskF3F4'] = interstandCoolingBitmask[2];
            data['interstandCoolingBitmaskF4F5'] = interstandCoolingBitmask[3];
            data['interstandCoolingBitmaskF5F6'] = interstandCoolingBitmask[4];
        }

        if (data) {
            super.dataToForm(data);
        }

    }

    get formGroup(): FormGroup {
        const f = new FormGroupEasyBuilder(this.formValidationMetadata, this.isReadOnly);
        f.addNumbers(
            'calculatedWeight',
            'endOfInGotFlag',
            'entryHeadThickness',
            'entryHeadWidth',
            'entryLength',
            'exitThk',
            'exitWidth',
            'gapTime',
            'innerDiameter',
            'length',
            'measuredWeight',
            'outPieceSeq',
            'outerDiameter',
            'rollingTime',
            'shiftId',
            'soakTime',
            'targetThickness',
            'targetWidth',
            'testCut',
            'trialFlag',
            'usedDefaultChemComp',
            'outPieceCnt',
            'destCodeId',
            'combinationNo',
            'interstandCoolingBitmask',
            'stripCrownTolPerc',
            'exitThkTolPerc',
            'exitWidthTolPerc',
            'exitTempTolPerc',
            'downcoilTempTolPerc',
            'targetWidthMeas',
            'targetWidthPtol',
            'targetWidthNtol',
            'targetThicknessMeas',
            'targetThicknessPtol',
            'targetThicknessNtol',
            'targetTempFmMeas',
            'targetTempFmPtol',
            'targetTempFmNtol',
            'targetTempDcMeas',
            'targetTempDcPtol',
            'targetTempDcNtol',
            'targetProfileMeas',
            'targetProfilePtol',
            'targetProfileNtol',
            'targetTempInterm',
            'targetTempIntermUpTol',
            'targetTempIntermLoTol',
            'exitThkAvg',
            'exitWdtAvg',
            'exitStripTempAvg',
            'downcoilTempAvg',
            'profileAvg',
            'stripEdgeDropAvg',
            'stripQbFlatnessAvg',
            'intermediateTempAvg',
            'exitThkMin',
            'exitWdtMin',
            'exitStripTempMin',
            'downcoilTempMin',
            'profileMin',
            'stripEdgeDropMin',
            'stripQbFlatnessMin',
            'intermediateTempMin',
            'exitThkMax',
            'exitWdtMax',
            'exitStripTempMax',
            'downcoilTempMax',
            'profileMax',
            'stripEdgeDropMax',
            'stripQbFlatnessMax',
            'intermediateTempMax',
            'exitThkStd',
            'exitWdtStd',
            'exitStripTempStd',
            'downcoilTempStd',
            'profileStd',
            'stripEdgeDropStd',
            'stripQbFlatnessStd',
            'intermediateTempStd',
            'wrUpRolledLenF1',
            'wrUpRolledLenF2',
            'wrUpRolledLenF3',
            'wrUpRolledLenF4',
            'wrUpRolledLenF5',
            'wrUpRolledLenF6'
        );

        f.addStrings(
            'crewId',
            'customerId',
            'gradeGroupId',
            'heatId',
            'jobId',
            'jobPieceSeq',
            'materialGradeId',
            'remark',
            'operator',
            'orderNumber',
            'orderPosition',
            'totalReduction',
            'outPieceArea',
            'brUpIdF1',
            'wrUpIdF1',
            'wrLoIdF1',
            'brLoIdF1',
            'brUpIdF2',
            'wrUpIdF2',
            'wrLoIdF2',
            'brLoIdF2',
            'brUpIdF3',
            'wrUpIdF3',
            'wrLoIdF3',
            'brLoIdF3',
            'brUpIdF4',
            'wrUpIdF4',
            'wrLoIdF4',
            'brLoIdF4',
            'brUpIdF5',
            'wrUpIdF5',
            'wrLoIdF5',
            'brLoIdF5',
            'brUpIdF6',
            'wrUpIdF6',
            'wrLoIdF6',
            'brLoIdF6',
            'interstandCoolingBitmaskF1F2',
            'interstandCoolingBitmaskF2F3',
            'interstandCoolingBitmaskF3F4',
            'interstandCoolingBitmaskF4F5',
            'interstandCoolingBitmaskF5F6'
        );

        f.addString('inPieceId', [Validators.required]);
        f.addString('outPieceId', [Validators.required]);
        f.addString('status', [Validators.required]);

        f.addDates(
            'productionStartDate',
            'productionStopDate',
            'revision'
        );
        f.addBooleans(
            'aimGaugeChanged',
            'useBaseGrade',
            'casterWedgeNotOk',
            'cobble',
            'cobblePrevention',
            'coilHeadTooHot',
            'coilSlippedInF1',
            'coilWeightOutsideCustomerTol',
            'coilingPyrometerNotWorking',
            'coilingTempOutsideCustomer',
            'coilingTemperatureChanged',
            'damagedTail',
            'descaleHighPressureNotOk',
            'edgeThicknessBelowCustomer',
            'finishingPyrometerNotWorking',
            'finishingTempOutsideCustomer',
            'foldedHead',
            'gradeChanged',
            'heatRidge',
            'orderNumberChanged',
            'processorWidthChanged',
            'slabInTunnelFurnaceTooLong',
            'slipPrevention',
            'thicknessGaugeNotWorking',
            'thicknessOutsideCustomerTol',
            'transitionWidth',
            'widthDips',
            'widthGaugeNotWorking',
            'widthOutsideCustomerTol',
            'widthWarning',
            'stripCrownIntolFlag',
            'exitThkIntolFlag',
            'exitWidthIntolFlag',
            'exitTempIntolFlag',
            'downcoilTempIntolFlag',
            'biteSprayPressureTooLowInF1'
        );
        return f.formGroup;
    }

    get formGroupConverters(): FormGroupUomConverterMap {
        const cm = new FormGroupUomConverterMap(this.form, this.uomValuePipe);

        cm.map = [
            { key: 'exitThk', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'entryHeadThickness', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetThickness', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetWidth', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'length', uomSI: 'm', uomUSCS: 'ft' },
            { key: 'entryLength', uomSI: 'm', uomUSCS: 'ft' },
            { key: 'calculatedWeight', uomSI: 'kg', uomUSCS: 'lb' },
            { key: 'measuredWeight', uomSI: 'kg', uomUSCS: 'lb' },
            { key: 'entryHeadWidth', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'exitWidth', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'innerDiameter', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'outerDiameter', uomSI: 'mm', uomUSCS: 'in' },

            { key: 'targetWidthMeas', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetWidthPtol', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetWidthNtol', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'exitWdtAvg', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'exitWdtMin', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'exitWdtMax', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'exitWdtStd', uomSI: 'mm', uomUSCS: 'in' },

            { key: 'targetThicknessMeas', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetThicknessPtol', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetThicknessNtol', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'exitThkAvg', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'exitThkMin', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'exitThkMax', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'exitThkStd', uomSI: 'mm', uomUSCS: 'in' },

            { key: 'targetTempFmMeas', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempFmPtol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempFmNtol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'exitStripTempAvg', uomSI: 'C', uomUSCS: 'F' },
            { key: 'exitStripTempMin', uomSI: 'C', uomUSCS: 'F' },
            { key: 'exitStripTempMax', uomSI: 'C', uomUSCS: 'F' },
            { key: 'exitStripTempStd', uomSI: 'C', uomUSCS: 'F' },

            { key: 'targetTempDcMeas', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempDcPtol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempDcNtol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'downcoilTempAvg', uomSI: 'C', uomUSCS: 'F' },
            { key: 'downcoilTempMin', uomSI: 'C', uomUSCS: 'F' },
            { key: 'downcoilTempMax', uomSI: 'C', uomUSCS: 'F' },
            { key: 'downcoilTempStd', uomSI: 'C', uomUSCS: 'F' },

            { key: 'targetProfileMeas', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetProfilePtol', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetProfileNtol', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'profileAvg', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'profileMin', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'profileMax', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'profileStd', uomSI: 'mm', uomUSCS: 'in' },

            { key: 'targetTempInterm', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempIntermLoTol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempIntermUpTol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'intermediateTempAvg', uomSI: 'C', uomUSCS: 'F' },
            { key: 'intermediateTempMin', uomSI: 'C', uomUSCS: 'F' },
            { key: 'intermediateTempMax', uomSI: 'C', uomUSCS: 'F' },
            { key: 'intermediateTempStd', uomSI: 'C', uomUSCS: 'F' },

        ];

        return cm;
    }

    get formToEntityForInsert(): ProducedCoilForInsert {
        const rawValues: (keyof ProducedCoilForInsert)[] = [
            'outPieceNo',
            'outPieceId',
            'outPieceCnt',
            'outPieceSeq',
            'inPieceId',
            'jobId',
            'productionStartDate',
            'productionStopDate',
            'measuredWeight',
            'innerDiameter',
            'outerDiameter',
            'crewId',
            'shiftId',
            'status',
            'remark',
            'soakTime',
            'gapTime',
            'rollingTime',
            'stripCrownTolPerc',
            'exitThkTolPerc',
            'exitWidthTolPerc',
            'exitTempTolPerc',
            'downcoilTempTolPerc',
            'operator',
            'revision',
            'outPieceArea',
            'destCodeId',
            'combinationNo'
        ];
        const convertedValues: (keyof ProducedCoilForInsert)[] = [
            'exitWidth',
            'exitThk',
            'targetThickness',
            'targetWidth',
            'length',
            'calculatedWeight'

        ];
        const options = this.getFormValuesForInsert(rawValues, convertedValues);
        options.dispositionCodesForHold = [
            this.form.get('biteSprayPressureTooLowInF1').value,
            this.form.get('aimGaugeChanged').value,
            this.form.get('cobble').value,
            this.form.get('cobblePrevention').value,
            this.form.get('coilingTempOutsideCustomer').value,
            this.form.get('casterWedgeNotOk').value,
            this.form.get('damagedTail').value,
            this.form.get('foldedHead').value,
            this.form.get('finishingTempOutsideCustomer').value,
            this.form.get('thicknessOutsideCustomerTol').value,
            this.form.get('gradeChanged').value,
            this.form.get('coilHeadTooHot').value,
            this.form.get('descaleHighPressureNotOk').value,
            this.form.get('heatRidge').value,
            this.form.get('coilingPyrometerNotWorking').value,
            this.form.get('finishingPyrometerNotWorking').value,
            this.form.get('widthGaugeNotWorking').value,
            this.form.get('thicknessGaugeNotWorking').value,
            this.form.get('orderNumberChanged').value,
            this.form.get('edgeThicknessBelowCustomer').value,
            this.form.get('processorWidthChanged').value,
            this.form.get('coilSlippedInF1').value,
            this.form.get('slipPrevention').value,
            this.form.get('coilingTemperatureChanged').value,
            this.form.get('slabInTunnelFurnaceTooLong').value,
            this.form.get('transitionWidth').value,
            this.form.get('widthDips').value,
            this.form.get('widthOutsideCustomerTol').value,
            this.form.get('coilWeightOutsideCustomerTol').value,
            this.form.get('widthWarning').value
        ];

        options.endOfInGotFlag = this.transformBooleanToInt(this.form.get('endOfInGotFlag').value),
            options.testCut = this.transformBooleanToInt(this.form.get('testCut').value),
            options.trialFlag = this.transformBooleanToInt(this.form.get('trialFlag').value),
            options.stripCrownIntolFlag = this.transformBooleanToInt(this.form.get('stripCrownIntolFlag').value),
            options.exitThkIntolFlag = this.transformBooleanToInt(this.form.get('exitThkIntolFlag').value),
            options.exitWidthIntolFlag = this.transformBooleanToInt(this.form.get('exitWidthIntolFlag').value),
            options.exitTempIntolFlag = this.transformBooleanToInt(this.form.get('exitTempIntolFlag').value),
            options.downcoilTempIntolFlag = this.transformBooleanToInt(this.form.get('downcoilTempIntolFlag').value);

        //     // TODO: other processing?

        return new ProducedCoilForInsert(options);
    }

    get formToEntityForUpdate(): ProducedCoilForUpdate {
        const rawValues: (keyof ProducedCoilForUpdate)[] = [
            'outPieceId',
            'outPieceSeq',
            'outPieceNo',
            'inPieceId',
            'jobId',
            'productionStartDate',
            'productionStopDate',
            'measuredWeight',
            'innerDiameter',
            'outerDiameter',
            'crewId',
            'shiftId',
            'remark',
            'soakTime',
            'gapTime',
            'rollingTime',
            'operator',
            'revision',
            'interstandCoolingBitmask',
            'status'
        ];
        const convertedValues: (keyof ProducedCoilForUpdate)[] = [
            'exitWidth',
            'exitThk',
            'targetThickness',
            'targetWidth',
            'length',
            'calculatedWeight'
        ];
        const options = this.getFormValuesForUpdate(rawValues, convertedValues);

        options.dispositionCodesForHold = [
            this.form.get('biteSprayPressureTooLowInF1').value,
            this.form.get('aimGaugeChanged').value,
            this.form.get('cobble').value,
            this.form.get('cobblePrevention').value,
            this.form.get('coilingTempOutsideCustomer').value,
            this.form.get('casterWedgeNotOk').value,
            this.form.get('damagedTail').value,
            this.form.get('foldedHead').value,
            this.form.get('finishingTempOutsideCustomer').value,
            this.form.get('thicknessOutsideCustomerTol').value,
            this.form.get('gradeChanged').value,
            this.form.get('coilHeadTooHot').value,
            this.form.get('descaleHighPressureNotOk').value,
            this.form.get('heatRidge').value,
            this.form.get('coilingPyrometerNotWorking').value,
            this.form.get('finishingPyrometerNotWorking').value,
            this.form.get('widthGaugeNotWorking').value,
            this.form.get('thicknessGaugeNotWorking').value,
            this.form.get('orderNumberChanged').value,
            this.form.get('edgeThicknessBelowCustomer').value,
            this.form.get('processorWidthChanged').value,
            this.form.get('coilSlippedInF1').value,
            this.form.get('slipPrevention').value,
            this.form.get('coilingTemperatureChanged').value,
            this.form.get('slabInTunnelFurnaceTooLong').value,
            this.form.get('transitionWidth').value,
            this.form.get('widthDips').value,
            this.form.get('widthOutsideCustomerTol').value,
            this.form.get('coilWeightOutsideCustomerTol').value,
            this.form.get('widthWarning').value
        ];

        options.endOfInGotFlag = this.transformBooleanToInt(this.form.get('endOfInGotFlag').value),
            options.testCut = this.transformBooleanToInt(this.form.get('testCut').value),
            options.trialFlag = this.transformBooleanToInt(this.form.get('trialFlag').value),
            options.stripCrownIntolFlag = this.transformBooleanToInt(this.form.get('stripCrownIntolFlag').value),
            options.exitThkIntolFlag = this.transformBooleanToInt(this.form.get('exitThkIntolFlag').value),
            options.exitWidthIntolFlag = this.transformBooleanToInt(this.form.get('exitWidthIntolFlag').value),
            options.exitTempIntolFlag = this.transformBooleanToInt(this.form.get('exitTempIntolFlag').value),
            options.downcoilTempIntolFlag = this.transformBooleanToInt(this.form.get('downcoilTempIntolFlag').value);

        return new ProducedCoilForUpdate(options);

    }

    get newEntityDetail(): Observable<Partial<ProducedCoilDetail>> {
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
            this.form.markAllAsTouched();
            return;
        }
        if (this.isNew) {
            // create new entity
            const entityForInsert = this.formToEntityForInsert;
            this.sanitizeId();
            entityForInsert.outPieceNo = this.id; // FIXME verify entity's key
            this.producedCoilApiService.create(entityForInsert).subscribe(
                (result) => this.goBack(),
                (error) => this.processServerErrorMap(error)
            );
        } else {
            // update existing entity
            const entityForUpdate = this.formToEntityForUpdate;
            this.sanitizeId();
            entityForUpdate.outPieceNo = this.id; // FIXME verify entity's key
            this.producedCoilApiService.update(this.id, entityForUpdate).subscribe(
                (result) => this.goBack()
                // (error) => this.processServerErrorMap(error)
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

        this.producedCoilApiService.delete(this.id).subscribe(
            (result) => this.goBack()
        );
    }

    private transformBooleanToInt(value: number) {
        const result = value || value === 1 ? 1 : 0;
        return result;
    }

    public onGenerateIdClicked(event: KeyboardEvent) {
        this.openNotImplementedDialog(ProducedCoilDetailComponent, 'onGenerateIdClicked');
        event.preventDefault();
    }

    private sanitizeId() {
        // if id = NaN backend side the controller crashes
        if (isNaN(this.id)) {
            this.id = undefined;
        }
    }

    private conLogCalled(callerName: string) {
        this.log.debug(`${callerName}() CALLED`);
    }
    private conLogCalledId(callerName: string, id: number) {
        this.log.debug(`${callerName}: NEXT id=${id}`);
    }

    private createinputSlabList() {
        this.conLogCalled('createJobListCbi');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.producedCoilApiService.inputSlabList.subscribe(
                    (list) => obs.next(list),
                    (err) => obs.next([])
                )
            );
        });
    }

    // Create ComboBox item list
    private createCoilStatusList() {
        this.conLogCalled('coilStatusList');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.idSubject.subscribe((id) => {
                    this.conLogCalledId('coilStatusList', id);
                    this.auxValueApiService.coilStatusList.subscribe(
                        (list) => obs.next(list)
                    );
                }),
            );
        });
    }

    // Create ComboBox item list
    private createShiftIdList() {
        this.conLogCalled('shiftIdList');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.idSubject.subscribe((id) => {
                    this.conLogCalledId('shiftIdList', id);
                    this.auxValueApiService.shiftIdList.subscribe(
                        (list) => obs.next(list)
                    );
                }),
            );
        });
    }

    // Create ComboBox item list
    private createInnerDiameterList() {
        this.conLogCalled('innerDiameterList');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.idSubject.subscribe((id) => {
                    this.conLogCalledId('innerDiameterList', id);
                    this.auxValueApiService.innerDiameterList.subscribe(
                        (list) => obs.next(list)
                    );
                }),
            );
        });
    }

    // Create ComboBox item list
    private createCrewIdList() {
        this.conLogCalled('innerDiameterList');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.idSubject.subscribe((id) => {
                    this.conLogCalledId('innerDiameterList', id);
                    this.rmlCrewApiService.crewIdList.subscribe(
                        (list) => obs.next(list)
                    );
                }),
            );
        });
    }

}

