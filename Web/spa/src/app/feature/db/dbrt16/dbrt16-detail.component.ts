import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { DbLocation, Dbrt16Service } from './dbrt16.service';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt16-detail',
  templateUrl: './dbrt16-detail.component.html',
  styleUrls: ['./dbrt16-detail.component.scss']
})
export class Dbrt16DetailComponent implements OnInit {
  
  dbrt16Form: FormGroup;
  location:DbLocation = {} as DbLocation;
  submited:boolean=false;
  saving:boolean=false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt16Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.location = data.dbrt16.detail;
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.dbrt16Form.markAsPristine();
    if (this.location.locationCode) {
      this.dbrt16Form.patchValue(this.location);
      this.dbrt16Form.controls.locationCode.disable();
    }
  }

  createForm(){
    this.dbrt16Form = this.fb.group({
      companyCode: null,
      locationCode: [null, [Validators.required, Validators.maxLength(10)]],
      locationNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      locationNameEng: [null, [Validators.maxLength(100)]],
      active: true
    });
  }

  save() {
    this.util.markFormGroupTouched(this.dbrt16Form);
    if (this.dbrt16Form.invalid) {
      return;
    }
    Object.assign(this.location, this.dbrt16Form.value);
    this.saving = true;
    this.cs.save(this.location).pipe(
      switchMap(() => this.cs.getLocationDetail(this.location.locationCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbLocation) => {
      this.location = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }
  canDeactivate(): Observable<boolean> {
    if (!this.dbrt16Form.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
