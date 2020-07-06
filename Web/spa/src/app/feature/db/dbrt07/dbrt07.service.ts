import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbPreNamae {
  preNameId: number,
  preNameTha: string,
  preNameEng: string,
  preNameShortTha: string,
  preNameShortEng: string,
  active: boolean,
  rowVersion: number,
  preNameType : string,
}

@Injectable()
export class Dbrt07Service {

  constructor(private http: HttpClient) { }
  getPreNames(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'preNameId'
    return this.http.get<any>('dbrt07', { params: filter });
  }

  getPreName(code) {
    return this.http.get<DbPreNamae>('dbrt07/detail', { params: { preNameId: code } });
  }

  save(premame: DbPreNamae) {
    if (premame.rowVersion) {
      return this.http.put('dbrt07', premame);
    }else {
      return this.http.post('dbrt07', premame);
    }
  }

  delete(code, version) {
    return this.http.delete('dbrt07', { params: { preNameId: code, rowVersion: version } })
  }
  getMaster() {
    return this.http.get<any>('dbrt07/master');
}
}
