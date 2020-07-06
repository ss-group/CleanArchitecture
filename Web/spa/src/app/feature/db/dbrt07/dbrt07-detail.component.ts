import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable, of } from 'rxjs';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { Dbrt07Service, DbPreNamae } from './dbrt07.service';
import { finalize, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt07-detail',
  templateUrl: './dbrt07-detail.component.html',
  styleUrls: ['./dbrt07-detail.component.scss']
})
export class Dbrt07DetailComponent implements OnInit {
    master = { preTypes: [] };
    dbrt07Form: FormGroup;
    prenameList: DbPreNamae = {} as DbPreNamae;
    saving: boolean = false;
    state: Observable<object>;
    paramListItemCode;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt07Service
) {
    this.createForm();
}

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.prenameList = data.dbrt07.detail;
      if (this.prenameList.preNameId != null) {
        this.master.preTypes = this.util.getActive(data.dbrt07.master.preTypes,this.prenameList.preNameType);
      }else{
        this.master.preTypes = this.util.getActive(data.dbrt07.master.preTypes,null);
      }
      this.rebuildForm();
  });
   
}

  rebuildForm() {
  this.dbrt07Form.markAsPristine();
  if (this.prenameList.preNameId) {
    this.dbrt07Form.patchValue(this.prenameList);
  }
}
  createForm() {
    this.dbrt07Form = this.fb.group({
        preNameTha: [null, [Validators.required, Validators.maxLength(100)]],
        preNameCodeMua: [null, [Validators.required, Validators.maxLength(10)]],
        preNameEng: [null, [Validators.maxLength(100)]],
        preNameShortTha: [null, [Validators.required, Validators.maxLength(100)]],
        preNameShortEng: [null, [Validators.maxLength(100)]],
        preNameType: '1',
        active: true
  });
}

save() {
  this.util.markFormGroupTouched(this.dbrt07Form);
  if (this.dbrt07Form.invalid) {
    return;
  }
  Object.assign(this.prenameList, this.dbrt07Form.value);
  this.saving = true;
  this.cs.save(this.prenameList).pipe(
    finalize(() => this.saving = false)
  ).subscribe((result) => {
    this.dbrt07Form.markAsPristine();
    this.ms.success("message.STD00006");
    this.router.navigate(['db/dbrt07/detail'],{ replaceUrl:true
      , state : { code: (result == null)?this.prenameList.preNameId:result }})
  })
}
canDeactivate(): Observable<boolean> {
  if (!this.dbrt07Form.dirty) {
    return of(true);
  }
  return this.modal.confirm("message.STD00002");
}
}



