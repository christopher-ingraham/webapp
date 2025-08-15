import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { LayoutModule } from '@progress/kendo-angular-layout';

import { UserComponent } from './user/user.component';
import { SettingsComponent } from './settings.component';
import { AboutComponent } from './about/about.component';
import { LocaleComponent } from './locale/locale.component';

import { CoreModule, ApiService } from '@app/core';
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

@NgModule({
    declarations: [
        SettingsComponent,
        UserComponent,
        AboutComponent,
        LocaleComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild([
            { path: '', component: SettingsComponent }
        ]),
        LayoutModule,
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

        // app modules
        CoreModule,
        SharedModule,
        WidgetModule,
    ]
})
export class SettingsModule { }
