import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbEducationTypes {
  educationTypeCode: string,
  educationTypeNameTha: string,
  educationTypeNameEng: string,
  educationShortNameTha: string,
  educationShortNameEng: string,
  typeLevel:string
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt10Service {

  constructor(private http: HttpClient) { }
  getEducationTypies(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'educationTypeCode'
    return this.http.get<any>('dbrt10', { params: filter });
  }

  getEducationTypie(code,fac) {
    return this.http.get<DbEducationTypes>('dbrt10/detail', { params: { educationTypeCode: code,typeLevel:fac } });
  }

  save(educationtype: DbEducationTypes) {
    if (educationtype.rowVersion) {
      return this.http.put('dbrt10', educationtype);
    }
    else {
      return this.http.post('dbrt10', educationtype);
    }
  }

  getMaster() {
    return this.http.get<any>('dbrt10/master');
}

  delete(code, version) {
    return this.http.delete('dbrt10', { params: { educationTypeCode: code, rowVersion: version } })
  }

}
