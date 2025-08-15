import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReportListRootComponent } from './report-list-root';

const routes: Routes = [
    {
        path: 'report-list',
        component: ReportListRootComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ReportsPrintOutRoutingModule { }
