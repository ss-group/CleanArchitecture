import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { DbProject, Dbrt14Service } from './dbrt14.service';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt14-detail',
  templateUrl: './dbrt14-detail.component.html',
  styleUrls: ['./dbrt14-detail.component.scss']
})
export class Dbrt14DetailComponent implements OnInit {
  
  dbrt14Form: FormGroup;
  project:DbProject = {} as DbProject;
  submited:boolean=false;
  saving:boolean=false;
  master = {
    educationTypeCode: [],
    facCode: [],
    studentTypeCode: []
  };

  
 

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt14Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.project = data.dbrt14.detail;
      if(this.project.facCode != null && this.project.educationTypeCode != null && this.project.studentTypeCode != null){
        this.master.facCode = this.util.getActive(data.dbrt14.master.facCode,this.project.facCode);
        this.master.educationTypeCode = this.util.getActive(data.dbrt14.master.educationTypeCode,this.project.educationTypeCode);
        this.master.studentTypeCode = this.util.getActive(data.dbrt14.master.studentTypeCode,this.project.studentTypeCode);
      }else{
        this.master.facCode = this.util.getActive(data.dbrt14.master.facCode,null);
        this.master.educationTypeCode = this.util.getActive(data.dbrt14.master.educationTypeCode,null);
        this.master.studentTypeCode = this.util.getActive(data.dbrt14.master.studentTypeCode,null);
      }
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt14Form.markAsPristine();
    if (this.project.projectId) {
      this.dbrt14Form.patchValue(this.project);
      this.dbrt14Form.controls.projectCode.disable();
    }
  }

  createForm(){
    this.dbrt14Form = this.fb.group({
      companyCode: null,
      projectCode: [null, [Validators.required, Validators.maxLength(10)]],
      projectNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      projectNameEng: [null, [Validators.maxLength(100)]],
      projectDescription: [null, [Validators.maxLength(500)]],
      educationTypeCode: null,
      facCode: null,
      studentTypeCode: null,
      erpProduct: [null, [Validators.maxLength(20)]],
      erpCode: [null, [Validators.required,Validators.maxLength(20)]],
      erpActivity: [null, [Validators.maxLength(20)]],
      erpProject: [null, [Validators.maxLength(20)]],
      bypassRegister: false,
      active: true
    });
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt14Form);
    if (this.dbrt14Form.invalid) {
      return;
    }
    Object.assign(this.project, this.dbrt14Form.value);
    this.saving = true;
    this.cs.save(this.project).pipe(
      switchMap(() => this.cs.getProjectDetail(this.project.projectCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbProject) => {
      this.project = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }
  canDeactivate(): Observable<boolean> {
    if (!this.dbrt14Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
