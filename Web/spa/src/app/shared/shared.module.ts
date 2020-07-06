import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { ModalModule } from 'ngx-bootstrap/modal';
import {MatDatepickerModule, MAT_DATE_FORMATS, DateAdapter } from '@angular/material';
import { TooltipModule } from 'ngx-bootstrap/tooltip';

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { CardComponent } from './component/card/card.component';
import { NgxDatatableModule } from 'ss-group-datatable'
import { TableComponent } from './component/datatable/table/table.component';
import { TableClientComponent } from './component/datatable/table-client/table-client.component';
import { ThaiDatePipe } from './pipe/thaidate.pipe';
import { CheckboxComponent } from './component/checkbox/checkbox.component';
import { TextboxComponent } from './component/textbox/textbox.component';
import { RadioComponent } from './component/radio/radio.component';
import { SelectComponent } from './component/select/select.component';
import { LookupComponent } from './component/lookup/lookup.component';
import { SearchComponent } from './component/search/search.component';
import { NumberComponent } from './component/number/number.component';
import { ToolbarComponent } from './component/toolbar/toolbar.component';
import { ModalComponent } from './component/modal/modal.component';
import { ConfirmComponent } from './component/modal/confirm/confirm.component';
import { ModalService } from './component/modal/modal.service';
import { DatepickerComponent } from './component/datepicker/datepicker.component';
import { AppDateAdapter, APP_DATE_FORMATS } from './component/datepicker/date.adaptor';
import { AreaComponent } from './component/area/area.component';
import { ImageComponent } from './component/attachment/image.component';
import { AttachmentComponent } from './component/attachment/attachment.component';
import { DragDropDirective } from './component/attachment/drag-drop.directive';
import { CurrencyComponent } from './component/number/currency.component';
import { FormUtilService } from './service/form-util.service';
import { YearComponent } from './component/year/year.component';
import { StatusComponent } from './component/status/status.component';
import { RadioCheckboxComponent } from './component/radio/radio-checkbox.component';
import { TranslateModule } from '@ngx-translate/core';
import { CalendarModule, DateAdapter as CalendarDateAdapter } from './component/calendar/calendar.module';
import { adapterFactory } from './component/calendar/date-adapters/date-fns';
import { TimeComponent } from './component/time/time.component';
import { AutoCompleteComponent } from './component/auto-complete/auto-complete.component';
import { BaseLookupComponent } from './component/lookup/base-lookup.component';
import { BaseLookupMultipleComponent } from './component/lookup/base-lookup-multiple.component';
import { CheckButtonComponent } from './component/checkbox/check-button.component';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';
import { ImportComponent } from './component/import/import.component';
import { DatePipe } from '@angular/common';
import { EditorModule } from '@tinymce/tinymce-angular';
import { SemesterComponent } from './component/semester/semester.component';
import { RadioButtonComponent } from './component/radio/radio-button.component';

declare let tinymce: any;

// import 'tinymce/themes/modernmodern/theme';
import 'tinymce/plugins/paste';
import 'tinymce/plugins/link';
import 'tinymce/plugins/image';
import 'tinymce/plugins/preview';
import 'tinymce/plugins/contextmenu';
import 'tinymce/plugins/textcolor';
import 'tinymce/plugins/table';
import 'tinymce/plugins/code';
import 'tinymce/plugins/advlist';
import 'tinymce/plugins/autolink';
import 'tinymce/plugins/lists';
import 'tinymce/plugins/hr';
import 'tinymce/plugins/anchor';
import 'tinymce/plugins/pagebreak';
import 'tinymce/plugins/searchreplace';
import 'tinymce/plugins/wordcount';
import 'tinymce/plugins/visualblocks';
import 'tinymce/plugins/visualchars';
import 'tinymce/plugins/insertdatetime';
import 'tinymce/plugins/media';
import 'tinymce/plugins/nonbreaking';
import 'tinymce/plugins/directionality';
import 'tinymce/plugins/template';
import 'tinymce/plugins/colorpicker';
import 'tinymce/plugins/textpattern';
import 'tinymce-emoji';
import { ThaiDateTimePipe } from './pipe/thaidateTime.pipe';

tinymce.init({
  selector: 'editor',
  convert_urls: false
});

@NgModule({
  declarations: [CardComponent, TableComponent, TableClientComponent,ThaiDatePipe,ThaiDateTimePipe, CheckboxComponent, TextboxComponent, RadioComponent, SelectComponent, LookupComponent, SearchComponent, NumberComponent, ToolbarComponent, ModalComponent, ConfirmComponent, DatepickerComponent, AreaComponent, ImageComponent, AttachmentComponent,DragDropDirective, CurrencyComponent, YearComponent, StatusComponent, RadioCheckboxComponent,TimeComponent, AutoCompleteComponent,BaseLookupComponent,BaseLookupMultipleComponent, CheckButtonComponent, ImportComponent, SemesterComponent,RadioButtonComponent ],
  imports: [
    CommonModule,
    ModalModule.forRoot(),
    CollapseModule.forRoot(),
    TabsModule.forRoot(),
    NgxDatatableModule,
    FormsModule,
    NgSelectModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    TooltipModule.forRoot(),
    TranslateModule,
    CalendarModule.forRoot({
      provide: CalendarDateAdapter,
      useFactory: adapterFactory
    }),
    NgxExtendedPdfViewerModule,
    EditorModule
  ],
  entryComponents: [ConfirmComponent],
  exports:[
    ModalModule,
    NgSelectModule,
    CollapseModule,
    TabsModule,
    CardComponent,
    NgxDatatableModule,
    TooltipModule,
    TableComponent,
    TableClientComponent,
    ThaiDatePipe,
    ThaiDateTimePipe,
    CheckboxComponent,
    RadioComponent,
    TextboxComponent,
    DatepickerComponent,
    TimeComponent,
    NumberComponent,
    YearComponent,
    CurrencyComponent,
    LookupComponent,
    AutoCompleteComponent,
    SelectComponent,
    SearchComponent,
    ToolbarComponent,
    ModalComponent,
    ConfirmComponent,
    AreaComponent,
    AttachmentComponent,
    ImageComponent,
    StatusComponent,
    RadioCheckboxComponent,
    CheckButtonComponent,
    TranslateModule,
    CalendarModule,
    NgxExtendedPdfViewerModule,
    ImportComponent,
    EditorModule,
    SemesterComponent,
    RadioButtonComponent
  ],
  providers:[ModalService,FormUtilService,
    {provide: DateAdapter, useClass: AppDateAdapter}, {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS},
    DatePipe
  ]
})
export class SharedModule { }
