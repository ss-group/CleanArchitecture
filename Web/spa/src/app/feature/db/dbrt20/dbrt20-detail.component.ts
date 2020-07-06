import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';
import { DbDepartment, Dbrt20Service } from './dbrt20.service';

@Component({
  selector: 'app-dbrt20-detail',
  templateUrl: './dbrt20-detail.component.html',
  styleUrls: ['./dbrt20-detail.component.scss']
})
export class Dbrt20DetailComponent implements OnInit {

  dbrt20Form: FormGroup;
  department: DbDepartment = {} as DbDepartment;
  submited: boolean = false;
  saving: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt20Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.department = data.dbrt20.detail;
      this.rebuildForm();
    });

  }

  rebuildForm() {
    this.dbrt20Form.markAsPristine();
    if (this.department.departmentCode) {
      this.dbrt20Form.patchValue(this.department);
      this.dbrt20Form.controls.departmentCode.disable();
    }
  }

  createForm() {
    this.dbrt20Form = this.fb.group({
      departmentCode: [null, [Validators.required,Validators.maxLength(10)]],
      departmentCodeMua: [null, [Validators.required,Validators.maxLength(10)]],
      departmentNameTha: [null, [Validators.required,Validators.maxLength(100)]],
      departmentNameEng: [null, [Validators.maxLength(100)]],
      departmentShortNameTha: [null, [Validators.required,Validators.maxLength(100)]],
      departmentShortNameEng: [null, [Validators.maxLength(100)]],
      active: true
    });
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt20Form);
    if (this.dbrt20Form.invalid) {
      return;
    }
    Object.assign(this.department, this.dbrt20Form.value);
    this.saving = true;
    this.cs.save(this.department).pipe(
      switchMap(() => this.cs.getDepartmentDetail(this.department.departmentCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbDepartment) => {
      this.department = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt20Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }
}
