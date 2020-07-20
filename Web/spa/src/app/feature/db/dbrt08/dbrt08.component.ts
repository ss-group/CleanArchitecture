import { Component, OnInit } from '@angular/core';
import { Page, ModalService, FormUtilService } from '@app/shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from '@app/core';
import { Dbrt08Service } from './dbrt08.service';

@Component({
  selector: 'app-dbrt08',
  templateUrl: './dbrt08.component.html',
  styleUrls: ['./dbrt08.component.scss']
})
export class Dbrt08Component implements OnInit {
  page = new Page();
  searchForm: FormGroup;
  employees = [];
  master = {
    divlist: [],
    groupTypeCode: [],
    statusWork: []
  };
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private db: Dbrt08Service
  ) {

  }

  ngOnInit() {
    this.createSearchForm();
    this.route.data.subscribe((data) => {
      this.master = data.dbrt08.master;
    });
    this.search(true);
  }

  createSearchForm() {
    this.searchForm = this.fb.group({
      employeeCode: null,
      teacherCode : null,
      employeeName: null,
      divCode: null,
      groupTypeCode: null,
      turnoverDate: '2',
    })
  }

  search(resetIndex) {
    if (resetIndex) this.page.index = 0;
    this.db.getEmployees(this.searchForm.value, this.page)
      .subscribe(res => {
        this.employees = res.rows;
        this.page.totalElements = res.count;
      });
  }

  clear() {
    this.searchForm.reset();
    this.search(true);
  }

  add() {
    this.router.navigate(['/db/dbrt08/detail']);
  }

}
