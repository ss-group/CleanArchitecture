import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable,of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { DbCountry, Dbrt01Service } from './dbrt01.service';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt01-detail',
  templateUrl: './dbrt01-detail.component.html',
  styleUrls: ['./dbrt01-detail.component.scss']
})
export class Dbrt01DetailComponent implements OnInit {
  
  dbrt01Form: FormGroup;
  country:DbCountry = {} as DbCountry;
  submited:boolean=false;
  saving:boolean=false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt01Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.country = data.dbrt01.detail;
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt01Form.markAsPristine();
    if (this.country.countryId) {
      this.dbrt01Form.patchValue(this.country);
      this.dbrt01Form.controls.countryCode.disable();
    }
  }

  createForm(){
    this.dbrt01Form = this.fb.group({
      countryCode: [null, [Validators.required,Validators.maxLength(10)]],
      countryCodeMua: [null, [Validators.required,Validators.maxLength(10)]],
      countryNameTha: [null, [Validators.required,Validators.maxLength(100)]],
      countryNameEng: [null,Validators.maxLength(100)],
      countryShortNameTha: [null, [Validators.required,Validators.maxLength(100)]],
      countryShortNameEng: [null,Validators.maxLength(100)],
      active: true
    });
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt01Form);
    if (this.dbrt01Form.invalid) {
      return;
    }
    Object.assign(this.country, this.dbrt01Form.value);
    this.saving = true;
    this.cs.save(this.country).pipe(
      switchMap(() => this.cs.getCountryDetail(this.country.countryCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbCountry) => {
      this.country = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt01Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
