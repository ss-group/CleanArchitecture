import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CanDeactivateGuard, AuthorizationGuard } from '@app/core';

import { Dbrt01Component } from './dbrt01/dbrt01.component';
import { Dbrt01DetailComponent } from './dbrt01/dbrt01-detail.component';
import { Dbrt02Component } from './dbrt02/dbrt02.component';
import { Dbrt02DetailComponent } from './dbrt02/dbrt02-detail.component';
import { Dbrt03Component } from './dbrt03/dbrt03.component';
import { Dbrt03DetailComponent } from './dbrt03/dbrt03-detail.component';
import { Dbrt04Component } from './dbrt04/dbrt04.component';
import { Dbrt04DetailComponent } from './dbrt04/dbrt04-detail.component';
import { Dbrt05Component } from './dbrt05/dbrt05.component';
import { Dbrt05DetailComponent } from './dbrt05/dbrt05-detail.component';
import { Dbrt06Component } from './dbrt06/dbrt06.component';
import { Dbrt06DetailComponent } from './dbrt06/dbrt06-detail.component';
import { Dbrt07Component } from './dbrt07/dbrt07.component';
import { Dbrt07DetailComponent } from './dbrt07/dbrt07-detail.component';
import { Dbrt08Component } from './dbrt08/dbrt08.component';
import { Dbrt08DetailComponent } from './dbrt08/dbrt08-detail.component';
import { Dbrt09Component } from './dbrt09/dbrt09.component';
import { Dbrt09DetailComponent } from './dbrt09/dbrt09-detail.component';
import { Dbrt10Component } from './dbrt10/dbrt10.component';
import { Dbrt10DetailComponent } from './dbrt10/dbrt10-detail.component';
import { Dbrt11Component } from './dbrt11/dbrt11.component';
import { Dbrt11DetailComponent } from './dbrt11/dbrt11-detail.component';
import { Dbrt12Component } from './dbrt12/dbrt12.component';
import { Dbrt12DetailComponent } from './dbrt12/dbrt12-detail.component';
import { Dbrt13Component } from './dbrt13/dbrt13.component';
import { Dbrt13DetailComponent } from './dbrt13/dbrt13-detail.component';
import { Dbrt14Component } from './dbrt14/dbrt14.component';
import { Dbrt14DetailComponent } from './dbrt14/dbrt14-detail.component';
import { Dbrt15Component } from './dbrt15/dbrt15.component';
import { Dbrt15DetailComponent } from './dbrt15/dbrt15-detail.component';
import { Dbrt16Component } from './dbrt16/dbrt16.component';
import { Dbrt16DetailComponent } from './dbrt16/dbrt16-detail.component';
import { Dbrt17Component } from './dbrt17/dbrt17.component';
import { Dbrt17DetailComponent } from './dbrt17/dbrt17-detail.component';
import { Dbrt18Component } from './dbrt18/dbrt18.component';
import { Dbrt18DetailComponent } from './dbrt18/dbrt18-detail.component';
import { Dbrt18DetailManagamentComponent } from './dbrt18/dbrt18-detail-managament.component';
import { Dbrt19Component } from './dbrt19/dbrt19.component';
import { Dbrt19DetailComponent } from './dbrt19/dbrt19-detail.component';
import { Dbrt20Component } from './dbrt20/dbrt20.component';
import { Dbrt20DetailComponent } from './dbrt20/dbrt20-detail.component';
import { Dbrt21Component } from './dbrt21/dbrt21.component';
import { Dbrt21DetailComponent } from './dbrt21/dbrt21-detail.component';

import { Dbrt01ResolverService } from './dbrt01/dbrt01-resolver.service';
import { Dbrt02ResolverService } from './dbrt02/dbrt02-resolver.service';
import { Dbrt03ResolverService } from './dbrt03/dbrt03-resolver.service';
import { Dbrt04ResolverService } from './dbrt04/dbrt04-resolver.service';
import { Dbrt05ResolverService } from './dbrt05/dbrt05-resolver.service';
import { Dbrt06ResolverService } from './dbrt06/dbrt06-resolver.service';
import { Dbrt07ResolverService } from './dbrt07/dbrt07-resolver.service';
import { Dbrt08ResolverService } from './dbrt08/dbrt08-resolver.service';
import { Dbrt09ResolverService } from './dbrt09/dbrt09-resolver.service';
import { Dbrt10ResolverService } from './dbrt10/dbrt10-resolver.service';
import { Dbrt11ResolverService } from './dbrt11/dbrt11-resolver.service';
import { Dbrt12ResolverService } from './dbrt12/dbrt12-resolver.service';
import { Dbrt13ResolverService } from './dbrt13/dbrt13-resolver.service';
import { Dbrt14ResolverService } from './dbrt14/dbrt14-resolver.service';
import { Dbrt15ResolverService } from './dbrt15/dbrt15-resolver.service';
import { Dbrt16ResolverService } from './dbrt16/dbrt16-resolver.service';
import { Dbrt17ResolverService } from './dbrt17/dbrt17-resolver.service';
import { Dbrt18ResolverService } from './dbrt18/dbrt18-resolver.service';
import { Dbrt19ResolverService } from './dbrt19/dbrt19-resolver.service';
import { Dbrt20ResolverService } from './dbrt20/dbrt20-resolver.service';
import { Dbrt21ResolverService } from './dbrt21/dbrt21-resolver.service';

const routes: Routes = [
    { path: 'dbrt01', component: Dbrt01Component },
    {
        path: 'dbrt01/detail', component: Dbrt01DetailComponent,
        resolve: { dbrt01: Dbrt01ResolverService },
        data: { code: 'DBRT01' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt02', component: Dbrt02Component },
    {
        path: 'dbrt02/detail', component: Dbrt02DetailComponent,
        resolve: { dbrt02: Dbrt02ResolverService },
        data: { code: 'DBRT02' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt03', component: Dbrt03Component },
    {
        path: 'dbrt03/detail', component: Dbrt03DetailComponent,
        resolve: { dbrt03: Dbrt03ResolverService },
        data: { code: 'DBRT03' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt04', component: Dbrt04Component },
    {
        path: 'dbrt04/detail', component: Dbrt04DetailComponent,
        resolve: { dbrt04: Dbrt04ResolverService },
        data: { code: 'DBRT04' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt05', component: Dbrt05Component },
    {
        path: 'dbrt05/detail', component: Dbrt05DetailComponent,
        resolve: { dbrt05: Dbrt05ResolverService },
        data: { code: 'DBRT05' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt06', component: Dbrt06Component },
    {
        path: 'dbrt06/detail', component: Dbrt06DetailComponent,
        resolve: { dbrt06: Dbrt06ResolverService },
        data: { code: 'DBRT06' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt07', component: Dbrt07Component },
    {
        path: 'dbrt07/detail', component: Dbrt07DetailComponent,
        resolve: { dbrt07: Dbrt07ResolverService },
        data: { code: 'DBRT07' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    {
        path: 'dbrt08', component: Dbrt08Component,
        resolve: { dbrt08: Dbrt08ResolverService },
    },
    {
        path: 'dbrt08/detail', component: Dbrt08DetailComponent,
        resolve: { dbrt08: Dbrt08ResolverService },
        data: { code: 'DBRT08' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt09', component: Dbrt09Component },
    {
        path: 'dbrt09/detail', component: Dbrt09DetailComponent,
        resolve: { dbrt09: Dbrt09ResolverService },
        data: { code: 'DBRT09' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt10', component: Dbrt10Component },
    {
        path: 'dbrt10/detail', component: Dbrt10DetailComponent,
        resolve: { dbrt10: Dbrt10ResolverService },
        data: { code: 'DBRT10' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt11', component: Dbrt11Component },
    {
        path: 'dbrt11/detail', component: Dbrt11DetailComponent,
        resolve: { dbrt11: Dbrt11ResolverService },
        data: { code: 'DBRT11' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt12', component: Dbrt12Component },
    {
        path: 'dbrt12/detail', component: Dbrt12DetailComponent,
        resolve: { dbrt12: Dbrt12ResolverService },
        data: { code: 'DBRT12' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt13', component: Dbrt13Component },
    {
        path: 'dbrt13/detail', component: Dbrt13DetailComponent,
        resolve: { dbrt13: Dbrt13ResolverService },
        data: { code: 'DBRT13' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt14', component: Dbrt14Component },
    {
        path: 'dbrt14/detail', component: Dbrt14DetailComponent,
        resolve: { dbrt14: Dbrt14ResolverService },
        data: { code: 'DBRT14' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt15', component: Dbrt15Component },
    {
        path: 'dbrt15/detail', component: Dbrt15DetailComponent,
        resolve: { dbrt15: Dbrt15ResolverService },
        data: { code: 'DBRT15' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt16', component: Dbrt16Component },
    {
        path: 'dbrt16/detail', component: Dbrt16DetailComponent,
        resolve: { dbrt16: Dbrt16ResolverService },
        data: { code: 'DBRT16' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt17', component: Dbrt17Component },
    {
        path: 'dbrt17/detail', component: Dbrt17DetailComponent,
        resolve: { dbrt17: Dbrt17ResolverService },
        data: { code: 'DBRT17' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    {
        path: '',
        canActivate: [AuthorizationGuard],
        canActivateChild: [AuthorizationGuard],
        children: [
            { path: 'dbrt18', component: Dbrt18Component },
            {
                path: 'dbrt18/detail', component: Dbrt18DetailComponent,
                resolve: { dbrt18: Dbrt18ResolverService }
            },
            {
                path: 'dbrt18/detail/managament', component: Dbrt18DetailManagamentComponent,
                resolve: { dbrt18: Dbrt18ResolverService },
                data: { code: 'DBRT18' },
                canDeactivate: [CanDeactivateGuard],
                runGuardsAndResolvers: 'always'
            }
        ]
    },
    { path: 'dbrt19', component: Dbrt19Component },
    {
        path: 'dbrt19/detail', component: Dbrt19DetailComponent,
        resolve: { dbrt19: Dbrt19ResolverService },
        data: { code: 'DBRT19' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },
    { path: 'dbrt20', component: Dbrt20Component },
    {
        path: 'dbrt20/detail', component: Dbrt20DetailComponent,
        resolve: { dbrt20: Dbrt20ResolverService },
        data: { code: 'DBRT20' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    },{ path: 'dbrt21', component: Dbrt21Component },
    {
        path: 'dbrt21/detail', component: Dbrt21DetailComponent,
        resolve: { dbrt21: Dbrt21ResolverService },
        data: { code: 'DBRT21' },
        canDeactivate: [CanDeactivateGuard],
        runGuardsAndResolvers: 'always'
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    providers: [
        Dbrt01ResolverService,
        Dbrt02ResolverService,
        Dbrt03ResolverService,
        Dbrt04ResolverService,
        Dbrt05ResolverService,
        Dbrt06ResolverService,
        Dbrt07ResolverService,
        Dbrt08ResolverService,
        Dbrt09ResolverService,
        Dbrt10ResolverService,
        Dbrt11ResolverService,
        Dbrt12ResolverService,
        Dbrt13ResolverService,
        Dbrt14ResolverService,
        Dbrt15ResolverService,
        Dbrt16ResolverService,
        Dbrt17ResolverService,
        Dbrt18ResolverService,
        Dbrt19ResolverService,
        Dbrt20ResolverService,
        Dbrt21ResolverService
    ]
})
export class DbRoutingModule { }
