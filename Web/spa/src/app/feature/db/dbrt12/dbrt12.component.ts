import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Dbrt12Service } from './dbrt12.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt12',
  templateUrl: './dbrt12.component.html',
  styleUrls: ['./dbrt12.component.scss']
})
export class Dbrt12Component implements OnInit {

 
  page = new Page();
  keyword: string = '';
  program = [];
  constructor(
    private router: Router,
    private cs: Dbrt12Service,
    private modal:ModalService,
    private as:MessageService,
    private util:FormUtilService
  ) { }


  search() {
    this.cs.getProgram(this.keyword, this.page)
      .subscribe(res => {
        this.program = res.rows;
        this.page.totalElements = res.count;
      });
      // console.log(this.program)
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
    this.router.navigate(['/db/dbrt12/detail']);
  }
  
  remove(code,version) {
    this.modal.confirm("message.STD00003").subscribe(
      (res) => {
        if (res) {
          this.cs.delete(code,version)
            .subscribe(() => {
              this.as.success("message.STD00014");                        
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }
}
