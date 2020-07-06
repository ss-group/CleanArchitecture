import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SuCompany, Surt01Service } from './surt01.service';
import { ActivatedRoute } from '@angular/router';
import { FormUtilService, ModalService, Pattern } from '@app/shared';
import { MessageService } from '@app/core';
import { switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-surt01-detail',
  templateUrl: './surt01-detail.component.html'
})
export class Surt01DetailComponent implements OnInit {

  companyForm: FormGroup;
  company: SuCompany = {} as SuCompany;
  saving: boolean = false;
  master = { personalTaxTypes: [], countries: [], provinces: [], districts: [], subDistricts: [], postalCodes: [] };
  countries = [];

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private su: Surt01Service
  ) { }

  ngOnInit() {
    this.createForm();
    this.route.data.subscribe((data) => {
      this.company = data.surt01.detail;
      this.master = data.surt01.master;
      this.rebuildForm();
    });
  }

  createForm() {
    this.companyForm = this.fb.group({
      companyCode: [null, [Validators.required, Validators.maxLength(5), Validators.pattern(Pattern.UpperOnly)]],
      active: true,
      companyNameTha: [null, [Validators.required, Validators.maxLength(100)]],
      companyNameEng: [null, Validators.maxLength(100)],
      personalTaxType: null,
      taxId: [null, Validators.maxLength(20)],
      receiptBranchCode: [null, Validators.maxLength(20)],
      receiptBranchName: [null, Validators.maxLength(100)],
      socailSecurityNo: [null, Validators.maxLength(20)],
      socailSecurityBranch: [null, Validators.maxLength(100)],
      addressTha: [null, Validators.maxLength(100)],
      addressEng: [null, Validators.maxLength(100)],
      moo: [null, Validators.maxLength(100)],
      soi: [null, Validators.maxLength(100)],
      road: [null, Validators.maxLength(100)],
      countryId: null,
      provinceId: null,
      districtId: null,
      subDistrictId: null,
      postalCodeId: null,
      telephoneNo: [null, [Validators.maxLength(20), Validators.pattern('[0-9-#,]*')]],
      faxNo: [null, [Validators.maxLength(20), Validators.pattern('[0-9-#,]*')]],
      email: [null, [Validators.maxLength(100), Validators.email]]
    });

    this.companyForm.controls.countryId.valueChanges.subscribe(countryId => {
      this.master.provinces = [];
      this.companyForm.controls.provinceId.setValue(null);
      if (countryId) {
        this.su.getMaster({ FormName: 'province', CountryId: countryId }).subscribe(master => {
          if (this.company.companyCode)
            this.master.provinces = this.util.getActive(master.provinces, this.company.provinceId);
          else
            this.master.provinces = this.util.getActive(master.provinces, null);
        });
      }
    });

    this.companyForm.controls.provinceId.valueChanges.subscribe(provinceId => {
      this.master.districts = [];
      this.companyForm.controls.districtId.setValue(null);
      if (this.companyForm.controls.countryId.value && provinceId) {
        this.su.getMaster({ FormName: 'district', ProvinceId: provinceId }).subscribe(master => {
          if (this.company.companyCode)
            this.master.districts = this.util.getActive(master.districts, this.company.districtId);
          else
            this.master.districts = this.util.getActive(master.districts, null);
        });
      }
    });

    this.companyForm.controls.districtId.valueChanges.subscribe(districtId => {
      this.master.subDistricts = [];
      this.companyForm.controls.subDistrictId.setValue(null);
      if (this.companyForm.controls.provinceId.value && districtId) {
        this.su.getMaster({ FormName: 'subDistrict', DistrictId: districtId }).subscribe(master => {
          if (this.company.companyCode)
            this.master.subDistricts = this.util.getActive(master.subDistricts, this.company.subDistrictId);
          else
            this.master.subDistricts = this.util.getActive(master.subDistricts, null);
        });
      }
    });

    this.companyForm.controls.subDistrictId.valueChanges.subscribe(subDistrictId => {
      this.master.postalCodes = [];
      this.companyForm.controls.postalCodeId.setValue(null);
      if (this.companyForm.controls.districtId.value && subDistrictId) {
        this.su.getMaster(
          {
            FormName: 'postalCode',
            ProvinceId: this.companyForm.controls.provinceId.value,
            DistrictId: this.companyForm.controls.districtId.value,
            SubDistrictId: subDistrictId
          }
        ).subscribe(master => {
          if (this.company.companyCode)
            this.master.postalCodes = this.util.getActive(master.postalCodes, this.company.postalCodeId);
          else
            this.master.postalCodes = this.util.getActive(master.postalCodes, null);
        });
      }
    });
  }

  rebuildForm() {
    this.companyForm.markAsPristine();
    if (this.company.companyCode) {
      this.companyForm.patchValue(this.company);
      this.companyForm.controls.companyCode.disable();
      this.countries = this.util.getActive(this.master.countries, this.company.countryId);
    } else {
      this.countries = this.util.getActive(this.master.countries, null);
    }
  }

  save() {
    this.util.markFormGroupTouched(this.companyForm);
    if (this.companyForm.invalid) {
      return;
    }
    Object.assign(this.company, this.companyForm.value);
    this.su.saveCompany(this.company).pipe(
      switchMap(() => this.su.getCompany(this.company.companyCode))
    ).subscribe((result: SuCompany) => {
      this.company = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.companyForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
