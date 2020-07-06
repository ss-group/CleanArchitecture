import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService, ReportService, ReportParam } from '@app/shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Dbrt21Service } from './dbrt21.service';
import { MessageService } from '@app/core';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt21',
  templateUrl: './dbrt21.component.html',
  styleUrls: ['./dbrt21.component.scss']
})
export class Dbrt21Component implements OnInit {
  reportParam = {} as ReportParam;
  printing: boolean;
  page = new Page();
  keyword = '';
  facProgram = [];
  keywordSearch = '';
  constructor(
    private router: Router,
    private cs: Dbrt21Service,
    private modal: ModalService,
    private as: MessageService,
    private util: FormUtilService,
    public reportService: ReportService
  ) { }

  ngOnInit() {
    this.search();
  }

  search() {
    this.cs.getFacProgram(this.keyword, this.page)
      .subscribe(res => {
        this.facProgram = res.rows;
        this.page.totalElements = res.count;
        this.keywordSearch = this.keyword;
      });
  }

  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }

  add() {
    this.router.navigate(['/db/dbrt21/detail']);
  }

  remove(facCode, programCode, version) {
    this.modal.confirm('message.STD00003').subscribe(
      (res) => {
        if (res) {
          this.cs.delete(facCode, programCode, version)
            .subscribe(() => {
              this.as.success('message.STD00014');
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      });
  }

  print() {
    this.printing = true;

    const objParam = {
      keyword: this.keywordSearch
    };
    this.reportParam.paramsJson = objParam;
    this.reportParam.module = 'DB';
    this.reportParam.reportName = 'DBRP02';
    this.reportParam.exportType = 'PDF';

    this.reportService.generateReportBase64(this.reportParam).pipe(
      finalize(() => {
        this.printing = false;
      })
    ).subscribe((res: any) => {
      if (res) {
        this.DowloadFile(`data:application/pdf;base64,${res}`, this.reportParam.reportName);
      }
    });
  }

  async DowloadFile(data, reportName) {
    const a = document.createElement('a');
    a.href = data;
    a.download = reportName + '.pdf';
    a.click();
  }
}
