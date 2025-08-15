import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { CoolingDowncoilersComponent } from './cooling-downcoilers/cooling-downcoilers.component';
import { FinishingMillComponent } from './finishing-mill/finishing-mill.component';
import { RoughingMillComponent } from './roughing-mill/roughing-mill.component';

const routes: Routes = [
    {
        path: 'finishing-mill',
        component: FinishingMillComponent,
    },
    {
        path: 'roughing-mill',
        component: RoughingMillComponent
    },
    {
        path: 'cooling-downcoilers',
        component: CoolingDowncoilersComponent
    }
];

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
    ]
})
export class PlantOverviewRoutingModule {

}
