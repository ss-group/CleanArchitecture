import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SuPasswordPolicy, Surt07Service } from './surt07.service';
import { ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService, Pattern } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-surt07-detail',
  templateUrl: './surt07-detail.component.html'
})
export class Surt07DetailComponent implements OnInit {

  master = { courseTypes: [], categories: [] };
  passwordPolicyForm: FormGroup;
  passwordPolicy: SuPasswordPolicy = {} as SuPasswordPolicy;
  saving: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private su: Surt07Service
  ) { }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.passwordPolicy = data.surt07.detail;
      this.rebuildForm();
    });
  }

  createForm() {
    this.passwordPolicyForm = this.fb.group({
      passwordPolicyCode: [null, [Validators.required, Validators.maxLength(10), Validators.pattern(Pattern.UpperOnly)]],
      passwordPolicyName: [null, [Validators.required, Validators.maxLength(100)]],
      passwordPolicyDescription: [null, Validators.maxLength(200)],
      failTime: [null, [Validators.maxLength(4), Validators.min(0), Validators.max(9999)]],
      passwordAge: [null, [Validators.maxLength(4), Validators.min(0), Validators.max(9999)]],
      passwordMinimumLength: [null, [Validators.maxLength(4), Validators.min(0), Validators.max(9999)]],
      passwordMaximumLength: [null, [Validators.maxLength(4), Validators.min(0), Validators.max(9999)]],
      usingUppercase: false,
      usingLowercase: false,
      usingNumericChar: false,
      usingSpecialChar: false,
      active: true
    });
  }

  rebuildForm() {
    this.passwordPolicyForm.markAsPristine();
    if (this.passwordPolicy.passwordPolicyCode) {
      this.passwordPolicyForm.patchValue(this.passwordPolicy);
      this.passwordPolicyForm.controls.passwordPolicyCode.disable();
    }
  }

  save() {
    this.util.markFormGroupTouched(this.passwordPolicyForm);
    if (this.passwordPolicyForm.invalid) {
      return;
    }
    Object.assign(this.passwordPolicy, this.passwordPolicyForm.value);
    this.su.save(this.passwordPolicy).pipe(
      switchMap(() => this.su.getPasswordPolicy(this.passwordPolicy.passwordPolicyCode))
    ).subscribe((result: SuPasswordPolicy) => {
      this.passwordPolicy = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.passwordPolicyForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
