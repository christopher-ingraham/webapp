import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';
import { WidgetModule } from '../widget/widget.module';

import { CobbleReportComponent } from './report-list-root/component/cobble-report/cobble-report.component';
import { CobbleReportFiltersComponent } from './report-list-root/component/cobble-report-filters/cobble-report-filters.component';

import { CoilGeneralReportComponent } from './report-list-root/component/coil-general-report/coil-general-report.component';
import { CoilGeneralReportFiltersComponent } from './report-list-root/component/coil-general-report-filters/coil-general-report-filters.component';

import { PracticeReportComponent } from './report-list-root/component/practice-report/practice-report.component';
import { PracticeReportFiltersComponent } from './report-list-root/component/practice-report-filters/practice-report-filters.component';

import { ReportListRootComponent } from './report-list-root';
import { ReportsPrintOutRoutingModule } from './reports-print-out-routing.module';

import { ShiftReportComponent } from './report-list-root/component/shift-report/shift-report.component';
import { ShiftReportFiltersComponent } from './report-list-root/component/shift-report-filters/shift-report-filters.component';

import { StoppageReportComponent } from './report-list-root/component/stoppage-report/stoppage-report.component';
import { StoppageReportFiltersComponent } from './report-list-root/component/stoppage-report-filters/stoppage-report-filters.component';

import { TimeOrientedReportComponent } from './report-list-root/component/time-oriented-report/time-oriented-report.component';
import { TimeOrientedReportFiltersComponent } from './report-list-root/component/time-oriented-report-filters/time-oriented-report-filters.component';


@NgModule({
  declarations: [
      CobbleReportComponent,
      CobbleReportFiltersComponent,
      CoilGeneralReportComponent,
      CoilGeneralReportFiltersComponent,
      PracticeReportComponent,
      PracticeReportFiltersComponent,
      ReportListRootComponent,
      ShiftReportComponent,
      ShiftReportFiltersComponent,
      StoppageReportComponent,
      StoppageReportFiltersComponent,
      TimeOrientedReportComponent,
      TimeOrientedReportFiltersComponent,
    ],
  imports: [
    CommonModule,
    FormsModule,
    ReportsPrintOutRoutingModule,
    SharedModule,
    WidgetModule
  ]
})
export class ReportsPrintOutModule { }
