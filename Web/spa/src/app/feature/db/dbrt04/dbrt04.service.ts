import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbDistrict {
  districtId: number,
  provinceId: number,
  districtCode: string,
  districtCodeMua: string,
  districtNameTha: string,
  districtNameEng: string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt04Service {

  constructor(private http: HttpClient) { }

  getDistrict(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'districtId'
    return this.http.get<any>('dbrt04', { params: filter });
  }

  getDistrictType(code) {
    return this.http.get<DbDistrict>('dbrt04/detail', { params: { districtCode: code} });
  }
  save(districttype: DbDistrict) {
    if (districttype.rowVersion) {
      return this.http.put('dbrt04', districttype);
    }
    else {
      return this.http.post('dbrt04', districttype);
    }
  }

  getMaster() {
    return this.http.get<any>('dbrt04/master');
}

  delete(code, version) {
    return this.http.delete('dbrt04', { params: { districtId: code, rowVersion: version } })
  }

}
