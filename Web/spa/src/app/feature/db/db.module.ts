import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DbRoutingModule } from './db-routing.module';
import { SharedModule } from '@app/shared';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { LazyTranslationService } from '@app/core';

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

import { Dbrt01Service } from './dbrt01/dbrt01.service';
import { Dbrt02Service } from './dbrt02/dbrt02.service';
import { Dbrt03Service } from './dbrt03/dbrt03.service';
import { Dbrt04Service } from './dbrt04/dbrt04.service';
import { Dbrt05Service } from './dbrt05/dbrt05.service';
import { Dbrt06Service } from './dbrt06/dbrt06.service';
import { Dbrt07Service } from './dbrt07/dbrt07.service';
import { Dbrt08Service } from './dbrt08/dbrt08.service';
import { Dbrt09Service } from './dbrt09/dbrt09.service';
import { Dbrt10Service } from './dbrt10/dbrt10.service';
import { Dbrt11Service } from './dbrt11/dbrt11.service';
import { Dbrt12Service } from './dbrt12/dbrt12.service';
import { Dbrt13Service } from './dbrt13/dbrt13.service';
import { Dbrt14Service } from './dbrt14/dbrt14.service';
import { Dbrt15Service } from './dbrt15/dbrt15.service';
import { Dbrt16Service } from './dbrt16/dbrt16.service';
import { Dbrt17Service } from './dbrt17/dbrt17.service';
import { Dbrt18Service } from './dbrt18/dbrt18.service';
import { Dbrt19Service } from './dbrt19/dbrt19.service';
import { Dbrt20Service } from './dbrt20/dbrt20.service';
import { Dbrt21Service } from './dbrt21/dbrt21.service';


@NgModule({
  declarations: [
    Dbrt01Component,
    Dbrt01DetailComponent,
    Dbrt02Component,
    Dbrt02DetailComponent,
    Dbrt03Component,
    Dbrt03DetailComponent,
    Dbrt04Component,
    Dbrt04DetailComponent,
    Dbrt05Component,
    Dbrt05DetailComponent,
    Dbrt06Component,
    Dbrt06DetailComponent,
    Dbrt07Component,
    Dbrt07DetailComponent,
    Dbrt08Component,
    Dbrt08DetailComponent,
    Dbrt09Component,
    Dbrt09DetailComponent,
    Dbrt10Component,
    Dbrt10DetailComponent,
    Dbrt11Component,
    Dbrt11DetailComponent,
    Dbrt12Component,
    Dbrt12DetailComponent,
    Dbrt13Component,
    Dbrt13DetailComponent,
    Dbrt14Component,
    Dbrt14DetailComponent,
    Dbrt15Component,
    Dbrt15DetailComponent,
    Dbrt16Component,
    Dbrt16DetailComponent,
    Dbrt17Component,
    Dbrt17DetailComponent,
    Dbrt18Component,
    Dbrt18DetailComponent,
    Dbrt18DetailManagamentComponent,
    Dbrt19Component,
    Dbrt19DetailComponent,
    Dbrt20Component,
    Dbrt20DetailComponent,
    Dbrt21Component,
    Dbrt21DetailComponent
  ],
  imports: [
    CommonModule,
    DbRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [
    Dbrt01Service,
    Dbrt02Service,
    Dbrt03Service,
    Dbrt04Service,
    Dbrt05Service,
    Dbrt06Service,
    Dbrt07Service,
    Dbrt08Service,
    Dbrt09Service,
    Dbrt10Service,
    Dbrt11Service,
    Dbrt12Service,
    Dbrt13Service,
    Dbrt14Service,
    Dbrt15Service,
    Dbrt16Service,
    Dbrt17Service,
    Dbrt18Service,
    Dbrt19Service,
    Dbrt20Service,
    Dbrt21Service
  ]
})
export class DbModule {
  constructor(private lazy: LazyTranslationService) {
    lazy.add('db');
  }
}
