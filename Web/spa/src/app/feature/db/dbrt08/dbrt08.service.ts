import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Employee {
  companyCode: string;
  employeeCode: string;
  personalId: string;
  preNameId: number;
  tFirstName: string;
  tMiddleName: string;
  tLastName: string;
  empTypeCode: string;
  jobType: string;
  positionDivision: string;
  positionLevelCode: string;
  positionCode: string;
  divCode: string;
  workDate: Date;
  gender: string;
  birthday: Date;
  ageDesc:number;
  addrName: string;
  moo: string;
  soi: string;
  street: string;
  subDistrictId: number;
  districtId: number;
  provinceId: number;
  postalCode: number;
  telNo: string;
  email: string;
  raceCode: string;
  nationCode: string;
  religionCode: string;
  turnoverDate: Date;
  eFirstName: string;
  eMiddleName: string;
  eLastName: string;
  tNameConcat: string;
  eNameConcat: string;
  groupTypeCode: string;
  imageId: number;
  divWorkId: string;
  teacherCode: string;
  rowVersion: number;

}

@Injectable()
export class Dbrt08Service {

  constructor(private http: HttpClient) { }

  getEmployees(search: any, page: any) {
    const filter = Object.assign(search, page);
    return this.http.get<any>('dbrt08', { params: filter });
  }

  getMaster(page) {
    return this.http.get<any>('dbrt08/master/' + page);
  }

  getDependencyMaster(name, params) {
    return this.http.get<any>('dbrt08/dependency', { params: Object.assign({ name: name }, params) });
  }

  getEmployee(code) {
    return this.http.get<Employee>('dbrt08/detail', { params: { employeeCode: code } });
  }
  save(employee: Employee) {
    if (employee.rowVersion) {
      return this.http.put('dbrt08', employee);
    }
    else {
      return this.http.post('dbrt08', employee);
    }
  }

}
