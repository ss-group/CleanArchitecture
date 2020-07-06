import { Component, OnInit } from '@angular/core';
import { Surt03Service } from './surt03.service';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { MessageService, SaveDataService } from '@app/core';

@Component({
  selector: 'app-surt03',
  templateUrl: './surt03.component.html'
})
export class Surt03Component implements OnInit {

  page = new Page();
  keyword: string = '';
  menus = [];

  constructor(
    private su: Surt03Service,
    private router: Router,
    private modal: ModalService,
    private ms: MessageService,
    private util: FormUtilService,
    private saver: SaveDataService
  ) { }

  ngOnInit() {
    const saveData = this.saver.retrive('surt03');
    if (saveData) this.keyword = saveData;
    const savePage = this.saver.retrive('surt03Page');
    if (savePage) this.page = savePage;
    this.search();
  }

  ngOnDestroy() {
    this.saver.save(this.keyword, 'surt03');
    this.saver.save(this.page, 'surt03Page');
  }

  search() {
    this.su.getMenus(this.keyword, this.page)
      .pipe()
      .subscribe(res => {
        this.menus = res.rows;
        this.page.totalElements = res.count;
      });
  }

  add() {
    this.router.navigate(['/su/surt03/detail']);
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
