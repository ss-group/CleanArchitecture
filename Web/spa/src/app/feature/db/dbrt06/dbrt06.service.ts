import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface DbPostalCode {
  postalCodeId: number,
  postalCode: string,
  subDistrictId: number,
  districtId: number,
  provinceId: number,
  postalNameTha: string,
  postalNameEng: string,
  active: boolean,
  rowVersion: number
}

@Injectable()
export class Dbrt06Service {

  constructor(private http: HttpClient) { }

  getPostalCode(search: string, page: any) {
    const filter = Object.assign({ keyword: search }, page);
    filter.sort = page.sort || 'postalCodeId'
    return this.http.get<any>('dbrt06', { params: filter });
  }

  getPostalType(code) {
    return this.http.get<DbPostalCode>('dbrt06/detail', { params: { PostalCodeId: code } });
  }

  save(postalList: DbPostalCode) {
    if (postalList.rowVersion) {
      return this.http.put('dbrt06', postalList);
    }
    else {
      return this.http.post('dbrt06', postalList);
    }
  }

  getMaster() {
    return this.http.get<any>('dbrt06/master');
  }
  getDistrict(provinceId) {
    return this.http.get<any>('dbrt06/district', { params: { provinceId: provinceId } });
  }

  getSubDistrict(districtId) {
    return this.http.get<any>('dbrt06/subDistrict', { params: { districtId: districtId } });
  }

  delete(code, version) {
    return this.http.delete('dbrt06', { params: { postalCodeId: code, rowVersion: version } })
  }

}
