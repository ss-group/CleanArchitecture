import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from '@app/core';
import { Dbrt09Service } from './dbrt09.service';

@Component({
  selector: 'app-dbrt09',
  templateUrl: './dbrt09.component.html',
  styleUrls: ['./dbrt09.component.scss']
})
export class Dbrt09Component implements OnInit {

  page = new Page();
  keyword: string = '';
  educationtype = [];

  criteriaForm:FormGroup;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private cs: Dbrt09Service,
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
    this.router.navigate(['/db/dbrt09/detail']);
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
      educationTypeLevel:[null,Validators.required],
      educationTypeLevelNameTha:[null,Validators.required],
      educationTypeLevelNameEng:null,
      educationLevelShortNameTha:null,
      educationLevelShortNameEng:null,
      educationTypeCodeMua:null,
      active:true
    });
  }

  
}
