import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbLocation {
  companyCode: number,
  locationCode: string,
  locationNameTha: string,
  locationNameEng: string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt16Service {

  constructor(private http: HttpClient) { }

  getLocation(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'locationCode'
    return this.http.get<any>('dbrt16', { params: filter });
  }

  getLocationDetail(code) {
    return this.http.get<DbLocation>('dbrt16/detail', { params: { locationCode: code } });
  }
  save(location: DbLocation) {
    if (location.rowVersion) {
      return this.http.put('dbrt16', location);
    }
    else {
      return this.http.post('dbrt16', location);
    }
  }

  delete(id,code,version) {
    return this.http.delete('dbrt16', { params: { companyCode: id, locationCode: code, rowVersion: version } })
  }

}
