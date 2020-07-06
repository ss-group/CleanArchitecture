import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbDepartment {
  departmentCode: number,
  departmentCodeMua: string,
  departmentNameTha: string,
  departmentNameEng: string,
  departmentShortNameTha: string,
  departmentShortNameEng: string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt20Service {

  constructor(private http: HttpClient) { }

  getDepartment(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'departmentCode'
    return this.http.get<any>('dbrt20', { params: filter });
  }

  getDepartmentDetail(code) {
    return this.http.get<DbDepartment>('dbrt20/detail', { params: { departmentCode: code } });
  }
  save(department: DbDepartment) {
    if (department.rowVersion) {
      return this.http.put('dbrt20', department);
    }
    else {
      return this.http.post('dbrt20', department);
    }
  }

  delete(code, version) {
    return this.http.delete('dbrt20', { params: { departmentCode: code, rowVersion: version } })
  }

}
