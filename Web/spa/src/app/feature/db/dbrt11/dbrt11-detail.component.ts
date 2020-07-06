import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { DbFac, Dbrt11Service } from './dbrt11.service';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt11-detail',
  templateUrl: './dbrt11-detail.component.html',
  styleUrls: ['./dbrt11-detail.component.scss']
})
export class Dbrt11DetailComponent implements OnInit {

  dbrt11Form: FormGroup;
  fac:DbFac = {} as DbFac;
  submited:boolean=false;
  saving:boolean=false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt11Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.fac = data.dbrt11.detail;
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt11Form.markAsPristine();
    if (this.fac.facCode) {
      this.dbrt11Form.patchValue(this.fac);
      this.dbrt11Form.controls.facCode.disable();
    }
  }

  createForm() {
    this.dbrt11Form = this.fb.group({
      facCode: [null, [Validators.required, Validators.maxLength(2)]],
      facNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      facNameEng: [null, [Validators.maxLength(100)]],
      facShortNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      facShortNameEng: [null, [Validators.maxLength(100)]],
      fnPayinCode: [null, [Validators.required, Validators.maxLength(5)]],
      formatCode: [null, [Validators.required, Validators.maxLength(10)]],
      facCodeMua: [null, [Validators.required, Validators.maxLength(10)]],
      active: true
    });
  }
  save() {
    this.util.markFormGroupTouched(this.dbrt11Form);
    if (this.dbrt11Form.invalid) {
      return;
    }
    Object.assign(this.fac, this.dbrt11Form.value);
    this.saving = true;
    this.cs.save(this.fac).pipe(
      switchMap(() => this.cs.getFacType(this.fac.facCode)),
      finalize(() => {
        // console.log("fiz");
        this.saving = false;
      })
    ).subscribe((result: DbFac) => {
      this.fac = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt11Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
