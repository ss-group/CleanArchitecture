import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbSubDistrict {
  subDistrictId: number,
  provinceId: number,
  districtId: number,
  subDistrictCode: string,
  subDistrictCodeMua: string,
  subDistrictNameTha: string,
  subDistrictNameEng: string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt05Service {

  constructor(private http: HttpClient) { }

  getSubDistrict(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'subDistrictId', 'districtId', 'provinceId'
    return this.http.get<any>('dbrt05', { params: filter });
  }

  getSubDistrictType(code) {
    return this.http.get<DbSubDistrict>('dbrt05/detail', { params: { subDistrictCode: code } });
  }

  save(subDistrictList: DbSubDistrict) {
    if (subDistrictList.rowVersion) {
      return this.http.put('dbrt05', subDistrictList);
    }
    else {
      return this.http.post('dbrt05', subDistrictList);
    }
  }

  getMaster() {
    return this.http.get<any>('dbrt05/master');
  }

  delete(code, version) {
    return this.http.delete('dbrt05', { params: { subdistrictId: code, rowVersion: version } })
  }

  getDistrict(provinceId) {
    return this.http.get<any>('dbrt05/district', { params: { provinceId: provinceId } });
  }

}
