import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Dbrt18Service, DbListItem } from './dbrt18.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt18-detail',
  templateUrl: './dbrt18-detail.component.html',
  styleUrls: ['./dbrt18-detail.component.scss']
})
export class Dbrt18DetailComponent implements OnInit {

  page = new Page();
  keyword: string = '';
  listDetail = [];
  code: any = '';
  listCode: DbListItem = {} as DbListItem;
  constructor(
    private route: Router,
    private router: ActivatedRoute,
    private cs: Dbrt18Service,
    private modal:ModalService,
    private as:MessageService,
    private util:FormUtilService
  ) { }

  search(){
    this.cs.getListItemDetail(this.code,this.keyword, this.page)
    .subscribe(res => {
      this.listDetail = res.rows;
      this.page.totalElements = res.count;

    });
  }

  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }

  searchDetail(){
    this.cs.getListItemDetail(this.code,this.keyword, this.page)
    this.router.data.subscribe((data) => {
      this.listDetail = data.dbrt18.detail.rows;
      this.page.totalElements = data.dbrt18.detail.count;
    });
  }
  

  ngOnInit() {
   this.router.data.subscribe((data) => {
    this.code = data.dbrt18.detail.rows[0].listItemGroupCode
   })
   
    this.searchDetail()
  }

  remove(code,code2, version) {
    this.modal.confirm("message.STD00003").subscribe(
      (res) => {
        if (res) {
          this.cs.delete(code, code2, version)
            .subscribe(() => {
              this.as.success("message.STD00014");
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }

  add() {
    this.route.navigate(['/db/dbrt18/detail/managament'],{state: {code : this.code}});
  }
}
