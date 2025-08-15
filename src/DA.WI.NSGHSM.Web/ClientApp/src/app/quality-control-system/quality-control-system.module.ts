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

import { QualityControlSystemRoutingModule } from './quality-control-system-routing.module';

import {
    ChartStackComponent,
    RepHmMapsViewDetailComponent,
    RepHmMapsViewListComponent,
    RepHmPieceDetailComponent,
    RepHmPieceListComponent,
    RepHmPieceTrendDetailComponent,
    RepHmPieceTrendListComponent,
} from './component';

import {
    MainSignalsRootComponent,
    MainSignalsRootFiltersComponent,
} from './main-signals-root';

import {
    TrendsViewRootComponent,
    TrendsViewRootFiltersComponent,
} from './trends-view-root';

import {
    MapsViewRootComponent,
    MapsViewRootFiltersComponent,
} from './maps-view-root';


@NgModule({
    declarations: [
        ChartStackComponent,
        MainSignalsRootComponent,
        MainSignalsRootFiltersComponent,
        MapsViewRootComponent,
        MapsViewRootFiltersComponent,
        RepHmMapsViewDetailComponent,
        RepHmMapsViewListComponent,
        RepHmPieceDetailComponent,
        RepHmPieceListComponent,
        RepHmPieceTrendDetailComponent,
        RepHmPieceTrendListComponent,
        TrendsViewRootComponent,
        TrendsViewRootFiltersComponent,
    ],
    imports: [
        CommonModule,
        CoreModule,
        FormsModule,
        QualityControlSystemRoutingModule,
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
        WidgetModule,
    ],
    providers: [
    ]
})
export class QualityControlSystemModule { }
