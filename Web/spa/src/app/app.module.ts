import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localeTh from '@angular/common/locales/th';

registerLocaleData(localeTh);

import { ShellModule } from './shell/shell.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoadingComponent } from './loading/loading.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { CoreModule } from '@app/core';
import { EmptyComponent } from './feature/empty/empty.component';
import { AccordionModule } from 'ngx-bootstrap/accordion';

@NgModule({
  declarations: [
    AppComponent,LoadingComponent, EmptyComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule,
    ShellModule,
    ServiceWorkerModule.register('./ngsw-worker.js', { enabled: environment.production }),
    AccordionModule.forRoot()
  ],
  providers: [{provide: LOCALE_ID, useValue: "th-TH"}],
  bootstrap: [AppComponent]
})
export class AppModule { }
