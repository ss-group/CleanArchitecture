import { Component, OnInit } from '@angular/core';
import { Surt01Service } from './surt01.service';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { MessageService, SaveDataService } from '@app/core';

@Component({
  selector: 'app-surt01',
  templateUrl: './surt01.component.html'
})
export class Surt01Component implements OnInit {

  page = new Page();
  keyword: string = '';
  companies = [];

  constructor(
    private su: Surt01Service,
    private router: Router,
    private modal: ModalService,
    private ms: MessageService,
    private util: FormUtilService,
    private saver: SaveDataService
  ) { }

  ngOnInit() {
    const saveData = this.saver.retrive('surt01');
    if (saveData) this.keyword = saveData;
    const savePage = this.saver.retrive('surt01Page');
    if (savePage) this.page = savePage;
    this.search();
  }

  ngOnDestroy() {
    this.saver.save(this.keyword, 'surt01');
    this.saver.save(this.page, 'surt01Page');
  }

  search() {
    this.su.getCompanies(this.keyword, this.page)
      .pipe()
      .subscribe(res => {
        this.companies = res.rows;
        this.page.totalElements = res.count;
      });
  }

  add() {
    this.router.navigate(['/su/surt01/company/detail']);
  }

  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }

  remove(code, version) {
    this.modal.confirm("message.STD00003").subscribe(
      (res) => {
        if (res) {
          this.su.deleteCompany(code, version)
            .subscribe(() => {
              this.ms.success("message.STD00014");
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }
}
