import { NgModule, ModuleWithProviders, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';

// Kendo UI - https://www.telerik.com/kendo-angular-ui
import { AutoCompleteModule } from '@progress/kendo-angular-dropdowns';
import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { ChartsModule } from '@progress/kendo-angular-charts';
import '@progress/kendo-angular-common';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';
import { DatePipe } from '@progress/kendo-angular-intl';
import { DialogModule } from '@progress/kendo-angular-dialog';
import { DialogService } from '@progress/kendo-angular-dialog';
import { DropDownListModule } from '@progress/kendo-angular-dropdowns';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns';
import { GridModule } from '@progress/kendo-angular-grid';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { IntlModule } from '@progress/kendo-angular-intl';
import { LayoutModule } from '@progress/kendo-angular-layout';
import { MenuModule } from '@progress/kendo-angular-menu';
import { MessageService } from '@progress/kendo-angular-l10n';
import { MultiSelectModule } from '@progress/kendo-angular-dropdowns';
import { NotificationModule } from '@progress/kendo-angular-notification';
import { NumberPipe } from '@progress/kendo-angular-intl';
import { PanelBarModule } from '@progress/kendo-angular-layout';
import { PopupModule } from '@progress/kendo-angular-popup';
import { ProgressBarModule } from '@progress/kendo-angular-progressbar';
import { TabStripModule } from '@progress/kendo-angular-layout';
import { ToolBarModule } from '@progress/kendo-angular-toolbar';
import { TooltipModule } from '@progress/kendo-angular-tooltip';

import '@progress/kendo-angular-intl/locales/en/all';
import '@progress/kendo-angular-intl/locales/it/all';

// https://plot.ly/
import * as PlotlyJS from 'plotly.js/dist/plotly.js';
import { PlotlyModule } from 'angular-plotly.js';
PlotlyModule.plotlyjs = PlotlyJS;

// NGX-Translate - http://www.ngx-translate.com/

import { ConfirmService } from './confirm';

import {
    TranslateModule,
    TranslateService,
} from '@ngx-translate/core';

import {
    ActionToolbarComponent,
    AuditInfoComponent,
    DetailActionBarComponent,
    TimeFilterComponent,
    ToolbarItemComponent,
} from './component';


import {
    LocaleKendoDatePipe,
    LocaleKendoNumberPipe,
} from './pipes';

import { KendoTranslateService, } from './locale';


const components = [
    ActionToolbarComponent,
    AuditInfoComponent,
    DetailActionBarComponent,
    TimeFilterComponent,
    ToolbarItemComponent,
];

const pipes = [
    LocaleKendoDatePipe,
    LocaleKendoNumberPipe,
];

const modules = [
    AutoCompleteModule,
    ButtonsModule,
    ChartsModule,
    CommonModule,
    DateInputsModule,
    DialogModule,
    DropDownListModule,
    DropDownsModule,
    FlexLayoutModule,
    GridModule,
    InputsModule,
    IntlModule,
    LayoutModule,
    MenuModule,
    MultiSelectModule,
    NotificationModule,
    PanelBarModule,
    PlotlyModule,
    PopupModule,
    ProgressBarModule,
    RouterModule,
    TabStripModule,
    ToolBarModule,
    TooltipModule,
];

const providers = [
    {
        provide: DatePipe,
        useClass: DatePipe
    },
    {
        provide: NumberPipe,
        useClass: NumberPipe
    },
    {
        provide: MessageService,
        useClass: KendoTranslateService,
        deps: [TranslateService]
    },
    ConfirmService,
    DialogService,
    TranslateService,
];


@NgModule({
    declarations: [
        ...components,
        ...pipes,
        AuditInfoComponent,
    ],
    imports: [
        ...modules,
    ],
    exports: [
        ...components,
        ...modules,
        ...pipes,
        TranslateModule,
    ],
})
export class WidgetModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: WidgetModule,
            providers
        };
    }

    constructor(@Optional() @SkipSelf() parentModule?: WidgetModule) {
        if (parentModule) {
            // throw new Error(`${WidgetModule['name']} is already loaded. Import it in the AppModule only.`);
            // tslint:disable-next-line
            console.debug(`${WidgetModule['name']} is already loaded. Import it in the AppModule only.`);
        }
    }
}
