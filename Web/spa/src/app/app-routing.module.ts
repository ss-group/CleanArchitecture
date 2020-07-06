import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { Shell } from '@app/shell/shell.service';
import { EmptyComponent } from './feature/empty/empty.component';

const routes: Routes = [
  { path: 'account', loadChildren: './feature/account/account.module#AccountModule' },
  Shell.childRoutes([
    { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
    { path: 'empty/lang/:code', component: EmptyComponent },
    { path: 'empty/comp/:code', component: EmptyComponent },
    { path: 'empty/division/:code', component: EmptyComponent },
    { path: 'demo', loadChildren: './feature/demo/demo.module#DemoModule' },
    { path: 'dashboard', loadChildren: './feature/dashboard/dashboard.module#DashboardModule'},
    { path: 'cq', loadChildren: './feature/cq/cq.module#CqModule' },
    { path: 'cs', loadChildren: './feature/cs/cs.module#CsModule' },
    { path: 'sh', loadChildren: './feature/sh/sh.module#ShModule' },
	  { path: 'fn', loadChildren: './feature/fn/fn.module#FnModule' },
    { path: 'gs', loadChildren: './feature/gs/gs.module#GsModule' },
    { path: 'db', loadChildren: './feature/db/db.module#DbModule' },
    { path: 'rn', loadChildren: './feature/rn/rn.module#RnModule' },
    { path: 'gd', loadChildren: './feature/gd/gd.module#GdModule' },    
    { path: 'en', loadChildren: './feature/en/en.module#EnModule' },
    { path: 'dc', loadChildren: './feature/dc/dc.module#DcModule' },
    { path: 'rg', loadChildren: './feature/rg/rg.module#RgModule' },
    { path: 'su', loadChildren: './feature/su/su.module#SuModule' },
    { path: 'et', loadChildren: './feature/et/et.module#EtModule' },
    { path: 'is', loadChildren: './feature/is/is.module#IsModule' },
    { path: 'di', loadChildren: './feature/di/di.module#DiModule' }
  ]),
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{
    onSameUrlNavigation : 'reload'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
