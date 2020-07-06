import { Component, OnInit } from '@angular/core';
import { User, Surt06Service, UserProfile, UserPermission, UserType } from './surt06.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FormUtilService, ModalService, Pattern, RowState, SubscriptionDisposer, ParameterService } from '@app/shared';
import { MessageService, SupportedLanguages } from '@app/core';
import { Observable, of, BehaviorSubject, Subject } from 'rxjs';
import { finalize, takeUntil, switchMap, tap, map } from 'rxjs/operators';

interface Employee {
  employeeCode: string,
  studentId: number,
  personCompany: string
}
@Component({
  selector: 'app-surt06-detail',
  templateUrl: './surt06-detail.component.html',
  styleUrls: ['./surt06-detail.component.scss']
})
export class Surt06DetailComponent extends SubscriptionDisposer implements OnInit {
  master = { languages: [{ value: 0, text: 'ไทย' }, { value: 1, text: 'อังกฤษ' }], policies: [], profiles: [] };
  policies = [];
  profiles = {};
  policyStudent: string;
  userForm: FormGroup;
  userTypeParam: UserType;
  user: User;
  employee: Employee;
  employeeName: string;
  employeeChange = new Subject<any>();
  policyChange = new Subject<any>();
  saving = false;
  profileDelete = [];
  permisisonDelete = [];
  personLabel = '';
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    public ss: Surt06Service
  ) { super() }

  ngOnInit() {
    if (this.ss.userType.code == this.ss.userTypeParam.Student) {
      this.personLabel = 'label.SURT06.Student';
    }
    else if (this.ss.userType.code == this.ss.userTypeParam.VisitingProfessor) {
      this.personLabel = 'label.SURT06.VisitingProfessor';
    } else {
      this.personLabel = 'label.SURT06.Employee';
    }
    this.createForm();
    this.route.data.subscribe((data) => {
      Object.assign(this.master, data.surt06.master);
      this.userTypeParam = this.ss.userTypeParam;
      this.user = data.surt06.userTypeInfo.detail;
      this.employee = data.surt06.userTypeInfo.employee;
      this.policyStudent = this.ss.policyStudent;
      this.user.employeeCode = this.ss.userType.code == this.userTypeParam.Student ? this.employee.studentId : this.employee.employeeCode;
      this.rebuildForm();
    });
  }

  onSearchEmployee = (term, value) => {
    return this.ss.getAutoComplete(term, value, this.employee.personCompany).pipe(
      map(auto => auto.employees)
    );
  }

  createForm() {
    this.userForm = this.fb.group({
      userName: [null, [Validators.required, Validators.minLength(4), Validators.maxLength(20), Validators.pattern(Pattern.Username)]],
      employeeCode: [null, Validators.required],
      defaultLang: [0, Validators.required],
      passwordPolicyCode: [null, Validators.required],
      forceChangePassword: false,
      active: true,
      startEffectiveDate: [new Date(), Validators.required],
      endEffectiveDate: null,
    });
    this.userForm.controls.employeeCode.valueChanges.pipe(
      switchMap(() => this.employeeChange),
      takeUntil(this.ngUnsubscribe)
    ).subscribe(emp => {
      if (!this.userForm.controls.employeeCode.pristine) {
        if (this.ss.userType.code == this.userTypeParam.Student) {
          this.userForm.controls.userName.setValue(emp.studentCode || '');
          this.userForm.controls.passwordPolicyCode.setValue(this.policyStudent);
        }
        else this.userForm.controls.userName.setValue(emp.personalId || '');
      }
      this.user.email = emp.email || '';
      this.employeeName = emp.text || '';
    })
    this.userForm.controls.passwordPolicyCode.valueChanges.pipe(
      switchMap(() => this.policyChange),
      takeUntil(this.ngUnsubscribe)
    ).subscribe(policy => {
      if (this.user.lastChangePassword && policy.passwordAge) {
        this.user.passwordExpire = this.addDays(this.user.lastChangePassword, policy.passwordAge);
      }
      else this.user.passwordExpire = null;
    })
  }

  createProfileForm(profile: UserProfile) {
    this.profiles[profile.guid] = this.util.getActive(this.master.profiles, profile.profileCode);
    const fg = this.fb.group({
      profileCode: [null, Validators.required]
    });
    fg.valueChanges.pipe(takeUntil(this.ngUnsubscribe)).subscribe((control) => {
      if (profile.rowState == RowState.Normal) {
        profile.rowState = RowState.Edit;
      }
    })
    fg.patchValue(profile, { emitEvent: false });
    if (profile.id) fg.disable({ emitEvent: false });
    return fg;
  }

  addProfile() {
    const newProfile = new UserProfile();
    newProfile.form = this.createProfileForm(newProfile);
    if (this.user.id)
      newProfile.id = this.user.id;
    this.user.profiles.push(newProfile);
    this.user.profiles = [...this.user.profiles];
    this.userForm.markAsDirty();
  }

  removeProfile(row) {
    if (row.rowState !== RowState.Add) {
      row.rowState = RowState.Delete;
      this.profileDelete.push(row);
    }
    this.user.profiles = this.user.profiles.filter(item => item.guid !== row.guid);
    this.userForm.markAsDirty();
  }

  setUserInfo() {
    this.ss.userInfo = { username: this.user.userName, employeeName: this.employeeName };
  }

  addPermission() {
    if (this.userForm.dirty || !this.user.id) {
      this.ms.warning('message.STD00035');
      return;
    }
    this.setUserInfo();
    this.router.navigate(['su/surt06/permission'], { state: { id: this.user.id } });
  }

  removePermission(row) {
    if (row.rowState !== RowState.Add) {
      row.rowState = RowState.Delete;
      this.permisisonDelete.push(row);
    }
    this.user.permissions = this.user.permissions.filter(item => item.guid !== row.guid);
    this.userForm.markAsDirty();
  }


  private addDays(date: Date, days: number): Date {
    let clone = new Date(date.valueOf())
    clone.setDate(clone.getDate() + days);
    return clone;
  }

  rebuildForm() {
    this.userForm.markAsPristine();
    this.profileDelete = [];
    this.permisisonDelete = [];
    if (this.user.id) {
      this.policies = this.util.getActive(this.master.policies, this.user.passwordPolicyCode);
      this.userForm.patchValue(this.user);
      this.userForm.controls.userName.disable({ emitEvent: false });
      this.userForm.controls.employeeCode.disable({ emitEvent: false });
      this.user.password = "*****";
      this.user.profiles.map(profile => profile.form = this.createProfileForm(profile));
    }
    else {
      this.policies = this.util.getActive(this.master.policies, null);
      this.user.password = 'AUTO';
      this.user.lastChangePassword = new Date(new Date().toDateString());
    }
  }


  validate() {
    let invalid = false;

    if (this.userForm.invalid) {
      this.ms.warning('message.STD00032', ['label.SURT06.UserInfo']);
      invalid = true;
    }

    if (this.user.profiles.some(item => item.form.invalid)) {
      this.user.profiles.map(item => this.util.markFormGroupTouched(item.form));
      this.ms.warning('message.STD00032', ['label.SURT06.Profile']);
      invalid = true;
    }

    const seenProfile = new Set();
    const profileDuplicates = this.user.profiles.some(pro => {
      return pro.form.controls.profileCode.value != null && seenProfile.size === seenProfile.add(pro.form.controls.profileCode.value).size;
    });
    if (profileDuplicates) {
      this.ms.error('message.STD00004', ['label.SURT06.Profile']);
      invalid = true;
    }

    return invalid;
  }

  save() {
    this.util.markFormGroupTouched(this.userForm);

    if (this.validate()) return;

    this.saving = true;
    if (!this.user.rowVersion && this.ss.userType.code == this.userTypeParam.Student) {
      this.user.studentId = this.userForm.controls.employeeCode.value;
    }
    this.user.userType = this.ss.userType.code;
    this.ss.save(this.user, this.userForm.value, this.profileDelete, this.permisisonDelete).pipe(
      finalize(() => this.saving = false)
    ).subscribe((result: any) => {
      this.userForm.markAsPristine();
      this.ms.success("message.STD00006");
      if (result !== null && result.haveEmail === false) {
        this.ms.warning("message.SU00030", [this.employeeName]);
      }
      this.router.navigate(['su/surt06/detail'], { replaceUrl: true, state: { id: this.user.id || result.id } })
    })
  }



  resetPassword() {
    if (this.userForm.dirty || !this.user.id) {
      this.ms.warning('message.STD00035');
      return;
    }
    if (this.user && this.user.email) {
      this.ss.resetPassword(this.user.email, this.ss.userType.code == this.userTypeParam.Student).subscribe(() => this.ms.success('message.SU00032'))
    }
    else this.ms.warning("message.SU00030", [this.employeeName]);


  }
  canDeactivate(): Observable<boolean> {
    if (!this.userForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }
}
