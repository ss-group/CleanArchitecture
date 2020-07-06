import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Dbrt13Service } from './dbrt13.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt13',
  templateUrl: './dbrt13.component.html',
  styleUrls: ['./dbrt13.component.scss']
})
export class Dbrt13Component implements OnInit {
  
  page = new Page();
  keyword: string = '';
  major = [];
  constructor(
    private router: Router,
    private cs: Dbrt13Service,
    private modal:ModalService,
    private as:MessageService,
    private util:FormUtilService
  ) { }

  
  ngOnInit() {
    this.search();
  }

  search() {
    this.cs.getMajor(this.keyword, this.page)
      .subscribe(res => {
        this.major = res.rows;
        this.page.totalElements = res.count;
      });
  }

  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }
  
  add() {
    this.router.navigate(['/db/dbrt13/detail']);
  }

  remove(majorCode, facCode, programCode, version) {
    this.modal.confirm("message.STD00003").subscribe(
      (res) => {
        if (res) {
          this.cs.delete(majorCode, facCode, programCode, version)
            .subscribe(() => {
              this.as.success("message.STD00014");
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }
}
