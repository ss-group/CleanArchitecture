import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Dbrt01Service } from './dbrt01.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt01',
  templateUrl: './dbrt01.component.html',
  styleUrls: ['./dbrt01.component.scss']
})
export class Dbrt01Component implements OnInit {
  page = new Page();
  keyword: string = '';
  country = [];
  constructor(
    private router: Router,
    private cs: Dbrt01Service,
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
    this.cs.getCountry(this.keyword, this.page)
      .subscribe(res => {
        this.country = res.rows;
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
    this.router.navigate(['/db/dbrt01/detail']);
  }

}
