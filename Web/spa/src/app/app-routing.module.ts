import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { Shell } from '@app/shell/shell.service';
import { EmptyComponent } from './feature/empty/empty.component';

const routes: Routes = [
  { path: 'account', loadChildren: './feature/account/account.module#AccountModule' },
  Shell.childRoutes([
    { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
    { path: 'demo', loadChildren: './feature/demo/demo.module#DemoModule' },
    { path: 'empty/lang/:code', component: EmptyComponent },
    { path: 'empty/comp/:code', component: EmptyComponent },
    { path: 'empty/division/:code', component: EmptyComponent },
    { path: 'dashboard', loadChildren: './feature/dashboard/dashboard.module#DashboardModule'},
    { path: 'db', loadChildren: './feature/db/db.module#DbModule' },
    //{ path: 'cs', loadChildren: './feature/cs/cs.module#CsModule' },
    { path: 'su', loadChildren: './feature/su/su.module#SuModule' },
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
