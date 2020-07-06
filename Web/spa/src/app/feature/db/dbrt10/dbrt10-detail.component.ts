import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { DbEducationTypes, Dbrt10Service } from './dbrt10.service';
import { FormUtilService, ModalService } from '@app/shared';
import { switchMap, finalize } from 'rxjs/operators';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt10-detail',
  templateUrl: './dbrt10-detail.component.html',
  styleUrls: ['./dbrt10-detail.component.scss']
})
export class Dbrt10DetailComponent implements OnInit {
  master = { level: [] };
  dbrt10Form: FormGroup;
  educationType: DbEducationTypes = {} as DbEducationTypes;
  submited: boolean = false;
  saving: boolean = false;

  // state: Observable<object>;
  // paramListItemCode;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    public util: FormUtilService,
    private modal: ModalService,
    private db: Dbrt10Service,
    private ms: MessageService,
  ) {
  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.educationType = data.dbrt10.detail;
      if(this.educationType.educationTypeCode != null){
        this.master.level = this.util.getActive(data.dbrt10.master.level,this.educationType.typeLevel);
      }else{
        this.master.level = this.util.getActive(data.dbrt10.master.level,null);
      }
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt10Form.markAsPristine();
    if (this.educationType.educationTypeCode) {
      this.dbrt10Form.patchValue(this.educationType);
      //this.dbrt10Form.controls.typeLevel.disable();
      this.dbrt10Form.controls.educationTypeCode.disable();
  }
}

  createForm() {
    this.dbrt10Form = this.fb.group({
      educationTypeCode: [null, [Validators.required, Validators.maxLength(10)]],
      educationTypeCodeMua: [null, [Validators.required, Validators.maxLength(10)]],
      educationTypeNameTha: [null, [Validators.required, Validators.maxLength(200)]],
      educationTypeNameEng: [null, [Validators.maxLength(200)]],
      educationShortNameTha: [null, [Validators.required, Validators.maxLength(200)]],
      educationShortNameEng: [null, [Validators.maxLength(200)]],
      typeLevel: [null, [Validators.required, Validators.maxLength(10)]],
      formatCode: [null, [Validators.required, Validators.maxLength(10)]],
      summaryYear: [null, [Validators.required, Validators.maxLength(10)]],
      newLevelCode: [null, [Validators.maxLength(10)]],
      active: true
    });
  }

  save(){
    this.util.markFormGroupTouched(this.dbrt10Form);
    if (this.dbrt10Form.invalid) {
      return;
    }
    Object.assign(this.educationType, this.dbrt10Form.value);
    this.saving = true;
    this.db.save(this.educationType).pipe(
      switchMap(() => this.db.getEducationTypie( this.educationType.educationTypeCode, this.educationType.typeLevel)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbEducationTypes) => {
      this.educationType = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  onProgramCodeChange(value) {
    if(value) {
      this.dbrt10Form.get('typeLevel').setValue(value);
    } else {
      this.dbrt10Form.get('typeLevel').setValue(null);
    }
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt10Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }
}
