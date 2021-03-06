import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService, BaseList } from '@app/shared';

export interface DbEmployee {
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
  divWorkId : string;
  teacherCode : string;
  rowVersion: number;

}

@Injectable()
export class Dbrt08Service {

  constructor(private http: HttpClient) { }

  getEmployees(search: any, page: any) {
    const filter = Object.assign(search, page);
    filter.sort = page.sort || 'employeeCode'
    return this.http.get<any>('dbrt08', { params: filter });
  }

  getMaster(page) {
    return this.http.get<any>('dbrt08/master/' + page);
  }

  getDistrict(provinceId) {
    return this.http.get<any>('dbrt08/district', { params: { provinceId: provinceId } });
  }

  getSubDistrict(districtId) {
    return this.http.get<any>('dbrt08/subDistrict', { params: { districtId: districtId } });
  }

  getPostalCode(subDistrictId) {
    return this.http.get<any>('dbrt08/postalCode', { params: { subDistrictId: subDistrictId } });
  }

  getEmployee(code) {
    return this.http.get<DbEmployee>('dbrt08/detail', { params: { employeeCode: code } });
  }
  save(employee: DbEmployee) {
    if (employee.rowVersion) {
      return this.http.put('dbrt08', employee);
    }
    else {
      return this.http.post('dbrt08', employee);
    }
  }

}
