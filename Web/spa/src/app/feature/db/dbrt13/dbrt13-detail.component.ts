import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { Dbrt13Service, DbMajorDetail, DbProfessional } from './dbrt13.service';
import { FormUtilService, ModalService, RowState } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt13-detail',
  templateUrl: './dbrt13-detail.component.html',
  styleUrls: ['./dbrt13-detail.component.scss']
})
export class Dbrt13DetailComponent implements OnInit {

  dbrt13Form: FormGroup;
  submited:boolean=false;
  saving:boolean=false;
  master = { facCode: [], programCode: [] };
  detail: DbMajorDetail = {} as DbMajorDetail;
  professionalDelete: DbProfessional[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private sv: Dbrt13Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.detail = data.dbrt13.detail;
      if(this.detail.majorCode != null && this.detail.facCode != null && this.detail.programCode != null){
        this.master.facCode = this.util.getActive(data.dbrt13.master.facCode,this.detail.facCode);
        this.master.programCode = this.util.getActive(data.dbrt13.master.programCode,this.detail.programCode);
      }else{
        this.master.facCode = this.util.getActive(data.dbrt13.master.facCode,null);
        this.master.programCode = this.util.getActive(data.dbrt13.master.programCode,null);
      }
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt13Form.markAsPristine();
    if (this.detail.majorCode && this.detail.facCode && this.detail.programCode) {
      this.dbrt13Form.patchValue(this.detail);
      this.detail.dbProfessional.map(detail => detail.form = this.createDbProfessionalForm(detail));
      this.disableMajorForEdit();
    }
  }

  disableMajorForEdit(){
    this.dbrt13Form.controls.majorCode.disable();
    this.dbrt13Form.controls.facCode.disable();
    // this.dbrt13Form.controls.programCode.disable();
  }

  createForm() {
    this.dbrt13Form = this.fb.group({
      majorCode: [null, [Validators.required, Validators.maxLength(10)]],
      facCode: [null, [Validators.required, Validators.maxLength(2)]],
      programCode: [null, [Validators.required,Validators.maxLength(5)]],
      majorNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      majorNameEng: [null, [Validators.maxLength(100)]],
      majorShortNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      majorShortNameEng: [null, [ Validators.maxLength(100)]],
      formatCode: [null, [Validators.maxLength(10)]],
      majorCodeMua: [null, [Validators.required, Validators.maxLength(10)]],
      active: true
    });
  }

  createDbProfessionalForm(dbProfessional:DbProfessional) {
    const fg = this.fb.group({
      majorCode: [null, [Validators.required, Validators.maxLength(10)]],
      facCode: [null, [Validators.required, Validators.maxLength(2)]],
      programCode: [null, [Validators.required, Validators.maxLength(5)]],
      proCode: [null, [Validators.required, Validators.maxLength(10)]],
      proNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      proNameEng: [null, [Validators.maxLength(100)]],
      proShortNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      proShortNameEng: [null, [Validators.maxLength(100)]],
      formatCode: [null, [Validators.maxLength(10)]],
      active: true,
      assigned: false
    });

    fg.valueChanges.subscribe((control) => {
        if (control.assigned && dbProfessional.rowState == RowState.Normal) {
          dbProfessional.rowState = RowState.Edit;
        }
    })

    fg.patchValue(dbProfessional);
    if(dbProfessional.proCode){
      fg.controls.proCode.disable({ emitEvent: false });
    }
    fg.controls.assigned.setValue(true, { emitEvent: false });
    return fg;
  }

  facCodeChange(facCode){
    this.dbrt13Form.controls['programCode'].reset();
    this.sv.getMasterProgram(facCode).subscribe((result: any) => {
      // this.master.programCode = result.programCode;
      this.master.programCode = this.util.getActive(result.programCode,this.detail.programCode);
    });
  }

  add() {
    if (this.detail.facCode != null && this.detail.programCode != null && this.detail.majorCode != null) {
        const newGroup = new DbProfessional();
        newGroup.companyCode = this.detail.companyCode;
        newGroup.facCode = this.detail.facCode;
        newGroup.programCode = this.detail.programCode;
        newGroup.majorCode = this.detail.majorCode;
        newGroup.form = this.createDbProfessionalForm(newGroup);
        this.detail.dbProfessional.push(newGroup);
        this.detail.dbProfessional = [...this.detail.dbProfessional];
        this.dbrt13Form.markAsDirty();
    } else {
        this.ms.warning("message.STD00035");
        return;
    }
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt13Form);
    
    if (this.dbrt13Form.invalid) {
      return;
    }

    if(this.detail.dbProfessional != null && this.detail.dbProfessional != []){
      this.detail.dbProfessional.map(item => this.util.markFormGroupTouched(item.form));
      if (this.detail.dbProfessional.some(item => item.form.invalid)) {
        return;
      }
    }

    Object.assign(this.detail, this.dbrt13Form.value);
    this.saving = true;
    this.sv.save(this.detail, this.dbrt13Form.getRawValue(), this.professionalDelete).pipe(
      switchMap(() => this.sv.getProfessional(this.detail.majorCode, this.detail.facCode, this.detail.programCode)),
      finalize(() => { this.saving = false; })
    ).subscribe((result: DbMajorDetail) => {
      this.professionalDelete = [];
      this.dbrt13Form.markAsPristine();
      this.ms.success("message.STD00006");
      this.router.navigate(['db/dbrt13/detail'],{ replaceUrl:true
        , state : { majorCode: result.majorCode,  facCode: result.facCode, programCode: result.programCode }})
    })
  }

  remove(dbProfessional: DbProfessional) {
    if (dbProfessional.rowState !== RowState.Add) {
      dbProfessional.rowState = RowState.Delete;
        this.professionalDelete.push(dbProfessional);
    }
    this.detail.dbProfessional = this.detail.dbProfessional.filter(item => item.guid !== dbProfessional.guid);
    this.dbrt13Form.markAsDirty();
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt13Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
