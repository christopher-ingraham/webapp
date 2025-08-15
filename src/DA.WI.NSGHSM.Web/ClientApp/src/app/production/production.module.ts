import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { ApiService, CoreModule } from '@app/core';
import { SharedModule } from '@app/shared';
import {
    ApiTranslateLoader,
    TranslateHelperService,
    TranslateLoader,
    TranslateModule,
    TranslateParser,
    TranslateParserWithDefault,
    WidgetModule,
} from '@app/widget';

import { ProductionRoutingModule } from './production-routing.module';

import {
    HrmJobDetailComponent,
    HrmJobListComponent,
    StringsListManagementRootComponent,
} from './strings-list-management-root';

import {
    ChemLaboratoryAnalysisComponent,
    ChemMaterialSpecificationComponent,
    ChemSteelGradeComponent,
    HrmInputPieceDetailComponent,
    HrmInputPieceListComponent,
    InputSlabsManagementRootComponent,
} from './input-slabs-management-root';

import {
    ExitSaddleListComponent,
    ProducedCoilDetailComponent,
    ProducedCoilListComponent,
    ProducedCoilsManagementRootComponent,
    UsedSetupDetailComponent,
    UsedSetupListComponent,
} from './produced-coils-management-root';

@NgModule({
    declarations: [
        ChemLaboratoryAnalysisComponent,
        ChemMaterialSpecificationComponent,
        ChemSteelGradeComponent,
        ExitSaddleListComponent,
        HrmInputPieceDetailComponent,
        HrmInputPieceListComponent,
        HrmJobDetailComponent,
        HrmJobListComponent,
        InputSlabsManagementRootComponent,
        ProducedCoilDetailComponent,
        ProducedCoilListComponent,
        ProducedCoilsManagementRootComponent,
        StringsListManagementRootComponent,
        UsedSetupDetailComponent,
        UsedSetupListComponent,
    ],
    imports: [
        CommonModule,
        CoreModule,
        FormsModule,
        ProductionRoutingModule,
        ReactiveFormsModule,
        RouterModule,
        SharedModule,
        // Never call a forRoot static method in a shared module.
        // You might end up with different instances of the service in
        // your injector tree. But you can use forChild if necessary.
        TranslateModule.forChild({
            loader: {
                provide: TranslateLoader,
                useClass: ApiTranslateLoader,
                deps: [ApiService]
            },
            parser: {
                provide: TranslateParser,
                useClass: TranslateParserWithDefault,
                deps: [TranslateHelperService]
            }
        }),
        WidgetModule
    ],
    providers: [
    ]
})
export class ProductionModule { }
