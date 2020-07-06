import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Dbrt10Service } from './dbrt10.service';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt10',
  templateUrl: './dbrt10.component.html',
  styleUrls: ['./dbrt10.component.scss']
})
export class Dbrt10Component implements OnInit {

  page = new Page();
  keyword: string = '';
  educationtype = [];
  criteriaForm:FormGroup;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private cs: Dbrt10Service,
    private modal:ModalService,
    private as:MessageService,
    private util:FormUtilService
    ) {
      this.createForm();
    }

  onTableEvent(){
    
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
    this.cs.getEducationTypies(this.keyword, this.page)
      .subscribe(res => {
        this.educationtype = res.rows;
        this.page.totalElements = res.count;
      });
  }
  add() {
    this.router.navigate(['/db/dbrt10/detail']);
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
  createForm(){
    this.criteriaForm = this.fb.group({
      educationTypeCode:[null,Validators.required],
      educationTypeCodeMua:[null,Validators.required],
      educationTypeNameTha:[null,Validators.required],
      educationTypeNameEng:null,
      typeLevel:[null,Validators.required],
      active:true
    });
  }

}
