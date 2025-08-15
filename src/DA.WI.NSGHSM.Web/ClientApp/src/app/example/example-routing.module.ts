import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../core/auth/auth.guard';
import { Roles } from '../core/application-constants';

import { CityDetailComponent, CityListComponent, } from './city';
import { MeasurementDetailComponent, MeasurementListComponent, } from './measurement';
import { DashboardComponent } from './dashboard';


const routes: Routes = [
    {
        path: 'cities',
        component: CityListComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'cities/:id',
        component: CityDetailComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted] }
    },
    {
        path: 'cities/new',
        component: CityDetailComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All] }
    },
    {
        path: 'measurements',
        component: MeasurementListComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted] }
    },
    {
        path: 'measurements/:id',
        component: MeasurementDetailComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted] }
    },
    {
        path: 'measurements/new',
        component: MeasurementDetailComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All] }
    },
    {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted] }
    },
];

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
    ]
})
export class ExampleRoutingModule {

}
