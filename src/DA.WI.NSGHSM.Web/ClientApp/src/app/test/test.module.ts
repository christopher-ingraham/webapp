import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { CoreModule, AuthGuard, ApiService } from '@app/core';
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

import { TestComponent } from './test.component';

import {
    LocaleKendoDatePipe,
    LocaleKendoNumberPipe,
} from '@app/widget';


@NgModule({
    declarations: [
        TestComponent
    ],
    imports: [
        CommonModule,
        FormsModule,

        CoreModule,
        SharedModule,

        RouterModule.forChild([
            {
                path: '',
                component: TestComponent,
                canActivate: [AuthGuard]
            }]),
        // Never call a forRoot static method in the SharedModule.
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
        LocaleKendoDatePipe,
        LocaleKendoNumberPipe,
    ]
})
export class TestModule { }
