import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Surt05Component } from './surt05/surt05.component';
import { Surt05DetailComponent } from './surt05/surt05-detail.component';
import { Surt05Resolver } from './surt05/surt05-resolver.service';
import { CanDeactivateGuard, AuthorizationGuard } from '@app/core';
import { Surt04Component } from './surt04/surt04.component';
import { Surt04DetailComponent } from './surt04/surt04-detail.component';
import { Surt04Resolver } from './surt04/surt04-resolver.service';
import { Surt01Component } from './surt01/surt01.component';
import { Surt01DetailComponent } from './surt01/surt01-detail.component';
import { Surt01Resolver } from './surt01/surt01-resolver.service';
import { Surt01DivisionComponent } from './surt01/surt01-division.component';
import { Surt01DivisionDetailComponent } from './surt01/surt01-division-detail.component';
import { Surt03Component } from './surt03/surt03.component';
import { Surt03DetailComponent } from './surt03/surt03-detail.component';
import { Surt03Resolver } from './surt03/surt03-resolver.service';
import { Surt02Component } from './surt02/surt02.component';
import { Surt02DetailComponent } from './surt02/surt02-detail.component';
import { Surt02Resolver } from './surt02/surt02-resolver.service';
import { Surt07Component } from './surt07/surt07.component';
import { Surt07DetailComponent } from './surt07/surt07-detail.component';
import { Surt07Resolver } from './surt07/surt07-resolver.service';
import { Surt06Component } from './surt06/surt06.component';
import { Surt06Resolver } from './surt06/surt06-resolver.service';
import { Surt06DetailComponent } from './surt06/surt06-detail.component';
import { Surt06PermissionComponent } from './surt06/surt06-permission.component';
import { Surt06SearchComponent } from './surt06/surt06-search.component';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthorizationGuard],
    canActivateChild: [AuthorizationGuard],
    children: [
      {
        path: 'surt01',
        component: Surt01Component,
        data: { code: 'surt01' }
      },
      {
        path: 'surt01/company/detail',
        component: Surt01DetailComponent,
        resolve: { surt01: Surt01Resolver },
        canDeactivate: [CanDeactivateGuard],
        data: { code: 'surt01' }
      },
      {
        path: 'surt01/division',
        component: Surt01DivisionComponent,
        resolve: { surt01: Surt01Resolver },
        canDeactivate: [CanDeactivateGuard],
        data: { code: 'surt01' }
      },
      {
        path: 'surt01/division/detail',
        component: Surt01DivisionDetailComponent,
        resolve: { surt01: Surt01Resolver },
        canDeactivate: [CanDeactivateGuard],
        data: { code: 'surt01' }
      },
      {
        path: 'surt02',
        component: Surt02Component,
        data: { code: 'surt02' }
      },
      {
        path: 'surt02/detail',
        component: Surt02DetailComponent,
        resolve: { surt02: Surt02Resolver },
        canDeactivate: [CanDeactivateGuard],
        data: { code: 'surt02' }
      },
      {
        path: 'surt03',
        component: Surt03Component,
        data: { code: 'surt03' }
      },
      {
        path: 'surt03/detail',
        component: Surt03DetailComponent,
        resolve: { surt03: Surt03Resolver },
        canDeactivate: [CanDeactivateGuard],
        data: { code: 'surt03' }
      },
      {
        path: 'surt04',
        component: Surt04Component,
        data: { code: 'surt04' }
      },
      {
        path: 'surt04/detail',
        component: Surt04DetailComponent,
        resolve: { surt04: Surt04Resolver },
        canDeactivate: [CanDeactivateGuard],
        data: { code: 'surt04' }
      },
      {
        path: 'surt05',
        component: Surt05Component,
        data: { code: 'surt05' }
      },
      {
        path: 'surt05/detail',
        component: Surt05DetailComponent,
        resolve: { surt05: Surt05Resolver },
        canDeactivate: [CanDeactivateGuard],
        data: { code: 'surt05' }
      },
      { 
        path: 'surt06',
        component: Surt06Component,
        resolve: { surt06: Surt06Resolver },
        data: { code : 'surt06' }
      },
      {
        path: 'surt06/search',
        component: Surt06SearchComponent,
        resolve: { surt06: Surt06Resolver },
        data: { code : 'surt06' },
      },
      {
        path: 'surt06/detail',
        component: Surt06DetailComponent,
        canDeactivate: [CanDeactivateGuard],
        resolve: { surt06: Surt06Resolver },
        data: { code : 'surt06' },
        runGuardsAndResolvers:'always'
      },
      {
        path: 'surt06/permission',
        component: Surt06PermissionComponent,
        canDeactivate: [CanDeactivateGuard],
        resolve: { surt06: Surt06Resolver },
        data: { code : 'surt06' },
        runGuardsAndResolvers:'always'
      },
      {
        path: 'surt07',
        component: Surt07Component,
        data: { code: 'surt07' }
      },
      {
        path: 'surt07/detail',
        component: Surt07DetailComponent,
        resolve: { surt07: Surt07Resolver },
        canDeactivate: [CanDeactivateGuard],
        data: { code: 'surt07' }
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [
    Surt01Resolver,
    Surt02Resolver,
    Surt03Resolver,
    Surt04Resolver,
    Surt05Resolver,
    Surt06Resolver,
    Surt07Resolver
  ]
})
export class SuRoutingModule { }
