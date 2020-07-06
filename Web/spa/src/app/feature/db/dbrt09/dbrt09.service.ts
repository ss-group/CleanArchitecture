import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbEducationType {
  educationTypeLevel: string,
  educationTypeLevelCodeMua: string,
  educationTypeLevelNameTha: string,
  educationTypeLevelNameEng: string,
  educationLevelShortNameTha: string,
  educationLevelShortNameEng: string,
  groupLevel : string,
  prefix : string,
  formatCode : string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt09Service {

  constructor(private http: HttpClient) { }
  getEducationTypies(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'educationTypeLevel'
    return this.http.get<any>('dbrt09', { params: filter });
  }

  getEducationType(code) {
    return this.http.get<DbEducationType>('dbrt09/detail', { params: { educationTypeLevel: code } });
  }

  save(educationtype: DbEducationType) {
    if (educationtype.rowVersion) {
      return this.http.put('dbrt09', educationtype);
    }
    else {
      return this.http.post('dbrt09', educationtype);
    }
  }
  getMaster() {
    return this.http.get<any>('dbrt09/master');
}
  delete(code, version) {
    return this.http.delete('dbrt09', { params: { educationTypeLevel: code, rowVersion: version } })
  }

}
