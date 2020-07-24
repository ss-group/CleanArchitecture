import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Employee, Dbrt08Service } from './dbrt08.service';
import { Page, FormUtilService, ModalService, idCardPattern } from '@app/shared';
import { BsModalRef } from 'ngx-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from '@app/core';
import { finalize, switchMap } from 'rxjs/operators';
import { Observable, of, Subject } from 'rxjs';
import * as moment from 'moment';

@Component({
  selector: 'app-dbrt08-detail',
  templateUrl: './dbrt08-detail.component.html',
  styleUrls: ['./dbrt08-detail.component.scss']
})
export class Dbrt08DetailComponent implements OnInit {
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
    divWork: [],
  };

  employeeForm: FormGroup;
  employee: Employee;
  preNameChange = new Subject<any>();
  saving = false;
 
  currentDate = moment().add(-20, 'years').toDate();
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    public util: FormUtilService,
    private ms: MessageService,
    private modal: ModalService,
    private ds: Dbrt08Service
  ) { }

  ngOnInit() {
    this.createEmployeeForm();
    this.route.data.subscribe((data) => {
      this.employee = data.dbrt08.detail;
      this.master = data.dbrt08.master;
      this.master.preNames = this.util.getActive(data.dbrt08.master.preNames, this.employee.preNameId);
      this.master.genders = this.util.getActive(data.dbrt08.master.genders, this.employee.gender);
      this.master.races = this.util.getActive(data.dbrt08.master.races, this.employee.raceCode);
      this.master.nationalitys = this.util.getActive(data.dbrt08.master.nationalitys, this.employee.nationCode);
      this.master.religions = this.util.getActive(data.dbrt08.master.religions, this.employee.religionCode);
      //this.master.groupTypeCode = this.util.getActive(data.dbrt08.master.groupTypeCode, this.employee.groupTypeCode);
      this.master.addressProvinces = this.util.getActive(data.dbrt08.master.addressProvinces, this.employee.provinceId);
      this.rebuildForm();
    });
  }

  rebuildForm() {
    this.employeeForm.markAsPristine();
    if (this.employee.employeeCode) {
      this.employeeForm.patchValue(this.employee)
      this.employeeForm.controls.employeeCode.disable();
    }
  }

  createEmployeeForm() {
    this.employeeForm = this.fb.group({
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
      telNo: [null, [Validators.maxLength(20), Validators.pattern('[0-9-#,]*')]],
      email: [null, [Validators.required, Validators.email, Validators.maxLength(100)]],
      teacherCode: null
    });

    this.employeeForm.controls.preNameId.valueChanges.pipe(
      switchMap(()=>this.preNameChange)
    ).subscribe(result=>{
      if(!this.employeeForm.controls.preNameId.pristine){
        if(result.genderCode){
          this.employeeForm.controls.gender.setValue(result.genderCode);
        }
        else this.employeeForm.controls.gender.setValue(null);
      }
    })

    this.employeeForm.controls.birthday.valueChanges.subscribe(value => {
        let age = moment().diff(moment(value),'years');
        this.employee.ageDesc = age || null;
    }); 

    this.employeeForm.controls.provinceId.valueChanges.subscribe(value => {
      this.master.addressDistricts = [];
      this.employeeForm.controls.districtId.disable({ emitEvent: false });
      if (!this.employeeForm.controls.provinceId.pristine) this.employeeForm.controls.districtId.setValue(null);
      if (value) {
        this.employeeForm.controls.districtId.enable({ emitEvent: false });
        this.ds.getDependencyMaster('district', { provinceId: value }).subscribe(dependency => {
          this.master.addressDistricts =this.util.getActive(dependency.districts,this.employee.districtId);
        })
      }
    });

    this.employeeForm.controls.districtId.valueChanges.subscribe(value => {
      this.master.addressSubDistricts = [];
      this.employeeForm.controls.subDistrictId.disable({ emitEvent: false });
      if (!this.employeeForm.controls.districtId.pristine) this.employeeForm.controls.subDistrictId.setValue(null);
      if (value) {
        this.employeeForm.controls.subDistrictId.enable({ emitEvent: false });
        this.ds.getDependencyMaster('subDistrict', { districtId: value }).subscribe(dependency => {
          this.master.addressSubDistricts = dependency.subDistricts;
        })
      }
    })

    this.employeeForm.controls.subDistrictId.valueChanges.subscribe(value => {
      this.master.addressPostalCodes = [];
      this.employeeForm.controls.postalCode.disable({ emitEvent: false });
      if (!this.employeeForm.controls.subDistrictId.pristine) this.employeeForm.controls.postalCode.setValue(null);
      if (value) {
        this.employeeForm.controls.postalCode.enable({ emitEvent: false });
        this.ds.getDependencyMaster('postalCode', { subDistrictId: value }).subscribe(dependency => {
          this.master.addressPostalCodes = dependency.postalCodes;
        })
      }
    })
    this.employeeForm.controls.districtId.disable({ emitEvent: false });
    this.employeeForm.controls.subDistrictId.disable({ emitEvent: false });
    this.employeeForm.controls.postalCode.disable({ emitEvent: false });
  }


  save() {
    this.util.markFormGroupTouched(this.employeeForm);
    if (this.employeeForm.invalid) {
      return;
    }
    Object.assign(this.employee, this.employeeForm.getRawValue());
    this.saving = true;
    this.ds.save(this.employee).pipe(
      finalize(() => {
        this.saving = false;
      })
    ).subscribe(() => {
      this.employeeForm.markAsPristine();
      this.ms.success("message.STD00006");
      this.router.navigate(['/db/dbrt08/detail'],{ replaceUrl : true,state:{ code : this.employee.employeeCode }});
    })
  }

  canDeactivate(): Observable<boolean> {
    if (!this.employeeForm.dirty) {
      return of(true);
    }
    return this.modal.confirm("message.STD00002");
  }

}
