import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Dbrt14Service } from './dbrt14.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt14',
  templateUrl: './dbrt14.component.html',
  styleUrls: ['./dbrt14.component.scss']
})
export class Dbrt14Component implements OnInit {
  page = new Page();
  keyword: string = '';
  project = [];
  constructor(
    private router: Router,
    private cs: Dbrt14Service,
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
    this.cs.getProject(this.keyword, this.page)
      .subscribe(res => {
        this.project = res.rows;
        this.page.totalElements = res.count;
      });
  }
  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }
  
  remove(id,code, version) {
    this.modal.confirm("message.STD00003").subscribe(
      (res) => {
        if (res) {
          this.cs.delete(id,code, version)
            .subscribe(() => {
              this.as.success("message.STD00014");
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }

  add() {
    this.router.navigate(['/db/dbrt14/detail']);
  }

}
