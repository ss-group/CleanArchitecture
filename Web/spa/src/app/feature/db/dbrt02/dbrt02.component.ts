import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { Dbrt02Service } from './dbrt02.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt02',
  templateUrl: './dbrt02.component.html',
  styleUrls: ['./dbrt02.component.scss']
})
export class Dbrt02Component implements OnInit {
  page = new Page();
  keyword: string = '';
  region = [];
  constructor(
    private router: Router,
    private cs: Dbrt02Service,
    private modal:ModalService,
    private as:MessageService,
    private util:FormUtilService
  ) { }

  onTableEvent(){

  }

  ngOnInit() {
    this.search();
  }

  search() {
    this.cs.getRegion(this.keyword, this.page)
      .subscribe(res => {
        this.region = res.rows;
        this.page.totalElements = res.count;
      });
  }

  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }
  
  remove(id, version) {
    this.modal.confirm("message.STD00003").subscribe(
      (res) => {
        if (res) {
          this.cs.delete(id, version)
            .subscribe(() => {
              this.as.success("message.STD00014");
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }

  add() {
    this.router.navigate(['/db/dbrt02/detail']);
  }

}
