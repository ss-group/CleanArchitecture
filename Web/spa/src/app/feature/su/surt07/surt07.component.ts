import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Surt07Service } from './surt07.service';
import { Router } from '@angular/router';
import { MessageService, SaveDataService } from '@app/core';

@Component({
  selector: 'app-surt07',
  templateUrl: './surt07.component.html'
})
export class Surt07Component implements OnInit {

  page = new Page();
  keyword: string = '';
  passwordPolicy = [];

  constructor(
    private su: Surt07Service,
    private router: Router,
    private modal: ModalService,
    private ms: MessageService,
    private util: FormUtilService,
    private saver: SaveDataService
  ) { }

  ngOnInit() {
    const saveData = this.saver.retrive('SURT07');
    if (saveData) this.keyword = saveData;
    const savePage = this.saver.retrive('SURT07Page');
    if (savePage) this.page = savePage;
    this.search();
  }

  ngOnDestroy() {
    this.saver.save(this.keyword, 'SURT07');
    this.saver.save(this.page, 'SURT07Page');
  }

  search() {
    this.su.getPasswordPolicies(this.keyword, this.page)
      .pipe()
      .subscribe(res => {
        this.passwordPolicy = res.rows;
        this.page.totalElements = res.count;
      });
  }

  add() {
    this.router.navigate(['/su/surt07/detail']);
  }

  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }

  remove(code, version) {
    this.modal.confirm("message.STD00003").subscribe(res => {
      if (res) {
        this.su.delete(code, version)
          .subscribe(() => {
            this.ms.success("message.STD00014");
            this.page = this.util.setPageIndex(this.page);
            this.search();
          });
      }
    })
  }
}
