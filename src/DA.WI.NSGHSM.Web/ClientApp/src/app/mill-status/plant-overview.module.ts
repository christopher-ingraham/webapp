import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '@app/shared';

import { PlantOverviewRoutingModule } from './plant-overview-routing.module';

import {
    DescalerComponent,
    DowncoilerComponent,
    EdgerComponent,
    IntensiveComponent,
    RollsBigComponent,
    RollsSmallComponent,
    StandComponent,
    TrimmingComponent,
    NormalComponent,
} from './components';

import { CoolingDowncoilersComponent } from './cooling-downcoilers/cooling-downcoilers.component';
import { FinishingMillComponent } from './finishing-mill/finishing-mill.component';
import { RoughingMillComponent } from './roughing-mill/roughing-mill.component';

@NgModule({
    declarations: [
        CoolingDowncoilersComponent,
        DescalerComponent,
        DowncoilerComponent,
        EdgerComponent,
        FinishingMillComponent,
        IntensiveComponent,
        RollsBigComponent,
        RollsSmallComponent,
        RoughingMillComponent,
        StandComponent,
        TrimmingComponent,
        NormalComponent,
    ],
    imports: [
        CommonModule,
        PlantOverviewRoutingModule,
        SharedModule,
    ],
    providers: [
    ]
})
export class PlantOverviewModule { }
