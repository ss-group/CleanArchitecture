import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { Router } from '@angular/router';
import { Dbrt03Service } from './dbrt03.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt03',
  templateUrl: './dbrt03.component.html',
  styleUrls: ['./dbrt03.component.scss']
})
export class Dbrt03Component implements OnInit {
 
  page = new Page();
  keyword: string = '';
  province = [];
  constructor(
    private router: Router,
    private cs: Dbrt03Service,
    private modal:ModalService,
    private as:MessageService,
    private util:FormUtilService
  ) { }


  onTableEvent(){

  }

  search() {
    this.cs.getProvince(this.keyword, this.page)
      .subscribe(res => {
        this.province = res.rows;
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
    this.router.navigate(['/db/dbrt03/detail']);
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

}
