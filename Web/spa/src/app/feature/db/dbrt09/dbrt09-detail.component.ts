import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { DbEducationType, Dbrt09Service } from './dbrt09.service';
import { FormUtilService, ModalService } from '@app/shared';
import { switchMap, finalize } from 'rxjs/operators';
import { MessageService } from '@app/core';

@Component({
  selector: 'app-dbrt09-detail',
  templateUrl: './dbrt09-detail.component.html',
  styleUrls: ['./dbrt09-detail.component.scss']
})
export class Dbrt09DetailComponent implements OnInit {
  master = { level: [] };
  dbrt09Form:FormGroup;
  educationType: DbEducationType = {} as DbEducationType;
  saving:boolean=false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    public util: FormUtilService,
    private db: Dbrt09Service,
    private ms: MessageService,
    private modal: ModalService
) {
}

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.educationType = data.dbrt09.detail;
      if(this.educationType.educationTypeLevel != null){
        this.master.level = this.util.getActive(data.dbrt09.master.level,this.educationType.groupLevel);
      }else{
        this.master.level = this.util.getActive(data.dbrt09.master.level,null);
      }
      this.rebuildForm();
    });
  }
  
  rebuildForm() {
    this.dbrt09Form.markAsPristine();
    if (this.educationType.educationTypeLevel) {
      // console.log(this.educationType)
      this.dbrt09Form.patchValue(this.educationType);
      this.dbrt09Form.controls.educationTypeLevel.disable()
    }
}
  createForm() {
    this.dbrt09Form = this.fb.group({
      educationTypeLevel: [null, [Validators.required, Validators.maxLength(10)]],
      educationTypeLevelNameTha: [null, [Validators.required, Validators.maxLength(200)]],
      educationTypeLevelNameEng: [null, [Validators.maxLength(200)]],
      educationLevelShortNameTha: [null, [Validators.required, Validators.maxLength(200)]],
      educationLevelShortNameEng: [null, [Validators.maxLength(200)]],
      graduatedNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      graduatedNameEng: [null, [Validators.maxLength(100)]],
      groupLevel: [null, [Validators.required, Validators.maxLength(10)]],
      prefix: [null, [Validators.required, Validators.maxLength(10)]],
      formatCode: [null, [Validators.required, Validators.maxLength(10)]],
      educationTypeCodeMua: [null, [Validators.required, Validators.maxLength(10)]],
      active: true
    });
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt09Form);
    if (this.dbrt09Form.invalid) {
      return;
    }
    Object.assign(this.educationType, this.dbrt09Form.value);
    this.saving = true;
    this.db.save(this.educationType).pipe(
      switchMap(() => this.db.getEducationType(this.educationType.educationTypeLevel)),
      finalize(() => {
        
        this.saving = false;
      })
    ).subscribe((result: DbEducationType) => {
      this.educationType = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.dbrt09Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
