import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SuCompany, Surt01Service } from './surt01.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormUtilService, ModalService, Page } from '@app/shared';
import { MessageService, SaveDataService } from '@app/core';
import { switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-surt01-division',
  templateUrl: './surt01-division.component.html'
})
export class Surt01DivisionComponent implements OnInit {

  companyForm: FormGroup;
  company: { companyCode: string, companyName: string };
  divisions = [];
  page = new Page();
  keyword: string = '';

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private su: Surt01Service,
    private router: Router,
    private saver: SaveDataService
  ) { }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.company = data.surt01.detail;
    });
    if (this.company.companyCode)
      this.search();
  }

  search() {
    this.su.getDivisions(this.keyword, this.company.companyCode, this.page)
      .pipe()
      .subscribe(res => {
        this.company = res.company;
        this.divisions = res.divisions.rows;
        this.page.totalElements = res.divisions.count;
        this.rebuildForm();
      });
  }

  createForm() {
    this.companyForm = this.fb.group({
      companyCode: null,
      companyName: null,
    });
  }

  rebuildForm() {
    this.companyForm.markAsPristine();
    this.companyForm.patchValue(this.company);
    this.companyForm.controls.companyCode.disable();
    this.companyForm.controls.companyName.disable();
  }

  add() {
    this.router.navigate(['/su/surt01/division/detail'], { state: { companyCode: this.company.companyCode } });
  }

  remove(divCode, version) {
    this.modal.confirm("message.STD00003").subscribe(
      (res) => {
        if (res) {
          this.su.deleteDivision(this.company.companyCode, divCode, version)
            .subscribe(() => {
              this.ms.success("message.STD00014");
              this.page = this.util.setPageIndex(this.page);
              this.search();
            });
        }
      })
  }

  enter(value) {
    this.keyword = value;
    this.page.index = 0;
    this.search();
  }

}
