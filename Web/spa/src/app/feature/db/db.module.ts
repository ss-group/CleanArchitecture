import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DbRoutingModule } from './db-routing.module';
import { SharedModule } from '@app/shared';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { LazyTranslationService } from '@app/core';

import { Dbrt01Component } from './dbrt01/dbrt01.component';
import { Dbrt01DetailComponent } from './dbrt01/dbrt01-detail.component';
import { Dbrt01Service } from './dbrt01/dbrt01.service';
import { Dbrt22Component } from './dbrt22/dbrt22.component';

@NgModule({
  declarations: [
    Dbrt01Component,
    Dbrt01DetailComponent,
    Dbrt22Component,
  ],
  imports: [
    CommonModule,
    DbRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [
    Dbrt01Service
  ]
})
export class DbModule {
  constructor(private lazy: LazyTranslationService) {
    lazy.add('db');
  }
}
