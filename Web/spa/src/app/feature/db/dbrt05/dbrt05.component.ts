import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Dbrt05Service } from './dbrt05.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt05',
  templateUrl: './dbrt05.component.html',
  styleUrls: ['./dbrt05.component.scss']
})
export class Dbrt05Component implements OnInit {
  page = new Page();
  keyword: string = '';
  subdistrict = [];
  constructor(
    private router: Router,
    private cs: Dbrt05Service,
    private modal: ModalService,
    private as: MessageService,
    private util: FormUtilService
  ) { }


  onTableEvent() {

  }

  search() {
    this.cs.getSubDistrict(this.keyword, this.page)
      .subscribe(res => {
        this.subdistrict = res.rows;
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

  add() {
    this.router.navigate(['/db/dbrt05/detail']);
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
