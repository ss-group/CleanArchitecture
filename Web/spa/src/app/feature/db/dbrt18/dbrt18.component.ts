import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { Dbrt18Service } from './dbrt18.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt18',
  templateUrl: './dbrt18.component.html',
  styleUrls: ['./dbrt18.component.scss']
})
export class Dbrt18Component implements OnInit {

  page = new Page();
  keyword: string = '';
  listItem = [];
  constructor(
    private router: Router,
    private cs: Dbrt18Service,
    private modal:ModalService,
    private as:MessageService,
    private util:FormUtilService
  ) { }

  search(){
    this.cs.getListItem(this.keyword, this.page)
    .subscribe(res => {
      this.listItem = res.rows;
      this.page.totalElements = res.count;
    });
  }

  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }
  

  ngOnInit() {
    this.search();
  }


}
