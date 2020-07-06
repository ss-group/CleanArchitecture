import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Dbrt20Service } from './dbrt20.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt20',
  templateUrl: './dbrt20.component.html',
  styleUrls: ['./dbrt20.component.scss']
})
export class Dbrt20Component implements OnInit {
  page = new Page();
  degreeForm: FormGroup;
  keyword: string = '';
  department = [];
  constructor(
    private router: Router,
    private fb: FormBuilder,
    private cs: Dbrt20Service,
    private modal: ModalService,
    private as: MessageService,
    private util: FormUtilService
  ) {

  }
  ngOnInit() {
    this.search();
  }
  search() {
    this.cs.getDepartment(this.keyword, this.page)
      .subscribe(res => {
        this.department = res.rows;
        this.page.totalElements = res.count;
      });
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
          this.cs.delete(code, version)
            .subscribe(() => {
              this.as.success("message.STD00014");
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }

  add() {
    this.router.navigate(['/db/dbrt20/detail']);
  }
}
