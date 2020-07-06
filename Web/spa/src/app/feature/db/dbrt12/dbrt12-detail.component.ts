import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService, RowState } from '@app/shared';
import { MessageService } from '@app/core';

import { switchMap, finalize } from 'rxjs/operators';
import { Guid } from 'guid-typescript';
import { CsCurriculum, DbProgram, Dbrt12Service ,DbProgramDetail} from './dbrt12.service';

@Component({
  selector: 'app-dbrt12-detail',
  templateUrl: './dbrt12-detail.component.html',
  styleUrls: ['./dbrt12-detail.component.scss']
})
export class Dbrt12DetailComponent implements OnInit {
  
    RowState = RowState;
    dbrt12Form: FormGroup;
    groupForm:FormGroup;
    // programList: DbProgram = {} as DbProgram;
    submited:boolean=false;
    saving:boolean=false;
    master = { subjectGroupCode: [], courseDescriptionCode:[]};
    state: Observable<object>;
    detail: DbProgramDetail = {} as DbProgramDetail;

    disBtnSave: boolean;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt12Service
    
  ) {

  }
  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.detail = data.dbrt12.detail;
      if(this.detail.programCode != null){
        this.master.subjectGroupCode = this.util.getActive(data.dbrt12.master.subjectGroupCode,this.detail.subjectGroupCode);
        this.master.courseDescriptionCode = this.util.getActive(data.dbrt12.master.courseDescriptionCode,this.detail.courseDescriptionCode);
      }else{
        this.master.subjectGroupCode = this.util.getActive(data.dbrt12.master.subjectGroupCode,null);
        this.master.courseDescriptionCode = this.util.getActive(data.dbrt12.master.courseDescriptionCode,null);
      }
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt12Form.markAsPristine();
    if (this.detail.programCode) {
      this.dbrt12Form.patchValue(this.detail);  
      this.detail.csCurriculum.map(detail => detail.form = this.createMajorForm(detail))
      this.dbrt12Form.controls.programCode.disable();
    }
  }
  createForm(){
    this.dbrt12Form = this.fb.group({
      programCode:[null,[Validators.required, Validators.maxLength(5)]],
      programNameTha:[null,[Validators.required, Validators.maxLength(100)]],
      programNameEng:[null, [Validators.maxLength(100)]],
      programShortNameTha:[null,[Validators.required, Validators.maxLength(100)]],
      programShortNameEng:[null, [Validators.maxLength(100)]],
      formatCode:[null,[Validators.required, Validators.maxLength(10)]],
      fnPayinCode:[null,[Validators.required, Validators.maxLength(5)]],
      subjectGroupCode:[null,[Validators.required, Validators.maxLength(10)]],
      courseDescriptionCode:[null,[Validators.required, Validators.maxLength(10)]],
      cardName:[null,[Validators.maxLength(100)]],
      currId:[null,[Validators.maxLength(50)]],
      programId:[null,[Validators.maxLength(50)]],
      iscedId:[null,[Validators.maxLength(50)]],
      refGroupId:[null,[Validators.maxLength(50)]],
      programCodeMua:[null,[Validators.required,Validators.maxLength(10)]],
      branch:false,
      active:true
    });
  }

  createMajorForm(Curriculum: CsCurriculum) {
    const fg = this.fb.group({
      ohecCclCode: null
    });
    fg.patchValue(Curriculum);
    if (Curriculum.ohecCclCode) {
      fg.controls.ohecCclCode.disable({ emitEvent: false });
    }
    return fg;
}


save() {
  this.util.markFormGroupTouched(this.dbrt12Form);
  if (this.dbrt12Form.invalid) {
    return;
  }
  Object.assign(this.detail, this.dbrt12Form.value);
  this.saving = true;
  this.detail.csCurriculum = [];
  this.cs.save(this.detail).pipe(
    switchMap(() => this.cs.getProgramType(this.detail.programCode)),
    finalize(() => {
      this.saving = false;
    })
  ).subscribe((result: DbProgramDetail) => {
    this.dbrt12Form.markAsPristine();
    this.ms.success("message.STD00006");
    this.router.navigate(['db/dbrt12/detail'],{ replaceUrl:true, state : { code: this.detail.programCode }})
  })
}
canDeactivate(): Observable<boolean> {
  if (!this.dbrt12Form.dirty) {
    return of(true);
  }
  return this.modal.confirm("message.STD00002");
}
}
