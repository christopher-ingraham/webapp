import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard, Roles } from '@app/core';

import { StringsListManagementRootComponent } from './strings-list-management-root';
import { InputSlabsManagementRootComponent } from './input-slabs-management-root';
import { ProducedCoilsManagementRootComponent } from './produced-coils-management-root';

// Module routing table

const routes: Routes = [
    {
        path: 'strings-list-management',
        component: StringsListManagementRootComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted, Roles.ReadOnly] }
    },
    {
        path: 'input-slabs-management',
        component: InputSlabsManagementRootComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted, Roles.ReadOnly] }
    },
    {
        path: 'produced-coils-management',
        component: ProducedCoilsManagementRootComponent,
        canActivate: [AuthGuard],
        data: { roles: [Roles.All, Roles.Restricted, Roles.ReadOnly] }
    },
    {
        path: '',
        redirectTo: 'strings-list-management',
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
    ]
})
export class ProductionRoutingModule {

}
