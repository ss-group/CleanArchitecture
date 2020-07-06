import { Component, OnInit } from '@angular/core';
import { Surt05Service } from './surt05.service';
import { Router } from '@angular/router';
import { ModalService, FormUtilService, Page } from '@app/shared';
import { MessageService, SaveDataService } from '@app/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-surt05',
  templateUrl: './surt05.component.html'
})
export class Surt05Component implements OnInit {

  page = new Page();
  parameters = [];
  criteriaForm: FormGroup;
  master = { parameterGroupCodes: [] }

  constructor(
    private su: Surt05Service,
    private router: Router,
    private modal: ModalService,
    private ms: MessageService,
    private fb: FormBuilder,
    private util: FormUtilService,
    private saver: SaveDataService
  ) { }

  ngOnInit() {   
    this.createForm();
    this.su.getMaster().subscribe(data => {
      this.master.parameterGroupCodes = data.parameterGroupCodes;
    });

    const keyword = this.saver.retrive('surt05Keyword');
    if (keyword) this.criteriaForm.controls.keyWord.setValue(keyword);
    const parameterGroupCode = this.saver.retrive('surt05parameterGroupCode');
    if (parameterGroupCode) this.criteriaForm.controls.parameterGroupCode.setValue(parameterGroupCode);
    const savePage = this.saver.retrive('surt05Page');
    if (savePage) this.page = savePage;

    this.search();
  }

  ngOnDestroy() {
    this.saver.save(this.criteriaForm.controls.keyWord.value, 'surt05Keyword');
    this.saver.save(this.criteriaForm.controls.parameterGroupCode.value, 'surt05parameterGroupCode');
    this.saver.save(this.page, 'surt05Page');
  }

  createForm() {
    this.criteriaForm = this.fb.group({
      parameterGroupCode: null,
      keyWord: null
    });
  }

  search() {
    this.su.getParameters(this.criteriaForm.value, this.page)
      .pipe()
      .subscribe(res => {
        this.parameters = res.rows;
        this.page.totalElements = res.count;
      });
  }

  enter() {
    this.page.index = 0;
    this.search();
  }

  clearCriteria() {
    this.criteriaForm.reset();
  }
}
