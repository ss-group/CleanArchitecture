import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbDegree {
  degreeId: number,
  degreeNameTha: string,
  degreeNameEng: string,
  degreeShortNameTha: string,
  degreeShortNameEng: string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt19Service {

  constructor(private http: HttpClient) { }

  getDegree(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'degreeId'
    return this.http.get<any>('dbrt19', { params: filter });
  }

  getDegreeDetail(code) {
    return this.http.get<DbDegree>('dbrt19/detail', { params: { degreeId: code } });
  }
  save(degree: DbDegree) {
    if (degree.rowVersion) {
      return this.http.put('dbrt19', degree);
    }
    else {
      return this.http.post('dbrt19', degree);
    }
  }

  delete(id, version) {
    return this.http.delete('dbrt19', { params: { degreeId: id, rowVersion: version } })
  }

}
