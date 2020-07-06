import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Dbrt07Service } from './dbrt07.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt07',
  templateUrl: './dbrt07.component.html',
  styleUrls: ['./dbrt07.component.scss']
})
export class Dbrt07Component implements OnInit {

  page = new Page();
  keyword: string = '';
  prename = [];
  criteriaForm:FormGroup;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private cs: Dbrt07Service,
    private modal:ModalService,
    private as:MessageService,
    private util:FormUtilService
    ) {
     
    }

  ngOnInit() {
    this.search();
  }
  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }
  search() {
    this.cs.getPreNames(this.keyword, this.page)
      .subscribe(res => {
        this.prename = res.rows;
        this.page.totalElements = res.count;
      });
  }
  add() {
    this.router.navigate(['/db/dbrt07/detail']);
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

}
