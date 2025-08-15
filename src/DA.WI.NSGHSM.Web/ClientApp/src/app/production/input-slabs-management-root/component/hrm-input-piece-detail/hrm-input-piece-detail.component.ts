// hrm-input-piece-detail.component.ts
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
    AuxValueService,
    BaseEntityDetailComponent,
    ComboBoxItemListBuilder,
    ComboBoxItemNumberString,
    ComboBoxItemStringString,
    HrmAnalysisDataApiService,
    HrmAnalysisDataListFilter,
    HrmHeatApiService,
    HrmHeatListFilter,
    HrmInputPiece,
    HrmInputPieceApiService,
    HrmInputPieceBase,
    HrmInputPieceDetail,
    HrmInputPieceForInsert,
    HrmInputPieceForUpdate,
    HrmInputPieceListFilter,
    HrmInputPieceListItem,
    HrmJobApiService,
    TdbAlloyApiService,
    TdbAlloySpecApiService,
    TdbGradeGroupApiService,
    TdbGradeGroupListFilter,
    TdbMaterialGradeApiService,
    TdbMaterialGradeListFilter,
    TdbMaterialSpecApiService,
    TdbMaterialSpecListFilter,
    TdbProcessCodeApiService,
    FormGroupUomConverterMap,
    UomValuePipe,
    FormGroupEasyBuilder,
    TabstripTabMetadata,
    AppStatusStoreService,
} from '@app/shared';
import { ConfirmService } from '@app/widget';
import { StringsListManagementChildState } from '../../../strings-list-management-root';
import {
    ChemRowData,
    ChemRowDataList,
    ChemicalCompositionIndex,
    ChemicalCompositionViewModelHelper,
    ChemicalLabels,
} from './view-model-extra';

const chemicalLabels: ChemicalLabels = {
    aluminium: 'Aluminium - Al [%]',
    boron: 'Boron - B [%]',
    calcium: 'Calcium - Ca [%]',
    carbon: 'Carbon - C [%]',
    chromium: 'Chromium - Cr [%]',
    cobalt: 'Cobalt - Co [%]',
    copper: 'Copper - Cu [%]',
    lead: 'Lead - Pb [%]',
    manganese: 'Manganese - Mn [%]',
    molybdenum: 'Molybdenum - Mo [%]',
    nickel: 'Nichel - Ni [%]',
    niobium: 'Niobium - Nb [%]',
    nitrogen: 'Nitrogen - N [%]',
    phosphorus: 'Phosphorus - P [%]',
    silicon: 'Silicon - Si [%]',
    sulphur: 'Sulphur - S [%]',
    tin: 'Tin - Sn [%]',
    titanium: 'Titanium - Ti [%]',
    tungsten: 'Tungsten - W [%]',
    vanadium: 'Vanadium - V [%]',
};

@Component({
    selector: 'app-hrm-input-piece-detail',
    templateUrl: './hrm-input-piece-detail.component.html',
    styleUrls: ['./hrm-input-piece-detail.component.scss']
})
export class HrmInputPieceDetailComponent
    extends BaseEntityDetailComponent<
    HrmInputPieceBase,
    HrmInputPiece,
    HrmInputPieceDetail,
    HrmInputPieceForInsert,
    HrmInputPieceForUpdate,
    HrmInputPieceListItem,
    HrmInputPieceListFilter,
    HrmInputPieceApiService>
    implements OnInit, OnDestroy {
    public readonly modelOptions = {
        standalone: true,
    };

    public childState: StringsListManagementChildState;
    @Input() public parentTab: TabstripTabMetadata<HrmInputPieceListItem>;

    public readonly jobList: Observable<ComboBoxItemStringString[]>;
    public readonly statusList: Observable<ComboBoxItemNumberString[]>;
    public readonly heatList: Observable<ComboBoxItemNumberString[]>;
    public readonly transitionList: Observable<ComboBoxItemNumberString[]>;
    public readonly sourceProcessList: Observable<ComboBoxItemNumberString[]>;
    public readonly destinationProcessList: Observable<ComboBoxItemNumberString[]>;

    public readonly materialGradeIdList: Observable<ComboBoxItemNumberString[]>;
    public readonly materialSpecificationIdList: Observable<ComboBoxItemNumberString[]>;
    public readonly areaTypeList: Observable<ComboBoxItemStringString[]>;
    public readonly targetInternalDiameterList: Observable<ComboBoxItemNumberString[]>;
    private selectedChemMaterialGradeId: number;

    public readonly gradeGroupList: Observable<ComboBoxItemNumberString[]>;

    public readonly analysisSampleList: Observable<ComboBoxItemNumberString[]>;
    private selectedChemAnalysisSampleId: number;
    private mostRecentChemAnalysisSampleId: number;

    public vmHelper = new ChemicalCompositionViewModelHelper();



    public get chemElement() {
        return chemicalLabels;
    }

    public chem = {
        laboratoryAnalysisItems: new ChemRowDataList(chemicalLabels),
        steelGradeItems: new ChemRowDataList(chemicalLabels),
        materialSpecificationItems: new ChemRowDataList(chemicalLabels),
    };

    private get heatNo(): number {
        return this.getFormValue('heatNo');
    }

    private get jobId(): string {
        return this.getFormValue('jobId');
    }

    private get alloySpecCnt(): number {
        return this.getFormValue('alloySpecCnt');
    }

    private get alloyCodeCore(): number {
        return this.getFormValue('alloyCodeCore');
    }

    constructor(
        formBuilder: FormBuilder,
        appUserInfoService: AppUserInfoService,
        confirmService: ConfirmService,
        log: LogService,
        validationsDecoder: ValidationsDecoderService,
        authService: AuthService,
        private auxValueApiService: AuxValueService,
        private hrmJobApiService: HrmJobApiService,
        private hrmInputPieceApiService: HrmInputPieceApiService,
        private hrmHeatApiService: HrmHeatApiService,
        private hrmAnalysisDataApiService: HrmAnalysisDataApiService,
        private tdbAlloyApiService: TdbAlloyApiService,
        private tdbAlloySpecApiService: TdbAlloySpecApiService,
        private tdbMaterialGradeApiService: TdbMaterialGradeApiService,
        private tdbMaterialSpecApiService: TdbMaterialSpecApiService,
        private tdbProcessCodeApiService: TdbProcessCodeApiService,
        private tdbGradeGroupApiService: TdbGradeGroupApiService,
        private translateService: TranslateService,
        uomValuePipe: UomValuePipe,
    ) {
        super(formBuilder, log, authService, appUserInfoService,
            confirmService, validationsDecoder, 'EXAMPLE', 'HrmInputPiece', uomValuePipe );


        // Create Observables for ComboBox lists
        this.jobList = this.createJobListCbi();
        this.statusList = this.createStatusListCbi();
        this.heatList = this.createHeatListCbi();
        this.transitionList = this.createTransitionListCbi();
        this.sourceProcessList = this.createSourceProcessListCbi();
        this.destinationProcessList = this.createDestinationProcessListCbi();
        this.materialGradeIdList = this.createMaterialGradeIdListCbi();
        this.materialSpecificationIdList = this.createMaterialSpecificationIdList();
        this.targetInternalDiameterList = this.createTargetInternalDiameterList();
        this.areaTypeList = this.createAreaTypeList();
        this.gradeGroupList = this.createGroupGradeListCbi();
        this.analysisSampleList = this.createAnalysisSampleListCbi();
    }

    get service() {
        return this.hrmInputPieceApiService;
    }

    get formGroup(): FormGroup {
        // disabled always
        this.disabledControlNames.push(
            'heatSeq',
            'orderNumber',
            'targetExitTempResultMax',
            'targetTempDCResultMin',
            'targetTempDCResultMax',
            'targetProfileResultMin',
            'targetProfileResultMax',
            'targetFlatnessResultMin',
            'targetFlatnessResultMax'
        );
        // disabled on edit only
        if (this.isEdit) {
            this.disabledControlNames.push(
                'jobId',
            );
        }
        const f = new FormGroupEasyBuilder(this.formValidationMetadata, this.isReadOnly);
        f.addNumbers(
            'alloyCodeCore',
            'alloySpecCnt',
            'alloySpecVersion',
            'enduseSurfaceRating',
            'entryTemp',
            'exitWidthTolPerc',
            'furnaceDischargeTemp',
            'heatNo',
            'heatPieceSeq',
            'heatSeq',
            'jobPieceSeq',
            'length',
            'materialSpecNo',
            'measuredTemp',
            'measuredWidthHead',
            'measuredWidthTail',
            'pieceNo',
            'pieceSeq',
            'preliminaryWdtChg',
            'sourceCodeId',
            'targetExitTemp',
            'targetExitTempNtol',
            'targetExitTempPtol',
            'targetExitTempResultMax',
            'targetExitThicknessResultMax',
            'targetExitThicknessResultMin',
            'targetExitWeightResultMax',
            'targetExitWeightResultMin',
            'targetExitWidthResultMax',
            'targetExitWidthResultMin',
            'targetExitTempResultMin',
            'targetFlatness',
            'targetFlatnessCustomertol',
            'targetFlatnessNtol',
            'targetFlatnessPtol',
            'targetFlatnessResultMax',
            'targetFlatnessResultMin',
            'targetProfile',
            'targetProfileCustomertol',
            'targetProfileNtol',
            'targetProfilePtol',
            'targetProfileResultMax',
            'targetProfileResultMin',
            'targetTempDC',
            'targetTempDCCustomertol',
            'targetTempDCNtol',
            'targetTempDCPtol',
            'targetTempDCResultMax',
            'targetTempDCResultMin',
            'targetTempFmCustomertol',
            'targetThicknessNtol',
            'targetThicknessPtol',
            'targetWeight',
            'targetWeightNtol',
            'targetWeightPtol',
            'targetWidthNtol',
            'targetWidthPtol',
            'useBaseGrade'
        );
        f.addStrings(
            'areaType',
            'baseGradeId',
            'carrierMode',
            'customerContactName',
            'customerName',
            'endUse',
            'gradeGroupLabel',
            'heatId',
            'materialGradeId',
            'operator',
            'orderNumber',
            'orderPosition',
            'preliminaryMatGradeId',
            'remark',
            'sampleId',
            'trialFlag',
            'trialNo'
        );
        f.addNumber('status', [Validators.required]);
        f.addNumber('transition', [Validators.required]);
        f.addString('pieceId', [Validators.required]);
        f.addString('jobId', [Validators.required]);
        f.addString('destCodeId', [Validators.required]);
        f.addNumber('preliminaryLen', [Validators.required]);
        f.addNumber('preliminaryThk', [Validators.required]);
        f.addNumber('preliminaryThkHead', [Validators.required]);
        f.addNumber('preliminaryThkTail', [Validators.required]);
        f.addNumber('preliminaryWdt', [Validators.required]);
        f.addNumber('weight', [Validators.required]);
        f.addNumber('preliminaryWdtHead', [Validators.required]);
        f.addNumber('preliminaryWdtTail', [Validators.required]);
        f.addNumber('gradeGroupId', [Validators.required]);
        f.addString('alloySpecCode', [Validators.required]);
        f.addString('materialSpecId', [Validators.required]);
        f.addNumber('targetThickness', [Validators.required]);
        f.addNumber('targetWidth', [Validators.required]);
        f.addNumber('gradeGroupId', [Validators.required]);
        f.addNumber('gradeGroupId', [Validators.required]);
        f.addNumber('gradeGroupId', [Validators.required]);
        f.addString('customerId', [Validators.required]);
        f.addNumber('targetInternalDiameter', [Validators.required]);

        f.addDates(
            'revision'
        );
        f.addBooleans(
            'trialFlag',
            'useBaseGrade',
            'useMeasTemp',
            'useMeasWidth',
            'useDefaultSteelGrade',
        );
        return f.formGroup;
    }

    get formGroupConverters(): FormGroupUomConverterMap {
        const cm = new FormGroupUomConverterMap(this.form, this.uomValuePipe);

        cm.map = [
            { key: 'preliminaryLen', uomSI: 'm', uomUSCS: 'ft' },
            { key: 'targetThickness', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetWidth', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetInternalDiameter', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'preliminaryThk', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'preliminaryThkHead', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'preliminaryThkTail', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'weight', uomSI: 'kg', uomUSCS: 'lb' },
            { key: 'preliminaryWdt', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'preliminaryWdtHead', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'preliminaryWdtTail', uomSI: 'mm', uomUSCS: 'in' },
            { key: 'targetTempDCPtol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempDCResultMax', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempDCNtol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempDCResultMin', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetTempDC', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetExitTempPtol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetExitTempResultMax', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetExitTempNtol', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetExitTempResultMin', uomSI: 'C', uomUSCS: 'F' },
            { key: 'targetExitTemp', uomSI: 'C', uomUSCS: 'F' },
            { key: 'furnaceDischargeTemp', uomSI: 'C', uomUSCS: 'F' },

        ];

        return cm;
    }

    get formToEntityForInsert(): HrmInputPieceForInsert {
        const rawValues: (keyof HrmInputPieceForInsert)[] = [
            'jobId',
            'jobPieceSeq',
            'materialGradeId',
            'pieceId',
            'pieceNo',
            'status',
            'destCodeId',
            'heatNo',
            'sourceCodeId',
            'useMeasTemp',
            'useMeasWidth',
            'remark',
            'gradeGroupId',
            'baseGradeId',
            'useBaseGrade',
            'furnaceDischargeTemp',
            'targetFlatness',
            'targetFlatnessNtol',
            'targetFlatnessCustomertol',
            'targetFlatnessPtol',
            'alloySpecCode',
            'materialSpecId',
            'trialFlag',
            'customerId',
            'customerName',
            'customerContactName',
            'carrierMode',
            'endUse',
            'enduseSurfaceRating',
            'areaType'
        ];
        const convertedValues: (keyof HrmInputPieceForInsert)[] = [
            'preliminaryLen',
            'targetThickness',
            'targetWidth',
            'targetInternalDiameter',
            'preliminaryThk',
            'preliminaryThkHead',
            'preliminaryThkTail',
            'weight',
            'preliminaryWdt',
            'preliminaryWdtTail',
            'preliminaryWdtHead',
            'preliminaryWdtChg',
            'entryTemp',
            'measuredTemp',
            'measuredWidthHead',
            'measuredWidthTail',
            'targetExitTemp',
            'targetExitTempNtol',
            'targetTempFmCustomertol',
            'targetExitTempPtol',
            'targetTempDC',
            'targetTempDCNtol',
            'targetTempDCCustomertol',
            'targetTempDCPtol',
            'targetProfile',
            'targetProfileNtol',
            'targetProfileCustomertol',
            'targetProfilePtol',
            'targetThicknessNtol',
            'targetThicknessPtol',
            'targetWidthNtol',
            'targetWidthPtol',
            'targetWeight',
            'targetWeightNtol',
            'targetWeightPtol'
        ];
        const options = this.getFormValuesForInsert(rawValues, convertedValues);

        // TODO: other processing?

        return new HrmInputPieceForInsert(options);
    }
    get formToEntityForUpdate(): HrmInputPieceForUpdate {
        const rawValues: (keyof HrmInputPieceForUpdate)[] = [
            'jobId',
            'jobPieceSeq',
            'materialGradeId',
            'pieceId',
            'pieceNo',
            'status',
            'destCodeId',
            'heatNo',
            'sourceCodeId',
            'useMeasTemp',
            'useMeasWidth',
            'remark',
            'gradeGroupId',
            'baseGradeId',
            'useBaseGrade',
            'furnaceDischargeTemp',
            'targetFlatness',
            'targetFlatnessNtol',
            'targetFlatnessCustomertol',
            'targetFlatnessPtol',
            'alloySpecCode',
            'materialSpecId',
            'trialFlag',
            'customerId',
            'customerName',
            'customerContactName',
            'carrierMode',
            'endUse',
            'enduseSurfaceRating',
            'areaType'
        ];
        const convertedValues: (keyof HrmInputPieceForUpdate)[] = [
            'preliminaryLen',
            'targetThickness',
            'targetWidth',
            'targetInternalDiameter',
            'preliminaryThk',
            'preliminaryThkHead',
            'preliminaryThkTail',
            'weight',
            'preliminaryWdt',
            'preliminaryWdtTail',
            'preliminaryWdtHead',
            'preliminaryWdtChg',
            'entryTemp',
            'measuredTemp',
            'measuredWidthHead',
            'measuredWidthTail',
            'targetExitTemp',
            'targetExitTempNtol',
            'targetTempFmCustomertol',
            'targetExitTempPtol',
            'targetTempDC',
            'targetTempDCNtol',
            'targetTempDCCustomertol',
            'targetTempDCPtol',
            'targetProfile',
            'targetProfileNtol',
            'targetProfileCustomertol',
            'targetProfilePtol',
            'targetThicknessNtol',
            'targetThicknessPtol',
            'targetWidthNtol',
            'targetWidthPtol',
            'targetWeight',
            'targetWeightNtol',
            'targetWeightPtol'
        ];
        const options = this.getFormValuesForUpdate(rawValues, convertedValues);

        // TODO: other processing?

        return new HrmInputPieceForUpdate(options);
    }
    get newEntityDetail(): Observable<Partial<HrmInputPieceDetail>> {
        return this.newEntityDetailConstant({
            heatSeq: 0,
            status: 5,
        });
    }

    ngOnInit(): void {
        this.subscribe(
            this.vmHelper.visible.subscribe(
                (flag) => this.changeChemicalCompositionVisibility(flag)
            ),
            this.vmHelper.index.subscribe(
                (index) => this.changeChemicalCompositionIndex(index)
            ),
            this.vmHelper.defaultSteelGradeUsed.subscribe(
                (flag) => this.changeChemicalCompositionDefaultSteelGrade(flag)
            ),
        );
        this.detailInit().subscribe((ok) => {
            if (ok) {
                this.subscribe(
                    this.subscribeTargetExitTempMin,
                    this.subscribeTargetExitTempMax,
                    this.subscribeTargetTempDCMin,
                    this.subscribeTargetTempDCMax,
                    this.subscribeTargetProfileMin,
                    this.subscribeTargetProfileMax,
                    this.subscribeTargetFlatnessMin,
                    this.subscribeTargetFlatnessMax,
                    this.subscribeTargetExitThicknessMin,
                    this.subscribeTargetExitThicknessMax,
                    this.subscribeTargetExitWidthMin,
                    this.subscribeTargetExitWidthMax,
                    this.subscribeTargetExitWeightMin,
                    this.subscribeTargetExitWeightMax,
                );
            }
        });

    }

    private subscribeControlsOp(
        controlToSubscribe: string,
        controlToGet: string,
        controlToSet: string,
        op: (gotValue: number, subscribedValue: number) => number
    ) {
        return this.form.get(controlToSubscribe).valueChanges.subscribe((currentValue) => {
            const computedValue = op(this.form.get(controlToGet).value, currentValue);
            this.form.patchValue({ [controlToSet]: computedValue });
        });
    }
    private subscribeControlsAdd(controlToSubscribe: string, controlToGet: string, controlToSet: string) {
        const op = (gotValue: number, subscribedValue: number) => gotValue + subscribedValue;
        return this.subscribeControlsOp(controlToSubscribe, controlToGet, controlToSet, op);
    }
    private subscribeControlsSub(controlToSubscribe: string, controlToGet: string, controlToSet: string) {
        const op = (gotValue: number, subscribedValue: number) => gotValue - subscribedValue;
        return this.subscribeControlsOp(controlToSubscribe, controlToGet, controlToSet, op);
    }

    private get subscribeTargetExitWeightMax() {
        return this.subscribeControlsAdd('targetWeightPtol', 'targetWeight', 'targetExitWeightResultMax');
    }

    private get subscribeTargetExitWeightMin() {
        return this.subscribeControlsSub('targetWeightNtol', 'targetWeight', 'targetExitWeightResultMin');
    }

    private get subscribeTargetExitWidthMax() {
        return this.subscribeControlsAdd('targetWidthPtol', 'targetWidth', 'targetExitWidthResultMax');
    }

    private get subscribeTargetExitWidthMin() {
        return this.subscribeControlsSub('targetWidthNtol', 'targetWidth', 'targetExitWidthResultMin');
    }

    private get subscribeTargetExitThicknessMax() {
        return this.subscribeControlsAdd('targetThicknessPtol', 'targetThickness', 'targetExitThicknessResultMax');
    }

    private get subscribeTargetExitThicknessMin() {
        return this.subscribeControlsSub('targetThicknessNtol', 'targetThickness', 'targetExitThicknessResultMin');
    }

    private get subscribeTargetExitTempMin() {
        return this.subscribeControlsSub('targetExitTempNtol', 'targetExitTemp', 'targetExitTempResultMin');
    }

    private get subscribeTargetExitTempMax() {
        return this.subscribeControlsAdd('targetExitTempPtol', 'targetExitTemp', 'targetExitTempResultMax');
    }

    private get subscribeTargetTempDCMin() {
        return this.subscribeControlsSub('targetTempDCNtol', 'targetTempDC', 'targetTempDCResultMin');
    }

    private get subscribeTargetTempDCMax() {
        return this.subscribeControlsAdd('targetTempDCPtol', 'targetTempDC', 'targetTempDCResultMax');
    }

    private get subscribeTargetProfileMin() {
        return this.subscribeControlsSub('targetProfileNtol', 'targetProfile', 'targetProfileResultMin');
    }

    private get subscribeTargetProfileMax() {
        return this.subscribeControlsAdd('targetProfilePtol', 'targetProfile', 'targetProfileResultMax');
    }

    private get subscribeTargetFlatnessMin() {
        return this.subscribeControlsSub('targetFlatnessNtol', 'targetFlatness', 'targetFlatnessResultMin');
    }

    private get subscribeTargetFlatnessMax() {
        return this.subscribeControlsSub('targetFlatnessPtol', 'targetFlatness', 'targetFlatnessResultMax');
    }

    onSaveClick() {
        if (!this.form.valid || this.isReadOnly === true) {
            this.form.markAllAsTouched();
            return;
        }
        if (this.isNew) {
            // create new HrmInputPiece
            this.sanitizeId();
            const entityForInsert = this.formToEntityForInsert;
            entityForInsert.pieceNo = this.id;
            this.hrmInputPieceApiService.create(entityForInsert).subscribe(
                (result) => this.goBack(),
                (error) => this.processServerErrorMap(error)
            );
        } else {
            // update existing HrmInputPiece
            this.sanitizeId();
            const entityForUpdate = this.formToEntityForUpdate;
            entityForUpdate.pieceNo = this.id;
            this.hrmInputPieceApiService.update(this.id, entityForUpdate).subscribe(
                (result) => this.goBack(),
                (error) => this.processServerErrorMap(error)
            );
        }
    }

    onDeleteClick() {
        const title = 'Delete Input Slab';
        const question = this.translateService.instant(this.areYouSureToDeleteThisItem);
        this.deleteConfirm(title, question).subscribe((deleted) => {
            if (deleted) {
                this.goBack();
            }
        });
    }

    // TAB: Laboratory Analysis
    // ROW: 1
    // COL: Sample ID
    // POS: RX
    public changeLaboratoryAnalysisSampleId(sampleId: number) {
        this.log.debug(`changeLaboratoryAnalysisSampleId(sampleId=${sampleId})`);
        this.selectedChemAnalysisSampleId = sampleId;
        this.updateChemAnalysisSampleData();
    }

    // TAB: Input Piece Data
    // ROW: 1
    // COL: Entry Length [mm]
    // POS: RX
    public ipdCompute(event: MouseEvent) {
        alert(`Not implemented yet!`);
        event.preventDefault();
    }

    private changeChemicalCompositionVisibility(visible: boolean) {
        // TODO
        // tslint:disable-next-line
        this.log.debug('changeChemicalCompositionVisibility called');
    }
    private changeChemicalCompositionIndex(index: ChemicalCompositionIndex) {
        // tslint:disable-next-line
        this.log.debug('changeChemicalCompositionIndex called');
        switch (index) {
            case ChemicalCompositionIndex.LaboratoryAnalysis:
                this.updateChemAnalysisSampleData();
                break;
            case ChemicalCompositionIndex.SteelGrade:
                this.updateChemSteelGradeData();
                break;
            case ChemicalCompositionIndex.MaterialSpecification:
                this.updateChemMaterialSpecificationData();
                break;
            default:
        }
    }
    private changeChemicalCompositionDefaultSteelGrade(used: boolean) {
        // TODO
        // tslint:disable-next-line
        this.log.debug('changeChemicalCompositionDefaultSteelGrade called');
    }

    // .................................................................

    private conLogCalled(callerName: string) {
        this.log.debug(`${callerName}() CALLED`);
    }
    private conLogCalledId(callerName: string, id: number) {
        this.log.debug(`${callerName}: NEXT id=${id}`);
    }

    // .................................................................

    private createJobListCbi() {
        this.conLogCalled('createJobListCbi');
        return new Observable<ComboBoxItemStringString[]>((obs) => {
            this.subscribe(
                this.hrmJobApiService.entityLookupList.subscribe(
                    (list) => obs.next(list),
                    (err) => obs.next([])
                )
            );
        });
    }

    // Create ComboBox item list
    private createStatusListCbi() {
        this.conLogCalled('createStatusListCbi');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.idSubject.subscribe((id) => {
                    this.conLogCalledId('createStatusListCbi', id);
                    this.auxValueApiService.statusInputPieceList.subscribe(
                        (list) => obs.next(list)
                    );
                }),
            );
        });
    }
    // Create ComboBox item list
    private createHeatListCbi() {
        this.conLogCalled('createHeatListCbi');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.idSubject.subscribe((id) => {
                    this.conLogCalledId('createHeatListCbi', id);
                    const filter = new HrmHeatListFilter({ searchJobId: this.jobId, });
                    const listRequest = this.hrmHeatApiService.createListRequest<HrmHeatListFilter>(filter);
                    this.hrmHeatApiService.readList(listRequest).subscribe((listResult) => {
                        const list = ComboBoxItemListBuilder.build<any, number, string, ComboBoxItemNumberString>(
                            listResult.data, ComboBoxItemNumberString, 'heatNo', 'heatId');
                        obs.next(list);
                    });
                }),
            );
        });
    }
    // Create ComboBox item list
    private createTransitionListCbi() {
        this.conLogCalled('createTransitionListCbi');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.idSubject.subscribe((id) => {
                    this.conLogCalledId('createTransitionListCbi', id);
                    this.auxValueApiService.transitionList.subscribe((list) => obs.next(list));
                }),
            );
        });
    }
    // Create ComboBox item list
    private createSourceProcessListCbi() {
        this.conLogCalled('createSourceProcessListCbi');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.tdbProcessCodeApiService.readSourceProcessList().subscribe((listResult) => {
                const list = ComboBoxItemListBuilder.build<any, number, string, ComboBoxItemNumberString>(
                    listResult.data, ComboBoxItemNumberString, 'codeId', 'processCodeLabel');
                obs.next(list);
            });
        });
    }
    // Create ComboBox item list
    private createDestinationProcessListCbi() {
        this.conLogCalled('createDestinationProcessListCbi');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.tdbProcessCodeApiService.readDestinationProcessList().subscribe((listResult) => {
                const list = ComboBoxItemListBuilder.build<any, number, string, ComboBoxItemNumberString>(
                    listResult.data, ComboBoxItemNumberString, 'codeId', 'processCodeLabel');
                obs.next(list);
            });
        });
    }
    // Create ComboBox item list
    private createMaterialGradeIdListCbi() {
        this.conLogCalled('createMaterialGradeIdList');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            const filter = new TdbMaterialGradeListFilter();
            const listRequest = this.tdbMaterialGradeApiService.createListRequest(filter);
            this.tdbMaterialGradeApiService.readList(listRequest).subscribe((listResult) => {
                const list = ComboBoxItemListBuilder.build<any, number, string, ComboBoxItemNumberString>(
                    listResult.data, ComboBoxItemNumberString, 'alloyCodeCore', 'materialGradeId');
                obs.next(list);
            });
        });
    }
    // Create ComboBox item list
    private createMaterialSpecificationIdList() {
        this.conLogCalled('createMaterialSpecificationIdList');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.tdbMaterialSpecApiService.entityLookupList.subscribe(
                    (list) => obs.next(list),
                    (err) => obs.next([])
                )
            );
        });
    }
    // Create ComboBox item list
    private createTargetInternalDiameterList() {
        this.conLogCalled('createTargetInternalDiameterList');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            // TODO ask UP
            obs.next([
                new ComboBoxItemNumberString(575, '575'),
            ]);
        });
    }
    // Create ComboBox item list
    private createAreaTypeList() {
        this.conLogCalled('createAreaTypeList');
        return new Observable<ComboBoxItemStringString[]>((obs) => {
            this.auxValueApiService.areaTypeList.subscribe(
                (list) => {
                    obs.next(list);
                },
                (error) => obs.next([]));
        });
    }
    // Create ComboBox item list
    private createGroupGradeListCbi() {
        this.conLogCalled('createGroupGradeList');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            const filter = new TdbGradeGroupListFilter();
            const listRequest = this.tdbGradeGroupApiService.createListRequest(filter);
            this.tdbGradeGroupApiService.readList(listRequest).subscribe((listResult) => {
                const list = ComboBoxItemListBuilder.build<any, number, string, ComboBoxItemNumberString>(
                    listResult.data, ComboBoxItemNumberString, 'gradeGroupId', 'gradeGroupLabel');
                obs.next(list);
            });
        });
    }
    // Create ComboBox item list
    private createAnalysisSampleListCbi() {
        this.conLogCalled('createAnalysisSampleListCbi');
        return new Observable<ComboBoxItemNumberString[]>((obs) => {
            this.subscribe(
                this.idSubject.subscribe((id) => {
                    this.conLogCalledId('createAnalysisSampleListCbi', id);
                    const filter = new HrmAnalysisDataListFilter();
                    filter.searchHeatNo = this.heatNo;
                    const listRequest = this.hrmAnalysisDataApiService.createListRequest(filter);
                    this.hrmAnalysisDataApiService.readList(listRequest).subscribe((listResult) => {
                        const list = ComboBoxItemListBuilder.build<any, number, string, ComboBoxItemNumberString>(
                            listResult.data, ComboBoxItemNumberString, 'analysisCnt', 'sampleId');
                        if (list.length) {
                            this.mostRecentChemAnalysisSampleId = list[0].value;
                        }
                        obs.next(list);
                    });
                }),
            );
        });
    }

    // .................................................................

    private fetchChemAnalysisSampleData(sampleId?: number): Observable<ChemRowDataList> {
        return new Observable<ChemRowDataList>((obs) => {
            if (sampleId !== undefined) {
                this.hrmAnalysisDataApiService.read(sampleId).subscribe((data) => {
                    const items = new ChemRowDataList(chemicalLabels);
                    Object.getOwnPropertyNames(chemicalLabels).forEach((propertyName) =>
                        items.setAnalysis(chemicalLabels[propertyName], data[propertyName])
                    );
                    obs.next(items);
                });
            } else {
                obs.next(new ChemRowDataList(chemicalLabels));
            }
        });
    }
    private fetchChemMostRecentAnalysisSampleData() {
        return this.fetchChemAnalysisSampleData(this.mostRecentChemAnalysisSampleId);
    }

    // .................................................................

    private updateChemAnalysisSampleData() {
        this.fetchChemAnalysisSampleData(this.selectedChemAnalysisSampleId).subscribe(
            (items) => this.chem.laboratoryAnalysisItems = items
        );
    }
    private updateChemSteelGradeData() {
        this.log.debug('updateChemSteelGradeData()');

        this.fetchChemMostRecentAnalysisSampleData().subscribe((items) => {
            const alloySpecCnt = this.alloySpecCnt;
            this.log.debug('updateChemSteelGradeData() ' + alloySpecCnt);
            if (alloySpecCnt) {
                this.tdbAlloyApiService.read(alloySpecCnt).subscribe((data) => {
                    const valueNo = data.chemComp[0];
                    const valueMin = data.chemComp[1];
                    const valueMax = data.chemComp[2];
                    Object.getOwnPropertyNames(chemicalLabels).forEach((propertyName) => {
                        const label = chemicalLabels[propertyName];
                        items.setNo(label, valueNo[propertyName]);
                        items.setMin(label, valueMin[propertyName]);
                        items.setMax(label, valueMax[propertyName]);
                    });
                    this.chem.steelGradeItems = items;
                });
            } else {
                this.chem.steelGradeItems = items;
            }
        });
    }
    private updateChemMaterialSpecificationData() {
        this.log.debug('updateChemMaterialSpecificationData()');
        this.fetchChemMostRecentAnalysisSampleData().subscribe((items) => {
            const alloySpecCnt = this.alloySpecCnt;
            this.tdbAlloySpecApiService.read(alloySpecCnt).subscribe((data) => {
                const valueMin = data.chemComp[0];
                const valueMax = data.chemComp[1];
                Object.getOwnPropertyNames(chemicalLabels).forEach((propertyName) => {
                    const label = chemicalLabels[propertyName];
                    items.setMin(label, valueMin[propertyName]);
                    items.setMax(label, valueMax[propertyName]);
                });
                this.chem.materialSpecificationItems = items;
            });
        });
    }

    private sanitizeId() {
        // if id = NaN backend side the controller crashes
        if (isNaN(this.id)) {
            this.id = undefined;
        }
    }

}
