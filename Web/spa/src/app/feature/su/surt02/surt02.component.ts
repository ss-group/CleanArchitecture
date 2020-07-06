import { Component, OnInit } from '@angular/core';
import { Surt02Service } from './surt02.service';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { MessageService, SaveDataService } from '@app/core';

@Component({
  selector: 'app-surt02',
  templateUrl: './surt02.component.html'
})
export class Surt02Component implements OnInit {

  page = new Page();
  keyword: string = '';
  programs = [];

  constructor(
    private su: Surt02Service,
    private router: Router,
    private modal: ModalService,
    private ms: MessageService,
    private util: FormUtilService,
    private saver: SaveDataService
  ) { }

  ngOnInit() {
    const saveData = this.saver.retrive('surt02');
    if (saveData) this.keyword = saveData;
    const savePage = this.saver.retrive('surt02Page');
    if (savePage) this.page = savePage;
    this.search();
  }

  ngOnDestroy() {
    this.saver.save(this.keyword, 'surt02');
    this.saver.save(this.page, 'surt02Page');
  }

  search() {
    this.su.getPrograms(this.keyword, this.page)
      .pipe()
      .subscribe(res => {
        this.programs = res.rows;
        this.page.totalElements = res.count;
      });
  }

  add() {
    this.router.navigate(['/su/surt02/detail']);
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
          this.su.deleteProgram(code, version)
            .subscribe(() => {
              this.ms.success("message.STD00014");
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }
}
