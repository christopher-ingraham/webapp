import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes, ExtraOptions, Router, NavigationCancel, NavigationStart, NavigationEnd } from '@angular/router';

import { NotifierService, UnauthorizedComponent } from '@app/core';
import { PageNotFoundComponent } from './page-not-found.component';
import { PlaygroundComponent } from './playground/playground.component';

import { environment } from 'src/environments/environment';

const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./home/home.module').then((m) => m.HomeModule),
    },
    {
        path: 'example',
        loadChildren: () => import('./example/example.module').then((m) => m.ExampleModule),
    },
    {
        path: 'mill-status',
        loadChildren: () => import('./mill-status/plant-overview.module').then((m) => m.PlantOverviewModule),
    },
    {
        path: 'production',
        loadChildren: () => import('./production/production.module').then((m) => m.ProductionModule),
    },
    {
        path: 'qcs',
        loadChildren: () => import('./quality-control-system/quality-control-system.module').then(m => m.QualityControlSystemModule)
    },
    {
        path: 'reports-print-out',
        loadChildren: () => import('./reports-print-out/reports-print-out.module').then((m) => m.ReportsPrintOutModule),
    },
    {
        path: 'settings',
        loadChildren: () => import('./settings/settings.module').then((m) => m.SettingsModule),
    },
    {
        path: 'test',
        loadChildren: () => import('./test/test.module').then((m) => m.TestModule),
    }/*,
    {
        path: 'playground',
        component: PlaygroundComponent,
    }*/,
    {
        path: 'unauthorized',
        component: UnauthorizedComponent
    },
    {
        path: 'page-not-found',
        component: PageNotFoundComponent
    },
    {
        path: '**',
        component: PageNotFoundComponent
    },
];

const config: ExtraOptions = {
    // enableTracing: !environment.production,
    // useHash: !environment.production,
};

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forRoot(routes, config),
    ]
})
export class AppRoutingModule {
    constructor(
        private notifier: NotifierService,
        router: Router,
    ) {
        router.events.subscribe((event) => {
            if (event instanceof NavigationStart) {
                this.notifier.busy();
            } else if (event instanceof NavigationEnd || event instanceof NavigationCancel) {
                this.notifier.unbusy();
            } else {
                // ?
            }
        });
    }
}
