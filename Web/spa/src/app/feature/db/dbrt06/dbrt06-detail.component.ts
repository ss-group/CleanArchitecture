import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { ActivatedRoute, Router} from '@angular/router';
import { Dbrt06Service, DbPostalCode } from './dbrt06.service';
import { FormUtilService, ModalService } from '@app/shared';
import { MessageService } from '@app/core';
import { finalize, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-dbrt06-detail',
  templateUrl: './dbrt06-detail.component.html',
  styleUrls: ['./dbrt06-detail.component.scss']
})
export class Dbrt06DetailComponent implements OnInit {
  postalForm: FormGroup;
  master = { province: [], district: [], subDistrict: [] };
  postalList: DbPostalCode = {} as DbPostalCode;
  saving: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private cs: Dbrt06Service
  ) {

  }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.postalList = data.dbrt06.detail;
      this.master = data.dbrt06.master;
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.postalForm.markAsPristine();
    if (this.postalList.postalCode) {
      this.postalForm.patchValue(this.postalList);
      this.postalForm.controls.postalCode.disable();
    }
    if (this.postalList.provinceId) {
      this.postalForm.patchValue(this.postalList);
      this.changeProvinceAddress('edit');
      this.changeDistrictAddress('edit');
    } else {
      this.postalForm.controls.districtId.disable();
      this.postalForm.controls.subDistrictId.disable();
      this.changeProvinceAddress(null);
      this.changeDistrictAddress(null);
    }
  }

  createForm() {
    this.postalForm = this.fb.group({
      postalCode: [null, [Validators.required, Validators.maxLength(5)]],
      districtId: [null, Validators.required,],
      provinceId: [null, Validators.required],
      subDistrictId: [null, Validators.required],
      postalNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      postalNameEng: [null, [Validators.maxLength(100)]],
      remark: [null, [Validators.maxLength(500)]],
      active: true
    })
  }


  save() {
    this.util.markFormGroupTouched(this.postalForm);
    if (this.postalForm.invalid) {
      return;
    }
    Object.assign(this.postalList, this.postalForm.value);
    this.saving = true;
    this.cs.save(this.postalList).pipe(
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((PostalCodeId) => {
      this.postalForm.markAsPristine();
      this.ms.success("message.STD00006");
      this.router.navigate(['db/dbrt06/detail'], { replaceUrl: true, state: { code: PostalCodeId } })
    })
  }

  changeProvinceAddress(event) {
    if (event && event == 'edit') {
      this.cs.getDistrict(this.postalList.provinceId).subscribe(res => {
        this.master.district = res.district;
        this.postalForm.controls.districtId.enable();
      });
    } else if (event && event != null) {
      this.cs.getDistrict(event.value).subscribe(res => {
        this.master.district = res.district;
        this.postalForm.controls.districtId.setValue(null);
        this.postalForm.controls.subDistrictId.setValue(null);
        this.postalForm.controls.districtId.enable();
        this.postalForm.controls.subDistrictId.disable();
      });
    } else {
      this.master.district = [];
      this.postalForm.controls.districtId.setValue(null);
      this.postalForm.controls.subDistrictId.setValue(null);
      this.postalForm.controls.districtId.disable();
      this.postalForm.controls.subDistrictId.disable();
    }
  }

  changeDistrictAddress(event) {
    if (event && event == 'edit') {
      this.cs.getSubDistrict(this.postalList.districtId).subscribe(res => {
        this.master.subDistrict = res.subDistrict;
        this.postalForm.controls.subDistrictId.enable();
      });
    } else if (event && event != null) {
      this.cs.getSubDistrict(event.value).subscribe(res => {
        this.master.subDistrict = res.subDistrict;
        this.postalForm.controls.subDistrictId.setValue(null);
        this.postalForm.controls.subDistrictId.enable();
      });
    } else {
      this.master.subDistrict = [];
      this.postalForm.controls.subDistrictId.setValue(null);
      this.postalForm.controls.subDistrictId.disable();
    }
  }

  canDeactivate(): Observable<boolean> {
    if (!this.postalForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }


}
