import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard, Roles } from '@app/core';

import { MainSignalsRootComponent } from './main-signals-root';
import { TrendsViewRootComponent } from './trends-view-root';
import { MapsViewRootComponent } from './maps-view-root';

const routesMainSignals: Routes = [
    {
        path: '',
        component: MainSignalsRootComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted, Roles.ReadOnly] }
    }
];

const routesTrendsView: Routes = [
    {
        path: '',
        component: TrendsViewRootComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted, Roles.ReadOnly] }
    }
];

const routesMapsView: Routes = [
    {
        path: '',
        component: MapsViewRootComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted, Roles.ReadOnly] }
    }
];

const routes: Routes = [
    {
        path: 'main-signals',
        children: routesTrendsView,
    },
    {
        path: 'trends-view',
        children: routesMainSignals,
    },
    {
        path: 'maps-view',
        children: routesMapsView,
    },
    {
        path: '',
        redirectTo: 'main-signals',
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QualityControlSystemRoutingModule { }
