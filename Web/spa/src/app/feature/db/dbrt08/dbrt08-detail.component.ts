import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DbEmployee, Dbrt08Service } from './dbrt08.service';
import { Page, FormUtilService, ModalService, idCardPattern } from '@app/shared';
import { BsModalRef } from 'ngx-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from '@app/core';
import { finalize, switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';


@Component({
  selector: 'app-dbrt08-detail',
  templateUrl: './dbrt08-detail.component.html',
  styleUrls: ['./dbrt08-detail.component.scss']
})
export class Dbrt08DetailComponent implements OnInit {
  page = new Page();
  templatePopup: BsModalRef;
  educ: FormGroup;
  EmployeeForm: FormGroup;
  employee: DbEmployee = {} as DbEmployee;
  state: Observable<object>;
  saving: boolean = false;
  master = {
    routelist: [],
    preNames: [],
    //tab personality
    genders: [],
    races: [],
    nationalitys: [],
    religions: [],
    addressProvinces: [],
    addressDistricts: [],
    addressSubDistricts: [],
    addressPostalCodes: [],
    //tab worker
    employeeTypes: [],
    divlists: [],
    positionLevels: [],
    positionlist: [],
    groupTypeCode: [],
    positionDivisions: [],
    jobTypes: [],
    divWork : [],
  };
  ageDesc = null;
  checkStudentIdCard = true;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private db: Dbrt08Service
  ) { }

  ngOnInit() {
    this.employeeForm();
    this.route.data.subscribe((data) => {
      this.employee = data.dbrt08.detail;
      this.master = data.dbrt08.master;
      if (this.employee.preNameId != null 
        && this.employee.gender != null
        && this.employee.raceCode != null
        && this.employee.nationCode != null
        && this.employee.religionCode != null
        && this.employee.groupTypeCode != null
        && this.employee.provinceId != null
        && this.employee.districtId != null
        && this.employee.subDistrictId != null
        && this.employee.postalCode != null
        ) {
        this.master.preNames = this.util.getActive(data.dbrt08.master.preNames, this.employee.preNameId);
        this.master.genders = this.util.getActive(data.dbrt08.master.genders, this.employee.gender);
        this.master.races = this.util.getActive(data.dbrt08.master.races, this.employee.raceCode);
        this.master.nationalitys = this.util.getActive(data.dbrt08.master.nationalitys, this.employee.nationCode);
        this.master.religions = this.util.getActive(data.dbrt08.master.religions, this.employee.religionCode);
        this.master.groupTypeCode = this.util.getActive(data.dbrt08.master.groupTypeCode, this.employee.groupTypeCode);
        this.master.addressProvinces = this.util.getActive(data.dbrt08.master.addressProvinces, this.employee.provinceId);
      } else {
        this.master.preNames = this.util.getActive(data.dbrt08.master.preNames, null);
        this.master.genders = this.util.getActive(data.dbrt08.master.genders, null);
        this.master.races = this.util.getActive(data.dbrt08.master.races, null);
        this.master.nationalitys = this.util.getActive(data.dbrt08.master.nationalitys, null);
        this.master.religions = this.util.getActive(data.dbrt08.master.religions, null);
        this.master.groupTypeCode = this.util.getActive(data.dbrt08.master.groupTypeCode, null);
        this.master.addressProvinces = this.util.getActive(data.dbrt08.master.addressProvinces, null);
      }
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.EmployeeForm.controls.groupTypeCode.setValue("999");
    this.EmployeeForm.markAsPristine();
    this.EmployeeForm.controls.groupTypeCode.disable();
    if (this.employee.employeeCode) {
      this.EmployeeForm.patchValue(this.employee)
      this.EmployeeForm.controls.employeeCode.disable();
    }
    if (this.employee.groupTypeCode != '999') {
      this.EmployeeForm.patchValue(this.employee);
      this.EmployeeForm.disable();
    }
    if (this.employee.groupTypeCode == null) {
      this.EmployeeForm.patchValue(this.employee);
      this.EmployeeForm.enable();
      this.EmployeeForm.controls.groupTypeCode.disable();
    }
    if (this.employee.provinceId) {
      this.EmployeeForm.patchValue(this.employee);
      if(this.employee.provinceId = null){
        this.changeProvinceAddress('edit');
      }
      if(this.employee.districtId = null){
        this.changeDistrictAddress('edit');
      }
      if(this.employee.subDistrictId = null){
        this.changeDistrictAddress('edit');
      }
    } else {
      this.EmployeeForm.controls.districtId.disable();
      this.EmployeeForm.controls.subDistrictId.disable();
      this.EmployeeForm.controls.postalCode.disable();
      this.changeProvinceAddress(null);
      this.changeDistrictAddress(null);
      this.changeSubDistrictAddress(null);
    }
  }

  employeeForm() {
    this.EmployeeForm = this.fb.group({
      employeeCode: [null, [Validators.required, Validators.maxLength(20)]],
      preNameId: [null, [Validators.required, Validators.maxLength(20)]],
      tFirstName: [null, [Validators.required, Validators.maxLength(200)]],
      tMiddleName: [null, [Validators.maxLength(200)]],
      tLastName: [null, [Validators.required, Validators.maxLength(200)]],
      eFirstName: [null, [Validators.maxLength(200)]],
      eMiddleName: [null, [Validators.maxLength(200)]],
      eLastName: [null, [Validators.maxLength(200)]],
      tNameConcat: [null, [Validators.maxLength(600)]],
      eNameConcat: [null, [Validators.maxLength(600)]],
      imageId: null,
      personalId: [null, idCardPattern()],
      birthday: null,
      gender: null,
      raceCode: null,
      nationCode: null,
      religionCode: null,
      addrName: [null, [Validators.maxLength(1000)]],
      moo: [null, [Validators.maxLength(100)]],
      soi: [null, [Validators.maxLength(100)]],
      street: [null, [Validators.maxLength(100)]],
      provinceId: null,
      districtId: null,
      subDistrictId: null,
      postalCode: null,
      telNo: [null, [Validators.maxLength(20),Validators.pattern('[0-9-#,]*')]],
      email: [null, [Validators.required,Validators.email, Validators.maxLength(100)]],
      workDate: [null, Validators.required],
      turnoverDate: null,
      empTypeCode: null,
      jobType: null,
      positionLevelCode: null,
      positionCode: null,
      positionDivision: null,
      divCode: null,
      groupTypeCode: null,
      divWorkId: null,
      teacherCode: null,

    });

    this.EmployeeForm.controls.birthday.valueChanges.subscribe(value => {
      if (value) {
        const dateNow: Date = new Date();
        const birthDate: Date = new Date(value);
        if (dateNow.getFullYear() > birthDate.getFullYear()) {
          this.ageDesc = dateNow.getFullYear() - birthDate.getFullYear() || '';
        }
      }
    });
  }


  save() {
    this.util.markFormGroupTouched(this.EmployeeForm);
    if (this.EmployeeForm.invalid) {
      return;
    }
    Object.assign(this.employee, this.EmployeeForm.getRawValue());
    this.saving = true;
    this.db.save(this.employee).pipe(
      switchMap(() => this.db.getEmployee(this.employee.employeeCode)),
      finalize(() => {
        this.saving = false;
      })
    ).subscribe((result: DbEmployee) => {
      this.employee = result;
      this.rebuildForm();
      this.ms.success("message.STD00006");
    })
  }

  changeProvinceAddress(event) {
    if (event && event == 'edit') {
      this.db.getDistrict(this.employee.provinceId).subscribe(res => {
        this.master.addressDistricts = res.district;
        this.EmployeeForm.controls.districtId.enable();
      });
    } else if (event && event != null) {
      this.db.getDistrict(event.value).subscribe(res => {
        this.master.addressDistricts = res.district;
        this.EmployeeForm.controls.districtId.setValue(null);
        this.EmployeeForm.controls.subDistrictId.setValue(null);
        this.EmployeeForm.controls.postalCode.setValue(null);
        this.EmployeeForm.controls.districtId.enable();
        this.EmployeeForm.controls.subDistrictId.disable();
        this.EmployeeForm.controls.postalCode.disable();
      });
    } else {
      this.master.addressDistricts = [];
      this.EmployeeForm.controls.districtId.setValue(null);
      this.EmployeeForm.controls.subDistrictId.setValue(null);
      this.EmployeeForm.controls.postalCode.setValue(null);
      this.EmployeeForm.controls.districtId.disable();
      this.EmployeeForm.controls.subDistrictId.disable();
      this.EmployeeForm.controls.postalCode.disable();
    }
  }
  changeDistrictAddress(event) {
    if (event && event == 'edit') {
      this.db.getSubDistrict(this.employee.districtId).subscribe(res => {
        this.master.addressSubDistricts = res.subDistrict;
        this.EmployeeForm.controls.subDistrictId.enable();
      });
    } else if (event && event != null) {
      this.db.getSubDistrict(event.value).subscribe(res => {
        this.master.addressSubDistricts = res.subDistrict;
        this.EmployeeForm.controls.subDistrictId.setValue(null);
        this.EmployeeForm.controls.postalCode.setValue(null);
        this.EmployeeForm.controls.subDistrictId.enable();
        this.EmployeeForm.controls.postalCode.disable();
      });
    } else {
      this.master.addressSubDistricts = [];
      this.EmployeeForm.controls.subDistrictId.setValue(null);
      this.EmployeeForm.controls.postalCode.setValue(null);
      this.EmployeeForm.controls.subDistrictId.disable();
      this.EmployeeForm.controls.postalCode.disable();
    }
  }
  changeSubDistrictAddress(event) {
    if (event && event == 'edit') {
      this.db.getPostalCode(this.employee.subDistrictId).subscribe(res => {
        this.master.addressPostalCodes = res.postalCode;
        this.EmployeeForm.controls.postalCode.enable();
      });
    } else if (event && event != null) {
      this.db.getPostalCode(event.value).subscribe(res => {
        this.master.addressPostalCodes = res.postalCode;
        this.EmployeeForm.controls.postalCode.setValue(null);
        this.EmployeeForm.controls.postalCode.enable();
      });
    } else {
      this.master.addressPostalCodes = [];
      this.EmployeeForm.controls.postalCode.setValue(null);
      this.EmployeeForm.controls.postalCode.disable();
    }
  }

  canDeactivate(): Observable<boolean> {
    if (!this.EmployeeForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
