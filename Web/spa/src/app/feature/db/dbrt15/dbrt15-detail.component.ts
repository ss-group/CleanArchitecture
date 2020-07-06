import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of} from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { DbDegreeSub, Dbrt15Service, DbDegreeSubEduGroup, DbDegreeSubDetail } from './dbrt15.service';
import { FormUtilService, ModalService, RowState } from '@app/shared';
import { MessageService } from '@app/core';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt15-detail',
  templateUrl: './dbrt15-detail.component.html',
  styleUrls: ['./dbrt15-detail.component.scss']
})
export class Dbrt15DetailComponent implements OnInit {

  dbrt15Form: FormGroup;
  groupForm: FormGroup;
  subdegree: DbDegreeSub = {} as DbDegreeSub;
  submited: boolean = false;
  saving: boolean = false;
  master = {
    degree: [],
    group: []
  };
  state: Observable<object>;
  detail: DbDegreeSubDetail = {} as DbDegreeSubDetail;
  dbDegreeSubEduGroupDelete: DbDegreeSubEduGroup[] = [];
  masterDegreeSub = [];
  DegreeSubs = [];
  disBtnSave: boolean;


  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt15Service
  ) {
  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.detail = data.dbrt15.detail;
      if (this.detail.degreeId != null) {
        this.master.degree = this.util.getActive(data.dbrt15.master.degree, this.detail.degreeId);
      } else {
        this.master.degree = this.util.getActive(data.dbrt15.master.degree, null);
      }
      this.masterDegreeSub = data.dbrt15.master.group;
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt15Form.markAsPristine();
    if (this.detail.subDegreeId) {
      this.dbrt15Form.patchValue(this.detail);
      this.detail.dbDegreeSubEduGroup.map(detail => detail.form = this.createSubEduGroupForm(detail))
      this.dbrt15Form.controls.degreeId.disable();
    }
  }

  createForm() {
    this.dbrt15Form = this.fb.group({
      subDegreeNameTha: [null, [Validators.required, Validators.maxLength(200)]],
      subDegreeNameEng: [null, [Validators.maxLength(200)]],
      subDegreeShortNameTha: [null, [Validators.required, Validators.maxLength(200)]],
      subDegreeShortNameEng: [null, [Validators.maxLength(200)]],
      degreeId: [null, Validators.required],
      active: true
    });

  }

  createSubEduGroupForm(DegreeSubEduGroup: DbDegreeSubEduGroup) {
    const fg = this.fb.group({
      groupLevel: [null, [Validators.required, Validators.maxLength(10)]],
      active: true,
      assigned: false
    });

    fg.valueChanges.subscribe((control) => {
      if (control.assigned && DegreeSubEduGroup.rowState == RowState.Normal) {
        DegreeSubEduGroup.rowState = RowState.Edit;
      }
    })

    fg.patchValue(DegreeSubEduGroup);
    if (DegreeSubEduGroup.groupLevel) {
      DegreeSubEduGroup.subDegreeDDL = this.util.getActive(this.masterDegreeSub, DegreeSubEduGroup.groupLevel);
      fg.controls.groupLevel.disable({ emitEvent: false });
    }
    fg.controls.assigned.setValue(true, { emitEvent: false });
    return fg;
  }

  add() {
    if (this.detail.subDegreeId != null) {
      const newGroup = new DbDegreeSubEduGroup();
      newGroup.subDegreeDDL = this.util.getActive(this.masterDegreeSub, null);
      newGroup.form = this.createSubEduGroupForm(newGroup);
      this.detail.dbDegreeSubEduGroup.push(newGroup);
      this.detail.dbDegreeSubEduGroup = [...this.detail.dbDegreeSubEduGroup];
      this.dbrt15Form.markAsDirty();
    } else {
      this.ms.warning("message.STD00035");
      return;
    }
  }

  remove(dbDegreeSubEduGroup: DbDegreeSubEduGroup) {
    if (dbDegreeSubEduGroup.rowState !== RowState.Add) {
      dbDegreeSubEduGroup.rowState = RowState.Delete;
      this.dbDegreeSubEduGroupDelete.push(dbDegreeSubEduGroup);
    }
    this.detail.dbDegreeSubEduGroup = this.detail.dbDegreeSubEduGroup.filter(item => item.guid !== dbDegreeSubEduGroup.guid);
    this.dbrt15Form.markAsDirty();
  }

  save() {
    if (this.validate()) return;
    this.util.markFormGroupTouched(this.dbrt15Form);
    if (this.dbrt15Form.invalid) {
      return;
    }
    if (this.detail.dbDegreeSubEduGroup != null && this.detail.dbDegreeSubEduGroup != []) {
      this.detail.dbDegreeSubEduGroup.map(item => this.util.markFormGroupTouched(item.form));
      if (this.detail.dbDegreeSubEduGroup.some(item => item.form.invalid)) {
        return;
      }
    }
    Object.assign(this.detail, this.dbrt15Form.value);
    this.saving = true;
    this.cs.save(this.detail, this.dbrt15Form.getRawValue(), this.dbDegreeSubEduGroupDelete).pipe(
      finalize(() => this.saving = false)
    ).subscribe((subDegreeId) => {
      this.dbrt15Form.markAsPristine();
      this.ms.success("message.STD00006");
      this.router.navigate(['db/dbrt15/detail'], { replaceUrl: true, state: { code: subDegreeId } })
    })
  }

  validate() {
    let invalid = false;
    if (this.dbrt15Form.invalid) {
      this.util.markFormGroupTouched(this.dbrt15Form);
      invalid = true;
    }
    if (this.detail.dbDegreeSubEduGroup.some(item => item.form.invalid)) {
      this.detail.dbDegreeSubEduGroup.map(item => this.util.markFormGroupTouched(item.form));
      invalid = true;
    }
    const seen = new Set();
    const detail = this.detail.dbDegreeSubEduGroup.some((item) => {
      return seen.size === seen.add(item.form.controls.groupLevel.value).size && item.form.controls.groupLevel.value;
    });
    if (detail) {
      this.ms.error("message.STD00004", ['label.DBRT15.groupLevel']);
      invalid = true;
    }
    return invalid;
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt15Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
