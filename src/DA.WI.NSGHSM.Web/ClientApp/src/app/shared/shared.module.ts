// Angular
import { NgModule, ModuleWithProviders, Provider, Optional, SkipSelf } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';

import { CoreModule } from '@app/core';

import {
    UomLabelPipe,
    UomValuePipe,
} from './pipe';

import {
    AppStatusStoreService,
    AuxValueService,
    CoilGeneralReportApiService,
    CoolingDowncoilersApiService,
    ExitSaddleApiService,
    FinishingMillApiService,
    HrmAnalysisDataApiService,
    HrmHeatApiService,
    HrmInputPieceApiService,
    HrmJobApiService,
    PracticeReportApiService,
    ProducedCoilApiService,
    RepHmPieceApiService,
    RepHmPieceStatApiService,
    RepHmPieceTrendApiService,
    RoughingMillApiService,
    ShiftReportApiService,
    StoppageReportApiService,
    TdbAlloyApiService,
    TdbAlloySpecApiService,
    TdbGradeGroupApiService,
    TdbMaterialGradeApiService,
    TdbMaterialSpecApiService,
    TdbProcessCodeApiService,
    UsedSetupApiService,
} from './service';

const providers: Provider[] = [
    { provide: AppStatusStoreService, useClass: AppStatusStoreService, },
    { provide: AuxValueService, useClass: AuxValueService, },
    { provide: CoilGeneralReportApiService, useClass: CoilGeneralReportApiService, },
    { provide: ExitSaddleApiService, useClass: ExitSaddleApiService, },
    { provide: HrmAnalysisDataApiService, useClass: HrmAnalysisDataApiService, },
    { provide: HrmHeatApiService, useClass: HrmHeatApiService, },
    { provide: HrmInputPieceApiService, useClass: HrmInputPieceApiService, },
    { provide: HrmJobApiService, useClass: HrmJobApiService, },
    { provide: PracticeReportApiService, useClass: PracticeReportApiService, },
    { provide: ProducedCoilApiService, useClass: ProducedCoilApiService },
    { provide: RepHmPieceApiService, useClass: RepHmPieceApiService, },
    { provide: RepHmPieceStatApiService, useClass: RepHmPieceStatApiService, },
    { provide: RepHmPieceTrendApiService, useClass: RepHmPieceTrendApiService, },
    { provide: ShiftReportApiService, useClass: ShiftReportApiService, },
    { provide: StoppageReportApiService, useClass: StoppageReportApiService, },
    { provide: TdbAlloyApiService, useClass: TdbAlloyApiService, },
    { provide: TdbAlloySpecApiService, useClass: TdbAlloySpecApiService, },
    { provide: TdbGradeGroupApiService, useClass: TdbGradeGroupApiService, },
    { provide: TdbMaterialGradeApiService, useClass: TdbMaterialGradeApiService, },
    { provide: TdbMaterialSpecApiService, useClass: TdbMaterialSpecApiService, },
    { provide: TdbProcessCodeApiService, useClass: TdbProcessCodeApiService, },
    { provide: UsedSetupApiService, useClass: UsedSetupApiService, },
    { provide: CoolingDowncoilersApiService, useClass: CoolingDowncoilersApiService, },
    { provide: FinishingMillApiService, useClass: FinishingMillApiService, },
    { provide: RoughingMillApiService, useClass: RoughingMillApiService, },
    DecimalPipe,
    UomLabelPipe,
    UomValuePipe,
];

const pipes = [
    UomLabelPipe,
    UomValuePipe,
];

const directives = [

];

@NgModule({
    imports: [
        // Angular
        CommonModule,
        // Application
        CoreModule,
    ],
    declarations: [
        // Application
        ...pipes,
        ...directives,
    ],
    exports: [
        // Application
        ...pipes,
        ...directives,
    ],
})
export class SharedModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers,
        };
    }
    constructor(@Optional() @SkipSelf() parentModule?: SharedModule) {
        if (parentModule) {
            // throw new Error(`${SharedModule['name']} is already loaded. Import it in the AppModule only.`);
            // tslint:disable-next-line
            console.debug(`${SharedModule['name']} is already loaded. Import it in the AppModule only.`);
        }
    }
}
