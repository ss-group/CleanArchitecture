import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Dbrt19Service } from './dbrt19.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt19',
  templateUrl: './dbrt19.component.html',
  styleUrls: ['./dbrt19.component.scss']
})
export class Dbrt19Component implements OnInit {
  page = new Page();
  degreeForm: FormGroup;
  keyword: string = '';
  degree = [];
  constructor(
    private router: Router,
    private fb: FormBuilder,
    private cs: Dbrt19Service,
    private modal: ModalService,
    private as: MessageService,
    private util: FormUtilService
  ) {

  }
  ngOnInit() {
    this.search();
  }
  search() {
    this.cs.getDegree(this.keyword, this.page)
      .subscribe(res => {
        this.degree = res.rows;
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
    this.router.navigate(['/db/dbrt19/detail']);
  }
}
