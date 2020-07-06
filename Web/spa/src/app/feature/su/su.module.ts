import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTreeModule, MatButtonModule, MatIconModule, MatCheckboxModule } from '@angular/material';

import { SuRoutingModule } from './su-routing.module';
import { SharedModule } from '@app/shared';
import { ReactiveFormsModule } from '@angular/forms';
import { Surt05Component } from './surt05/surt05.component';
import { Surt05DetailComponent } from './surt05/surt05-detail.component';
import { Surt05Service } from './surt05/surt05.service';
import { Surt01DetailComponent } from './surt01/surt01-detail.component';
import { Surt01Component } from './surt01/surt01.component';
import { Surt01Service } from './surt01/surt01.service';
import { Surt01DivisionComponent } from './surt01/surt01-division.component';
import { Surt01DivisionDetailComponent } from './surt01/surt01-division-detail.component';
import { Surt03DetailComponent } from './surt03/surt03-detail.component';
import { Surt03Component } from './surt03/surt03.component';
import { Surt03Service } from './surt03/surt03.service';
import { Surt04Service } from './surt04/surt04.service';
import { Surt04Component } from './surt04/surt04.component';
import { Surt04DetailComponent } from './surt04/surt04-detail.component';
import { Surt04LookupComponent } from './surt04/surt04-lookup.component';
import { Surt02DetailComponent } from './surt02/surt02-detail.component';
import { Surt02Component } from './surt02/surt02.component';
import { Surt02Service } from './surt02/surt02.service';
import { LazyTranslationService } from '@app/core';
import { Surt07Component } from './surt07/surt07.component';
import { Surt07DetailComponent } from './surt07/surt07-detail.component';
import { Surt07Service } from './surt07/surt07.service';
import { Surt06DetailComponent } from './surt06/surt06-detail.component';
import { Surt06Component } from './surt06/surt06.component';
import { Surt06Service } from './surt06/surt06.service';
import { Surt06PermissionComponent } from './surt06/surt06-permission.component';
import { DivisionTreeComponent } from './surt06/division-tree/division-tree.component';
import { Surt06SearchComponent } from './surt06/surt06-search.component';

@NgModule({
  declarations: [
    Surt01Component,
    Surt01DetailComponent,
    Surt01DivisionComponent,
    Surt01DivisionDetailComponent,
    Surt02Component,
    Surt02DetailComponent,
    Surt03Component,
    Surt03DetailComponent,
    Surt04Component,
    Surt04DetailComponent,
    Surt04LookupComponent,
    Surt05Component,
    Surt05DetailComponent,
    Surt06Component,
    Surt06DetailComponent,
    DivisionTreeComponent,
    Surt06PermissionComponent,
    Surt07Component,
    Surt07DetailComponent,
    Surt06SearchComponent
  ],
  imports: [
    CommonModule,
    SuRoutingModule,
    ReactiveFormsModule,
    SharedModule,
    MatTreeModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
  ],
  providers: [
    Surt01Service,
    Surt02Service,
    Surt03Service,
    Surt04Service,
    Surt05Service,
    Surt06Service,
    Surt07Service
  ],
  entryComponents: [
    Surt04LookupComponent
  ]
})
export class SuModule {
  constructor(private lazy: LazyTranslationService) {
    lazy.add('su');
  }
}