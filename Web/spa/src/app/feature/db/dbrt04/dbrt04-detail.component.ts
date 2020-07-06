import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { Dbrt04Service, DbDistrict } from './dbrt04.service';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt04-detail',
  templateUrl: './dbrt04-detail.component.html',
  styleUrls: ['./dbrt04-detail.component.scss']
})
export class Dbrt04DetailComponent implements OnInit {
  districtForm: FormGroup;
  master = { province: [] };
  districtList: DbDistrict = {} as DbDistrict;
  saving: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt04Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.districtList = data.dbrt04.detail;
      if (this.districtList.provinceId != null) {
        this.master.province = this.util.getActive(data.dbrt04.master.province, this.districtList.provinceId);
      } else {
        this.master.province = this.util.getActive(data.dbrt04.master.province, null);
      }
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.districtForm.markAsPristine();
    if (this.districtList.provinceId) {
      this.districtForm.patchValue(this.districtList);
      this.districtForm.controls.districtCode.disable();
    }
  }

  createForm() {
    this.districtForm = this.fb.group({
      provinceId: [null, Validators.required],
      districtCode: [null, [Validators.required, Validators.maxLength(10)]],
      districtCodeMua: [null, [Validators.required, Validators.maxLength(10)]],
      districtNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      districtNameEng: [null, [Validators.maxLength(100)]],
      active: true
    });
  }

  save() {
    this.util.markFormGroupTouched(this.districtForm);
    if (this.districtForm.invalid) {
      return;
    }
    Object.assign(this.districtList, this.districtForm.value);
    this.saving = true;
    this.cs.save(this.districtList).pipe(
      switchMap(() => this.cs.getDistrictType(this.districtList.districtCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbDistrict) => {
      this.districtList = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }
  canDeactivate(): Observable<boolean> {
    if (!this.districtForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }
}
