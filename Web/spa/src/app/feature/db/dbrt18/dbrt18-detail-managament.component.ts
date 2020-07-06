import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { DbListItem, Dbrt18Service } from './dbrt18.service';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt18-detail-managament',
  templateUrl: './dbrt18-detail-managament.component.html',
  styleUrls: ['./dbrt18-detail-managament.component.scss']
})
export class Dbrt18DetailManagamentComponent implements OnInit {

  listItemForm:FormGroup;
  groupcode: any = '';
  saving:boolean=false;
  itemList: DbListItem = {} as DbListItem

  constructor(
    private router: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt18Service
  ) {
  }

  ngOnInit() {
    this.createForm();
    this.router.data.subscribe((data) => {
      this.itemList = data.dbrt18.manage;
      this.groupcode = data.dbrt18.detail2.listItemGroupCode;
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.listItemForm.markAsPristine();
    this.listItemForm.controls.listItemGroupCode.disable();
    if (this.itemList.listItemGroupCode) {
      this.listItemForm.patchValue(this.itemList);
      this.listItemForm.controls.listItemCode.disable();
  }
  else{
    this.listItemForm.patchValue({
      listItemGroupCode: this.groupcode
    })
  }

}

  createForm(){
    this.listItemForm = this.fb.group({
      listItemGroupCode: [null, [Validators.required, Validators.maxLength(20)]],
      listItemCode: [null, [Validators.required, Validators.maxLength(20)]],
      listItemCodeMua:[null, [Validators.maxLength(20)]],
      sequence: [null, Validators.required],
      listItemNameTha: [null, [Validators.required, Validators.maxLength(500)]],
      listItemNameEng: [null, [Validators.maxLength(500)]],
      listItemShortNameTha: [null, [Validators.maxLength(500)]],
      listItemShortNameEng: [null, [Validators.maxLength(500)]],
      attibute1: [null, [Validators.maxLength(200)]],
      attibute2: [null, [Validators.maxLength(200)]],
      attibute3: [null, [Validators.maxLength(200)]],
      attibute4: [null, [Validators.maxLength(200)]],
      attibute5: [null, [Validators.maxLength(200)]],
      remark: [null, [Validators.maxLength(200)]],
      active:true
    });
  }
  save() {
    this.util.markFormGroupTouched(this.listItemForm);
    if (this.listItemForm.invalid) {
      return;
    }
    Object.assign(this.itemList, this.listItemForm.getRawValue());
    this.saving = true;
    this.cs.save(this.itemList).pipe(
      switchMap(() => this.cs.getListType(this.itemList.listItemGroupCode ,this.itemList.listItemCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbListItem) => {
      this.itemList = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }
  canDeactivate(): Observable<boolean> {
    if (!this.listItemForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }
}
