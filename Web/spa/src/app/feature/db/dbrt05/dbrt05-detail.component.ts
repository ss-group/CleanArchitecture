import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { Dbrt05Service, DbSubDistrict } from './dbrt05.service';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt05-detail',
  templateUrl: './dbrt05-detail.component.html',
  styleUrls: ['./dbrt05-detail.component.scss']
})
export class Dbrt05DetailComponent implements OnInit {
  subDistrictForm: FormGroup;
  master = { province: [], district: [] };
  subDistrictList: DbSubDistrict = {} as DbSubDistrict;
  saving: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt05Service
  ) {
  }
  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.subDistrictList = data.dbrt05.detail;
      this.master = data.dbrt05.master;
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.subDistrictForm.markAsPristine();
    if (this.subDistrictList.subDistrictId) {
      this.subDistrictForm.patchValue(this.subDistrictList);
      this.subDistrictForm.controls.subDistrictCode.disable();
    }
    if (this.subDistrictList.provinceId) {
      this.subDistrictForm.patchValue(this.subDistrictForm);
      this.changeProvinceAddress('edit');
    } else {
      this.subDistrictForm.controls.districtId.disable();
      this.changeProvinceAddress(null);
    }
  }

  createForm() {
    this.subDistrictForm = this.fb.group({
      subDistrictCode: [null, [Validators.required, Validators.maxLength(10)]],
      subDistrictCodeMua: [null, [Validators.required, Validators.maxLength(10)]],
      districtId: [null, Validators.required],
      provinceId: [null, Validators.required],
      subDistrictNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      subDistrictNameEng: [null, [Validators.maxLength(100)]],
      active: true
    })
  }

  save() {
    this.util.markFormGroupTouched(this.subDistrictForm);
    if (this.subDistrictForm.invalid) {
      return;
    }
    Object.assign(this.subDistrictList, this.subDistrictForm.value);
    this.saving = true;
    this.cs.save(this.subDistrictList).pipe(
      switchMap(() => this.cs.getSubDistrictType(this.subDistrictList.subDistrictCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbSubDistrict) => {
      this.subDistrictList = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  changeProvinceAddress(event) {
    if (event && event == 'edit') {
      this.cs.getDistrict(this.subDistrictList.provinceId).subscribe(res => {
        this.master.district = res.district;
        this.subDistrictForm.controls.districtId.enable();
      });
    } else if (event && event != null) {
      this.cs.getDistrict(event.value).subscribe(res => {
        this.master.district = res.district;
        this.subDistrictForm.controls.districtId.setValue(null);
        this.subDistrictForm.controls.districtId.enable();
      });
    } else {
      this.master.district = [];
      this.subDistrictForm.controls.districtId.setValue(null);
      this.subDistrictForm.controls.districtId.disable();
    }
  }

  canDeactivate(): Observable<boolean> {
    if (!this.subDistrictForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }


}
