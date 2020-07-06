import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormUtilService, ModalService, RowState, SubscriptionDisposer } from '@app/shared';
import { MessageService } from '@app/core';
import { Surt06Service, UserPermission, UserDivisions, UserEduLevel } from './surt06.service';
import { Observable, of, BehaviorSubject, Subject } from 'rxjs';
import { finalize, takeUntil, switchMap, map, filter } from 'rxjs/operators';
import { DivisionTreeComponent } from './division-tree/division-tree.component';

@Component({
  selector: 'app-surt06-permission',
  templateUrl: './surt06-permission.component.html',
  styleUrls: ['./surt06-permission.component.scss']
})
export class Surt06PermissionComponent extends SubscriptionDisposer implements OnInit {
  saving = false;
  master = { companies: [], typeLevels: [] }
  typeLevel = {};
  divisions = new BehaviorSubject<any>([]);
  permission: UserPermission;
  permissionForm: FormGroup;
  eduLevelDelete = [];
  id: number;
  username: string;
  employeeName: string;
  companyReady = new Subject<boolean>();
  @ViewChild(DivisionTreeComponent) divisionTree: DivisionTreeComponent;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    public ss: Surt06Service
  ) { super(); }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      Object.assign(this.master, data.surt06.master);
      this.permission = data.surt06.permission;
      this.id = data.surt06.id;
      this.username = this.ss.userInfo.username;
      this.employeeName = this.ss.userInfo.employeeName;
      this.master.companies = this.util.getActive(this.master.companies, this.permission.companyCode);
      this.rebuildForm();
    });
  }

  createForm() {
    this.permissionForm = this.fb.group({
      companyCode: [null, Validators.required]
    });

    this.permissionForm.controls.companyCode.valueChanges.pipe(takeUntil(this.ngUnsubscribe)).subscribe(comp => {
      if (comp) {
        this.ss.getMasterPermission('division', comp).pipe(
          map(dep => dep.divisions)
        ).subscribe(divs => {
          this.divisions.next(divs);
        })
        this.ss.getMasterPermission('eduLevel', comp).subscribe(dep => {
          this.master.typeLevels = dep.typeLevels;
          this.companyReady.next(true);
        })
      }
      else {
        this.divisions.next([]);
        this.companyReady.next(false);
      }
      if (!this.permission.rowVersion) this.permission.eduLevels = [];
    })
  }

  createEduLevelForm(edu: UserEduLevel) {
    this.typeLevel[edu.guid] = this.util.getActive(this.master.typeLevels, edu.educationTypeLevel);
    const fg = this.fb.group({
      educationTypeLevel: [null, Validators.required]
    });
    fg.valueChanges.pipe(takeUntil(this.ngUnsubscribe)).subscribe((control) => {
      if (edu.rowState == RowState.Normal) {
        edu.rowState = RowState.Edit;
      }
    })
    fg.patchValue(edu, { emitEvent: false });
    if (edu.educationTypeLevel) fg.disable({ emitEvent: false });
    return fg;
  }

  addTypeLevel() {
    const userEdu = new UserEduLevel();
    userEdu.companyCode = this.permissionForm.controls.companyCode.value;
    userEdu.form = this.createEduLevelForm(userEdu);
    this.permission.eduLevels.push(userEdu);
    this.permission.eduLevels = [...this.permission.eduLevels];
    this.permissionForm.markAsDirty();
  }

  removeTypeLevel(row) {
    if (row.rowState !== RowState.Add) {
      row.rowState = RowState.Delete;
      this.eduLevelDelete.push(row);
    }
    this.permission.eduLevels = this.permission.eduLevels.filter(item => item.guid !== row.guid);
    this.permissionForm.markAsDirty();
  }

  divisionChange() {
    this.permissionForm.markAsDirty();
  }

  rebuildForm() {
    this.eduLevelDelete = [];
    this.permissionForm.markAsPristine();
    if (this.permission.id) {
      this.permissionForm.patchValue(this.permission);
      this.permissionForm.controls.companyCode.disable({ emitEvent: false });
      this.companyReady.pipe(filter(ready => ready), takeUntil(this.ngUnsubscribe)).subscribe(() => this.permission.eduLevels.map(edu => edu.form = this.createEduLevelForm(edu)));
    }
    else {
      this.permission.id = this.id;
    }
    this.permissionForm.controls

  }

  validate() {
    let invalid = false;

    if (this.permissionForm.invalid) {
      invalid = true;
    }

    if (this.permission.eduLevels.some(item => item.form.invalid)) {
      this.permission.eduLevels.map(item => this.util.markFormGroupTouched(item.form));
      this.ms.warning('message.STD00032', ['label.SURT06.EducationPermission']);
      invalid = true;
    }

    const seenEduLevel = new Set();
    const eduDuplicates = this.permission.eduLevels.some(edu => {
      return edu.form.controls.educationTypeLevel.value != null && seenEduLevel.size === seenEduLevel.add(edu.form.controls.educationTypeLevel.value).size;
    });
    if (eduDuplicates) {
      this.ms.error('message.STD00004', ['label.SURT06.TypeLevel']);
      invalid = true;
    }

    return invalid;
  }

  save() {
    this.util.markFormGroupTouched(this.permissionForm);

    if (this.validate()) return;

    this.permission.divisions = this.divisionTree.selected.filter(select => select.rowState == RowState.Add);
    this.permission.divisions = this.permission.divisions.concat(this.divisionTree.removed);

    this.saving = true;
    this.ss.savePermission(this.permission, this.permissionForm.getRawValue(), this.eduLevelDelete).pipe(
      finalize(() => this.saving = false)
    ).subscribe(() => {
      this.permissionForm.markAsPristine();
      this.ms.success("message.STD00006");
      this.companyReady.next(false);
      this.router.navigate(['su/surt06/permission'], { replaceUrl: true, state: { id: this.permission.id, companyCode: this.permission.companyCode || this.permissionForm.controls.companyCode.value } })
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.permissionForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
