// angular modules
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

// https://github.com/annapogorelova/ng2-go-top-button
import { GoTopButtonModule } from 'ng2-go-top-button';

// app modules
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

// module components
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { ShellComponent } from './shell.component';
import { SidebarComponent } from './sidebar/sidebar.component';

@NgModule({
    imports: [
        // angular modules
        CommonModule,

        GoTopButtonModule,

        // app modules
        CoreModule,
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

        RouterModule.forChild([]),
        WidgetModule,
    ],
    declarations: [

        // module components
        ShellComponent,
        HeaderComponent,
        SidebarComponent,
        FooterComponent

    ],
    exports: [
        ShellComponent
    ]
})
export class ShellModule { }
