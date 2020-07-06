import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbFac {
  facCode: string,
  facNameTha: string,
  facNameEng: string,
  facShortNameTha: string,
  facShortNameEng: string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt11Service {

  constructor(private http: HttpClient) { }

  getFac(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'facCode'
    return this.http.get<any>('dbrt11', { params: filter });
  }

  getFacType(code) {
    return this.http.get<DbFac>('dbrt11/detail', { params: { facCode: code } });
  }
  save(factype: DbFac) {
    if (factype.rowVersion) {
      return this.http.put('dbrt11', factype);
    }
    else {
      return this.http.post('dbrt11', factype);
    }
  }

  delete(code, version) {
    return this.http.delete('dbrt11', { params: { facCode: code, rowVersion: version } })
  }

}
