import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SuParameter, Surt05Service } from './surt05.service';
import { ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-surt05-detail',
  templateUrl: './surt05-detail.component.html'
})
export class Surt05DetailComponent implements OnInit {

  parameterForm: FormGroup;
  parameter: SuParameter = {} as SuParameter;
  saving: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private su: Surt05Service
  ) { }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.parameter = data.surt05.detail;
      this.rebuildForm();
    });
  }

  createForm() {
    this.parameterForm = this.fb.group({
      parameterGroupCode: [null, Validators.required],
      parameterCode: [null, Validators.required],
      parameterValue: [null, [Validators.required, Validators.maxLength(200)]],
      remark: [null, Validators.maxLength(200)],
    });
  }

  rebuildForm() {
    this.parameterForm.markAsPristine();
    this.parameterForm.patchValue(this.parameter);
    this.parameterForm.controls.parameterGroupCode.disable();
    this.parameterForm.controls.parameterCode.disable();
  }

  save() {
    this.util.markFormGroupTouched(this.parameterForm);
    if (this.parameterForm.invalid) {
      return;
    }
    Object.assign(this.parameter, this.parameterForm.value);
    this.su.save(this.parameter).pipe(
      switchMap(() => this.su.getParameter(this.parameter.parameterGroupCode, this.parameter.parameterCode))
    ).subscribe((result: SuParameter) => {
      this.parameter = result;
      this.rebuildForm();
      this.ms.success("บันทึกข้อมูลสำเร็จ");
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.parameterForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }
}
