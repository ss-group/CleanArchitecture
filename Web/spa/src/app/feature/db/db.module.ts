import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DbRoutingModule } from './db-routing.module';
import { SharedModule } from '@app/shared';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { LazyTranslationService } from '@app/core';

import { Dbrt01Component } from './dbrt01/dbrt01.component';
import { Dbrt01DetailComponent } from './dbrt01/dbrt01-detail.component';
import { Dbrt01Service } from './dbrt01/dbrt01.service';
import { Dbrt08Component } from './dbrt08/dbrt08.component';
import { Dbrt08Service } from './dbrt08/dbrt08.service';
import { Dbrt08DetailComponent } from './dbrt08/dbrt08-detail.component';

@NgModule({
  declarations: [
    Dbrt01Component,
    Dbrt01DetailComponent,
    Dbrt08Component,
    Dbrt08DetailComponent
  ],
  imports: [
    CommonModule,
    DbRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [
    Dbrt01Service,Dbrt08Service
  ]
})
export class DbModule {
  constructor(private lazy: LazyTranslationService) {
    lazy.add('db');
  }
}
