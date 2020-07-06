import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Dbrt04Service } from './dbrt04.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt04',
  templateUrl: './dbrt04.component.html',
  styleUrls: ['./dbrt04.component.scss']
})
export class Dbrt04Component implements OnInit {
  page = new Page();
  keyword: string = '';
  district = [];
  constructor(
    private router: Router,
    private cs: Dbrt04Service,
    private modal:ModalService,
    private as:MessageService,
    private util:FormUtilService
  ) { }


  onTableEvent(){

  }

  search() {
    this.cs.getDistrict(this.keyword, this.page)
      .subscribe(res => {
        this.district = res.rows;
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
    this.router.navigate(['/db/dbrt04/detail']);
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
