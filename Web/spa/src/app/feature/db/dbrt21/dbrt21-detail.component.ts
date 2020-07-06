import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { Dbrt21Service, DbFacProgram, DbFacProgramDetail } from './dbrt21.service';
import { FormUtilService, ModalService, RowState } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt21-detail',
  templateUrl: './dbrt21-detail.component.html',
  styleUrls: ['./dbrt21-detail.component.scss']
})
export class Dbrt21DetailComponent implements OnInit {

  dbrt21Form: FormGroup;
  submited: boolean = false;
  saving: boolean = false;
  master = { facCode: [], programCode: [], departmentCode: [] };
  masterDepartment = [];
  detail: DbFacProgram = {} as DbFacProgram;
  dbFacProgramDetailDelete: DbFacProgramDetail[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private sv: Dbrt21Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.detail = data.dbrt21.detail;
      if (this.detail.facCode != null && this.detail.programCode != null) {
        this.master.facCode = this.util.getActive(data.dbrt21.master.facCode, this.detail.facCode);
        this.master.programCode = data.dbrt21.master.programCode;
      } else {
        this.master.facCode = this.util.getActive(data.dbrt21.master.facCode, null);
        this.master.programCode = data.dbrt21.master.programCode;
      }
      this.masterDepartment = data.dbrt21.master.departmentCode;
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt21Form.markAsPristine();
    if (this.detail.facCode && this.detail.programCode) {
      this.dbrt21Form.patchValue(this.detail);
      this.detail.dbFacProgramDetail.map(detail => detail.form = this.createDbFacProgramDetailForm(detail));
      this.disableFacProgramForEdit();
    }
  }

  disableFacProgramForEdit() {
    this.dbrt21Form.controls.facCode.disable();
    this.dbrt21Form.controls.programCode.disable();
  }

  createForm() {
    this.dbrt21Form = this.fb.group({
      facCode: [null, [Validators.required, Validators.maxLength(2)]],
      programCode: [null, [Validators.required, Validators.maxLength(5)]],
      refGroupId: null,
      currId: null,
      programId: null,
      iscedId: null,
      programCodeMua: null,
      active: true
    });

    this.dbrt21Form.controls.programCode.valueChanges.subscribe(
      program => {
        console.log(program);
        this.dbrt21Form.controls.refGroupId.setValue(null);
        this.dbrt21Form.controls.currId.setValue(null);
        this.dbrt21Form.controls.programId.setValue(null);
        this.dbrt21Form.controls.iscedId.setValue(null);
        this.dbrt21Form.controls.programCodeMua.setValue(null);
        if (program) {
          const result = this.master.programCode.find((i) => i.value === program);
          if (result) {
            this.dbrt21Form.controls.refGroupId.setValue(result.refGroupId);
            this.dbrt21Form.controls.currId.setValue(result.currId);
            this.dbrt21Form.controls.programId.setValue(result.programId);
            this.dbrt21Form.controls.iscedId.setValue(result.iscedId);
            this.dbrt21Form.controls.programCodeMua.setValue(result.programCodeMua);
          }
          console.log(program);
        }
      }
    );
  }

  createDbFacProgramDetailForm(facProgramDetail: DbFacProgramDetail) {
    const fg = this.fb.group({
      facCode: [null, [Validators.required, Validators.maxLength(2)]],
      programCode: [null, [Validators.required, Validators.maxLength(5)]],
      departmentCode: [null, [Validators.required, Validators.maxLength(10)]],
      // departmentDDL: [{active:true,text:'test',value:1}],
      active: true,
      assigned: false
    });

    fg.valueChanges.subscribe((control) => {
      if (control.assigned && facProgramDetail.rowState == RowState.Normal) {
        facProgramDetail.rowState = RowState.Edit;
      }
    })

    fg.patchValue(facProgramDetail);
    if (facProgramDetail.departmentCode) {
      facProgramDetail.departmentDDL = this.util.getActive(this.masterDepartment, facProgramDetail.departmentCode);
      fg.controls.departmentCode.disable({ emitEvent: false });
    }
    fg.controls.assigned.setValue(true, { emitEvent: false });
    return fg;
  }

  add() {
    if (this.detail.facCode != null && this.detail.programCode != null) {
      const newGroup = new DbFacProgramDetail();
      newGroup.companyCode = this.detail.companyCode;
      newGroup.facCode = this.detail.facCode;
      newGroup.programCode = this.detail.programCode;
      newGroup.departmentDDL = this.util.getActive(this.masterDepartment, null);
      newGroup.form = this.createDbFacProgramDetailForm(newGroup);
      this.detail.dbFacProgramDetail.push(newGroup);
      this.detail.dbFacProgramDetail = [...this.detail.dbFacProgramDetail];
      this.dbrt21Form.markAsDirty();
    } else {
      this.ms.warning("message.STD00035");
      return;
    }
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt21Form);

    if (this.dbrt21Form.invalid) {
      return;
    }

    if (this.detail.dbFacProgramDetail != null && this.detail.dbFacProgramDetail != []) {
      this.detail.dbFacProgramDetail.map(item => this.util.markFormGroupTouched(item.form));
      if (this.detail.dbFacProgramDetail.some(item => item.form.invalid)) {
        return;
      }
    }

    Object.assign(this.detail, this.dbrt21Form.value);
    this.saving = true;
    this.sv.save(this.detail, this.dbrt21Form.getRawValue(), this.dbFacProgramDetailDelete).pipe(
      switchMap(() => this.sv.getFacProgramDetail(this.detail.facCode, this.detail.programCode)),
      finalize(() => { this.saving = false; })
    ).subscribe((result: DbFacProgram) => {
      this.dbFacProgramDetailDelete = [];
      this.dbrt21Form.markAsPristine();
      this.ms.success("message.STD00006");
      this.router.navigate(['db/dbrt21/detail'], {
        replaceUrl: true
        , state: { facCode: result.facCode, programCode: result.programCode }
      })
    })
  }

  remove(dbFacProgramDetail: DbFacProgramDetail) {
    if (dbFacProgramDetail.rowState !== RowState.Add) {
      dbFacProgramDetail.rowState = RowState.Delete;
      this.dbFacProgramDetailDelete.push(dbFacProgramDetail);
    }
    this.detail.dbFacProgramDetail = this.detail.dbFacProgramDetail.filter(item => item.guid !== dbFacProgramDetail.guid);
    this.dbrt21Form.markAsDirty();
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt21Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
