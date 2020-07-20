import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CanDeactivateGuard, AuthorizationGuard } from '@app/core';

import { Dbrt01Component } from './dbrt01/dbrt01.component';
import { Dbrt01DetailComponent } from './dbrt01/dbrt01-detail.component';
import { Dbrt01ResolverService } from './dbrt01/dbrt01-resolver.service';
import { Dbrt08Component } from './dbrt08/dbrt08.component';
import { Dbrt08ResolverService } from './dbrt08/dbrt08-resolver.service';

const routes: Routes = [
    { path: 'dbrt01', component: Dbrt01Component },
    {
        path: 'dbrt01/detail', component: Dbrt01DetailComponent,
        resolve: { dbrt01: Dbrt01ResolverService },
        data: { code: 'DBRT01' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    {
        path: 'dbrt08', component: Dbrt08Component,
        resolve: { dbrt08: Dbrt08ResolverService },
    },
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    providers: [
        Dbrt01ResolverService,Dbrt08ResolverService
    ]
})
export class DbRoutingModule { }
