import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { AuthorizationGuard } from '@app/core';

const routes: Routes = [
  {
    path: '',
    component : DashboardComponent,
    canActivate:[AuthorizationGuard],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
