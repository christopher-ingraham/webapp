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

import {
    CityDetailComponent,
    CityListComponent,
    CityListFilterComponent,
} from './city';

import {
    MeasurementDetailComponent,
    MeasurementListComponent,
    MeasurementListFilterComponent,
} from './measurement';

import { DashboardComponent } from './dashboard';
import { DashboardCityComponent } from './dashboard-city';
import { ExampleRoutingModule } from './example-routing.module';

@NgModule({
    declarations: [
        CityDetailComponent,
        CityListComponent,
        CityListFilterComponent,
        MeasurementDetailComponent,
        MeasurementListComponent,
        MeasurementListFilterComponent,
        DashboardComponent,
        DashboardCityComponent,
    ],
    imports: [
        CommonModule,
        CoreModule,
        ExampleRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        SharedModule,
        // Never call a forRoot static method in the shared module.
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
    ]
})
export class ExampleModule { }
